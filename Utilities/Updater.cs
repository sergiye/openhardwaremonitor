using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace OpenHardwareMonitor.Utilities {

  public class GitHubRelease {
    public Uri assets_url { get; set; }
    public Uri html_url { get; set; }
    public string tag_name { get; set; }
    public string name { get; set; }
    public bool prerelease { get; set; }
    public DateTime created_at { get; set; }
    public DateTime published_at { get; set; }
    public Asset[] assets { get; set; }
  }

  public class Asset {
    public Uri url { get; set; }
    public string name { get; set; }
    public object label { get; set; }
    public string content_type { get; set; }
    public string state { get; set; }
    public long size { get; set; }
    public long download_count { get; set; }
    public DateTime created_at { get; set; }
    public DateTime updated_at { get; set; }
    public string browser_download_url { get; set; }
  }

  internal static class Updater {

    private const string GITHUB_LANDING_PAGE = "sergiye/openhardwaremonitor";
    private static readonly string selfFileName;

    internal static readonly string CurrentVersion;
    internal static readonly string CurrentFileLocation;

    static Updater() {
      var asm = Assembly.GetExecutingAssembly();
      CurrentVersion = asm.GetName().Version.ToString(3); //Application.ProductVersion;
      CurrentFileLocation = asm.Location;
      selfFileName = Path.GetFileName(CurrentFileLocation);
    }

    private static string GetJsonData(string uri, int timeout = 10, string method = "GET") {
      var request = (HttpWebRequest)WebRequest.Create(uri);
      request.Method = method;
      request.Timeout = timeout * 1000;
      request.UserAgent = "Mozilla/5.0 (Windows NT 11.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/100.0.500.27 Safari/537.36";
      //request.Accept = "text/xml,text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
      request.ContentType = "application/json; charset=utf-8";
      using (var webResp = request.GetResponse()) {
        using (var stream = webResp.GetResponseStream()) {
          if (stream == null) return null;
          var answer = new StreamReader(stream, Encoding.UTF8);
          var result = answer.ReadToEnd();
          return result;
        }
      }
    }

    internal static void CheckForUpdates(bool silent) {
      string newVersion;
      string newVersionUrl = null;
      bool update;
      try {
        //using (var wc = new WebClient())
        //  newVersion = wc.DownloadString($"https://raw.githubusercontent.com/{GithubLandingPage}/master/version.txt").TrimEnd();
        //newVersionUrl = $"https://github.com/{GithubLandingPage}/releases/download/{newVersion}/{selfFileName}";

        var jsonString = GetJsonData($"https://api.github.com/repos/{GITHUB_LANDING_PAGE}/releases").TrimEnd();
        var releases = JsonConvert.DeserializeObject<GitHubRelease[]>(jsonString);
        if (releases == null || releases.Length == 0)
          throw new Exception("Error getting list of releases.");

        newVersion = releases[0].tag_name;
        newVersionUrl = releases[0].assets[0].browser_download_url;

        if (string.Compare(CurrentVersion, newVersion, StringComparison.Ordinal) >= 0) {
          if (!silent)
            MessageBox.Show($"Your version is: {CurrentVersion}\nLatest released version is: {newVersion}\nNo need to update.", "Update", MessageBoxButtons.OK,
              MessageBoxIcon.Information);
          return;
        }
        update = MessageBox.Show($"Your version is: {CurrentVersion}\nLatest released version is: {newVersion}\n. Download this update?",
          "Update", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;
      } catch (Exception ex) {
        if (!silent)
          MessageBox.Show($"Error checking for a new version.\n{ex.Message}", "Update", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        update = false;
      }
      if (!update) return;

      try {
        var tempPath = Path.GetTempPath();
        var updateFilePath = $"{tempPath}{selfFileName}";

        using (var wc = new WebClient())
          wc.DownloadFile(newVersionUrl, updateFilePath);

        var cmdFilePath = Path.GetTempPath() + $"{selfFileName}_updater.cmd";
        using (var batFile = new StreamWriter(File.Create(cmdFilePath))) {
          batFile.WriteLine("@ECHO OFF");
          batFile.WriteLine("TIMEOUT /t 3 /nobreak > NUL");
          batFile.WriteLine("TASKKILL /IM \"{0}\" > NUL", selfFileName);
          batFile.WriteLine("MOVE \"{0}\" \"{1}\"", updateFilePath, CurrentFileLocation);
          batFile.WriteLine("DEL \"%~f0\" & START \"\" /B \"{0}\"", CurrentFileLocation);
        }
        var startInfo = new ProcessStartInfo(cmdFilePath) {
          CreateNoWindow = true,
          UseShellExecute = false,
          WorkingDirectory = tempPath
        };
        Process.Start(startInfo);
        Application.Exit(); // Environment.Exit(0);
      } catch (Exception ex) {
        if (!silent)
          MessageBox.Show($"Error downloading new version\n{ex.Message}", "Update", MessageBoxButtons.OK, MessageBoxIcon.Warning);
      }
    }
  }
}
