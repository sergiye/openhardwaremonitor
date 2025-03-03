using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using OpenHardwareMonitor.Hardware;
using OpenHardwareMonitor.UI;

namespace OpenHardwareMonitor.Utilities;

public class HttpServer
{
    private readonly HttpListener _listener;
    private readonly Node _root;
    private Thread _listenerThread;

    public HttpServer(Node node, string ip, int port, bool authEnabled = false, string userName = "", string password = "")
    {
        _root = node;
        ListenerIp = ip;
        ListenerPort = port;
        AuthEnabled = authEnabled;
        UserName = userName;
        Password = password;

        try
        {
            _listener = new HttpListener { IgnoreWriteExceptions = true };
        }
        catch (PlatformNotSupportedException)
        {
            _listener = null;
        }
    }

    ~HttpServer()
    {
        if (PlatformNotSupported)
            return;

        StopHttpListener();
        _listener.Abort();
    }

    public bool AuthEnabled { get; set; }

    public string ListenerIp { get; set; }

    public int ListenerPort { get; set; }

    public string Password
    {
        get { return PasswordSHA256; }
        set { PasswordSHA256 = ComputeSHA256(value); }
    }

    public bool PlatformNotSupported
    {
        get { return _listener == null; }
    }

    public string UserName { get; set; }

    private string PasswordSHA256 { get; set; }

    public bool StartHttpListener()
    {
        if (PlatformNotSupported)
            return false;

        try
        {
            if (_listener.IsListening)
                return true;

            // validate that the selected IP exists (it could have been previously selected before switching networks)
            IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());
            bool ipFound = false;
            foreach (IPAddress ip in host.AddressList)
            {
                if (ListenerIp == ip.ToString())
                {
                    ipFound = true;
                    break;
                }
            }

            if (!ipFound)
            {
                // default to behavior of previous version if we don't know what interface to use.
                ListenerIp = "+";
            }

            string prefix = "http://" + ListenerIp + ":" + ListenerPort + "/";

            _listener.Prefixes.Clear();
            _listener.Prefixes.Add(prefix);
            _listener.Realm = "Open Hardware Monitor";
            _listener.AuthenticationSchemes = AuthEnabled ? AuthenticationSchemes.Basic : AuthenticationSchemes.Anonymous;
            _listener.Start();

            if (_listenerThread == null)
            {
                _listenerThread = new Thread(HandleRequests);
                _listenerThread.Start();
            }
        }
        catch (Exception)
        {
            return false;
        }

        return true;
    }

    public bool StopHttpListener()
    {
        if (PlatformNotSupported)
            return false;

        try
        {
            _listenerThread?.Abort();
            _listener.Stop();
            _listenerThread = null;
        }
        catch (HttpListenerException)
        { }
        catch (ThreadAbortException)
        { }
        catch (NullReferenceException)
        { }
        catch (Exception)
        { }

        return true;
    }

    private void HandleRequests()
    {
        while (_listener.IsListening)
        {
            IAsyncResult context = _listener.BeginGetContext(ListenerCallback, _listener);
            context.AsyncWaitHandle.WaitOne();
        }
    }

    private void ListenerCallback(IAsyncResult result)
    {
        HttpListener listener = (HttpListener)result.AsyncState;
        if (listener == null || !listener.IsListening)
            return;

        // Call EndGetContext to complete the asynchronous operation.
        HttpListenerContext context;
        try
        {
            context = listener.EndGetContext(result);
        }
        catch (Exception)
        {
            return;
        }

        HttpListenerRequest request = context.Request;

        bool authenticated;

        if (AuthEnabled)
        {
            try
            {
                HttpListenerBasicIdentity identity = (HttpListenerBasicIdentity)context.User.Identity;
                authenticated = identity.Name == UserName & ComputeSHA256(identity.Password) == Password;
            }
            catch
            {
                authenticated = false;
            }
        }
        else
        {
            authenticated = true;
        }

        if (authenticated)
        {
            switch (request.HttpMethod)
            {
                case "GET":
                    {
                        string requestedFile = request.RawUrl.Substring(1);

                        if (requestedFile == "data.json")
                        {
                            SendJson(context.Response, request);
                            return;
                        }

                        if (requestedFile.Contains("images_icon"))
                        {
                            ServeResourceImage(context.Response,
                                               requestedFile.Replace("images_icon/", string.Empty));

                            return;
                        }

                        // default file to be served
                        if (string.IsNullOrEmpty(requestedFile))
                            requestedFile = "index.html";

                        string[] splits = requestedFile.Split('.');
                        string ext = splits[splits.Length - 1];
                        ServeResourceFile(context.Response, "Web." + requestedFile.Replace('/', '.'), ext);

                        break;
                    }
                default:
                    {
                        context.Response.StatusCode = 404;
                        break;
                    }
            }
        }
        else
        {
            context.Response.StatusCode = 401;
        }

        if (context.Response.StatusCode == 401)
        {
            const string responseString = @"<HTML><HEAD><TITLE>401 Unauthorized</TITLE></HEAD>
  <BODY><H4>401 Unauthorized</H4>
  Authorization required.</BODY></HTML> ";

            byte[] buffer = Encoding.UTF8.GetBytes(responseString);
            context.Response.ContentLength64 = buffer.Length;
            context.Response.StatusCode = 401;
            Stream output = context.Response.OutputStream;
            output.Write(buffer, 0, buffer.Length);
            output.Close();
        }

        try
        {
            context.Response.Close();
        }
        catch
        {
            // client closed connection before the content was sent
        }
    }

    private void ServeResourceFile(HttpListenerResponse response, string name, string ext)
    {
        // resource names do not support the hyphen
        name = "OpenHardwareMonitor.Resources." +
               name.Replace("custom-theme", "custom_theme");

        string[] names = Assembly.GetExecutingAssembly().GetManifestResourceNames();

        for (int i = 0; i < names.Length; i++)
        {
            if (names[i].Replace('\\', '.') == name)
            {
                using Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(names[i]);

                response.ContentType = GetContentType("." + ext);
                response.ContentLength64 = stream.Length;
                byte[] buffer = new byte[512 * 1024];
                try
                {
                    Stream output = response.OutputStream;
                    int len;
                    while ((len = stream.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        output.Write(buffer, 0, len);
                    }

                    output.Flush();
                    output.Close();
                    response.Close();
                }
                catch (HttpListenerException)
                { }
                catch (InvalidOperationException)
                { }

                return;
            }
        }

        response.StatusCode = 404;
        response.Close();
    }

    private void ServeResourceImage(HttpListenerResponse response, string name)
    {
        name = "OpenHardwareMonitor.Resources." + name;

        string[] names = Assembly.GetExecutingAssembly().GetManifestResourceNames();

        for (int i = 0; i < names.Length; i++)
        {
            if (names[i].Replace('\\', '.') == name)
            {
                using Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(names[i]);

                Image image = Image.FromStream(stream);
                response.ContentType = "image/png";
                try
                {
                    Stream output = response.OutputStream;
                    using (MemoryStream ms = new())
                    {
                        image.Save(ms, ImageFormat.Png);
                        ms.WriteTo(output);
                    }

                    output.Close();
                }
                catch (HttpListenerException)
                { }

                image.Dispose();
                response.Close();
                return;
            }
        }

        response.StatusCode = 404;
        response.Close();
    }

    private void SendJson(HttpListenerResponse response, HttpListenerRequest request = null)
    {
        int nodeIndex = 0;
        var json = new
        {
            id = nodeIndex++,
            Text = "Sensor",
            Min = "Min",
            Value = "Value",
            Max = "Max",
            ImageURL = string.Empty,
            Children = new[]
            {
                GenerateJsonForNode(_root, ref nodeIndex)
            }
        };

        string responseContent = json.ToJson();
        byte[] buffer = Encoding.UTF8.GetBytes(responseContent);

        bool acceptGzip;
        try
        {
            acceptGzip = request != null && request.Headers["Accept-Encoding"].ToLower().IndexOf("gzip", StringComparison.OrdinalIgnoreCase) >= 0;
        }
        catch
        {
            acceptGzip = false;
        }

        if (acceptGzip)
            response.AddHeader("Content-Encoding", "gzip");

        response.AddHeader("Cache-Control", "no-cache");
        response.AddHeader("Access-Control-Allow-Origin", "*");
        response.ContentType = "application/json";
        try
        {
            if (acceptGzip)
            {
                using var ms = new MemoryStream();
                using (var zip = new GZipStream(ms, CompressionMode.Compress, true))
                    zip.Write(buffer, 0, buffer.Length);

                buffer = ms.ToArray();
            }

            response.ContentLength64 = buffer.Length;
            Stream output = response.OutputStream;
            output.Write(buffer, 0, buffer.Length);
            output.Close();
        }
        catch (HttpListenerException)
        { }

        response.Close();
    }

    private object GenerateJsonForNode(Node n, ref int nodeIndex)
    {
        string imageUrl;

        var children = new List<object>();
        foreach (Node child in n.Nodes)
        {
            if (child is SensorNode sn && !sn.IsVisible)
                continue;
            var childNode = GenerateJsonForNode(child, ref nodeIndex);
            if (childNode != null)
                children.Add(childNode);
        }

        if (n is not SensorNode && children.Count == 0)
            return null;

        if (n is SensorNode sensorNode)
        {
            return new
            {
                id = nodeIndex++,
                Text = n.Text,
                SensorId = sensorNode.Sensor.Identifier.ToString(),
                Type = sensorNode.Sensor.SensorType.ToString(),
                Min = sensorNode.Min,
                Value = sensorNode.Value,
                Max = sensorNode.Max,
                ImageURL = "images/transparent.png",
                Children = children.ToArray(),
            };
        }
        else if (n is HardwareNode hardwareNode)
        {
            imageUrl = "images_icon/" + GetHardwareImageFile(hardwareNode);
        }
        else if (n is TypeNode typeNode)
        {
            imageUrl = "images_icon/" + GetTypeImageFile(typeNode);
        }
        else
        {
            imageUrl = "images_icon/computer.png";
        }

        return new
        {
            id = nodeIndex++,
            Text = n.Text,
            Min = string.Empty,
            Value = string.Empty,
            Max = string.Empty,
            Children = children.ToArray(),
            ImageURL = imageUrl,
        };
    }

    private static string GetContentType(string extension)
    {
        switch (extension)
        {
            case ".avi": return "video/x-msvideo";
            case ".css": return "text/css";
            case ".doc": return "application/msword";
            case ".gif": return "image/gif";
            case ".htm":
            case ".html": return "text/html";
            case ".jpg":
            case ".jpeg": return "image/jpeg";
            case ".js": return "application/x-javascript";
            case ".mp3": return "audio/mpeg";
            case ".png": return "image/png";
            case ".pdf": return "application/pdf";
            case ".ppt": return "application/vnd.ms-powerpoint";
            case ".zip": return "application/zip";
            case ".txt": return "text/plain";
            default: return "application/octet-stream";
        }
    }

    private static string GetHardwareImageFile(HardwareNode hn)
    {
        switch (hn.Hardware.HardwareType)
        {
            case HardwareType.Cpu:
                return "cpu.png";
            case HardwareType.GpuNvidia:
                return "nvidia.png";
            case HardwareType.GpuAmd:
                return "ati.png";
            case HardwareType.GpuIntel:
                return "intel.png";
            case HardwareType.Storage:
                return "hdd.png";
            case HardwareType.Motherboard:
                return "mainboard.png";
            case HardwareType.SuperIO:
                return "chip.png";
            case HardwareType.Memory:
                return "ram.png";
            case HardwareType.Cooler:
                return "fan.png";
            case HardwareType.Network:
                return "nic.png";
            case HardwareType.Psu:
                return "power-supply.png";
            case HardwareType.Battery:
                return "battery.png";
            default:
                return "cpu.png";
        }
    }

    private static string GetTypeImageFile(TypeNode tn)
    {
        switch (tn.SensorType)
        {
            case SensorType.Voltage:
            case SensorType.Current:
                return "voltage.png";
            case SensorType.Clock:
                return "clock.png";
            case SensorType.Load:
                return "load.png";
            case SensorType.Temperature:
                return "temperature.png";
            case SensorType.Fan:
                return "fan.png";
            case SensorType.Flow:
                return "flow.png";
            case SensorType.Control:
                return "control.png";
            case SensorType.Level:
                return "level.png";
            case SensorType.Power:
                return "power.png";
            case SensorType.Noise:
                return "loudspeaker.png";
            case SensorType.Conductivity:
                return "voltage.png";
            case SensorType.Throughput:
                return "throughput.png";
            case SensorType.Humidity:
                return "flow.png";
            default:
                return "power.png";
        }
    }

    private string ComputeSHA256(string text)
    {
        using SHA256 hash = SHA256.Create();
        return string.Concat(hash
                            .ComputeHash(Encoding.UTF8.GetBytes(text))
                            .Select(item => item.ToString("x2")));
    }

    public void Quit()
    {
        if (PlatformNotSupported)
            return;

        StopHttpListener();
        _listener.Abort();
    }
}
