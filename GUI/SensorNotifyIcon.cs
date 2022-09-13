using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using OpenHardwareMonitor.Hardware;
using OpenHardwareMonitor.Utilities;

namespace OpenHardwareMonitor.GUI {
  public class SensorNotifyIcon : IDisposable {

    private readonly UnitManager unitManager;

    private readonly ISensor sensor;
    private readonly NotifyIconAdv notifyIcon;
    private readonly Bitmap bitmap;
    private readonly Graphics graphics;
    private Color color;
    private Color darkColor;
    private Brush brush;
    private Brush darkBrush;
    private readonly Pen pen;
    private readonly Font font;
    private readonly Font smallFont;

    public SensorNotifyIcon(SystemTray sensorSystemTray, ISensor sensor,
      PersistentSettings settings, UnitManager unitManager) {

      this.unitManager = unitManager;
      this.sensor = sensor;
      notifyIcon = new NotifyIconAdv();

      //todo: set defaultColor depending on the taskbar color
      var defaultColor = Color.FromArgb(0xff, 0x00, 0xff, 0xff);
      //if (sensor.SensorType == SensorType.Load ||
      //    sensor.SensorType == SensorType.Control ||
      //    sensor.SensorType == SensorType.Level)
      //  defaultColor = Color.FromArgb(0xff, 0x70, 0x8c, 0xf1);

      Color = settings.GetValue(new Identifier(sensor.Identifier, "traycolor").ToString(), defaultColor);

      pen = new Pen(Color.FromArgb(96, Color.Black));

      var contextMenu = new ContextMenu();
      var hideShowItem = new MenuItem("Hide/Show");
      hideShowItem.DefaultItem = true;
      hideShowItem.Click += (sender, args) => sensorSystemTray.SendHideShowCommand();
      contextMenu.MenuItems.Add(hideShowItem);
      contextMenu.MenuItems.Add(new MenuItem("-"));
      var removeItem = new MenuItem("Remove Sensor");
      removeItem.Click += (sender, args) => sensorSystemTray.Remove(this.sensor);
      contextMenu.MenuItems.Add(removeItem);
      var colorItem = new MenuItem("Change Color...");
      colorItem.Click += (obj, args) => {
        var dialog = new ColorDialog();
        dialog.Color = Color;
        if (dialog.ShowDialog() == DialogResult.OK) {
          Color = dialog.Color;
          settings.SetValue(new Identifier(sensor.Identifier, "traycolor").ToString(), Color);
        }
      };
      contextMenu.MenuItems.Add(colorItem);
      contextMenu.MenuItems.Add(new MenuItem("-"));
      var exitItem = new MenuItem("Exit");
      exitItem.Click += (obj, args) => sensorSystemTray.SendExitCommand();
      contextMenu.MenuItems.Add(exitItem);
      notifyIcon.ContextMenu = contextMenu;
      notifyIcon.DoubleClick += (obj, args) => sensorSystemTray.SendHideShowCommand();

      // get the default dpi to create an icon with the correct size
      float dpiX, dpiY;
      using (var b = new Bitmap(1, 1, PixelFormat.Format32bppArgb)) {
        dpiX = b.HorizontalResolution;
        dpiY = b.VerticalResolution;
      }

      // adjust the size of the icon to current dpi (default is 16x16 at 96 dpi)
      var width = (int)Math.Round(16 * dpiX / 96);
      var height = (int)Math.Round(16 * dpiY / 96);

      // make sure it does never get smaller than 16x16
      width = width < 16 ? 16 : width;
      height = height < 16 ? 16 : height;

      // adjust the font size to the icon size
      FontFamily family;
      float baseSize;
      if (IsFontInstalled("Calibri", 15)) {
        family = new FontFamily("Calibri");
        baseSize = 15;
      } else if (IsFontInstalled("Segoe UI", 15)) {
        family = new FontFamily("Segoe UI");
        baseSize = 15;
      } else {
        family = new FontFamily("Tahome");// SystemFonts.MessageBoxFont.FontFamily;
        baseSize = 14;
      }

      font = new Font(family, baseSize * width / 16.0f, GraphicsUnit.Pixel);
      smallFont = new Font(family, 0.85f * baseSize * width / 16.0f, GraphicsUnit.Pixel);

      bitmap = new Bitmap(width, height, PixelFormat.Format32bppArgb);
      graphics = Graphics.FromImage(bitmap);

      if (Environment.OSVersion.Version.Major > 5) {
        graphics.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
        graphics.SmoothingMode = SmoothingMode.HighQuality;
      }
    }

    public ISensor Sensor => sensor;

    public Color Color {
      get => color;
      set {
        color = value;
        darkColor = Color.FromArgb(255,
          color.R / 3,
          color.G / 3,
          color.B / 3);
        var tmpBrush = brush;
        brush = new SolidBrush(color);
        if (tmpBrush != null)
          tmpBrush.Dispose();
        var tmpDarkBrush = darkBrush;
        darkBrush = new SolidBrush(darkColor);
        if (tmpDarkBrush != null)
          tmpDarkBrush.Dispose();
      }
    }

    public void Dispose() {
      var icon = notifyIcon.Icon;
      notifyIcon.Icon = null;
      if (icon != null)
        icon.Dispose();
      notifyIcon.Dispose();

      if (brush != null)
        brush.Dispose();
      if (darkBrush != null)
        darkBrush.Dispose();
      pen.Dispose();
      graphics.Dispose();
      bitmap.Dispose();
      font.Dispose();
      smallFont.Dispose();
    }

    private string GetString() {
      if (!sensor.Value.HasValue)
        return "-";

      switch (sensor.SensorType) {
        case SensorType.Voltage:
          return $"{sensor.Value:F1}";
        case SensorType.Clock:
          return $"{1e-3f * sensor.Value:F1}";
        case SensorType.Load:
          return $"{sensor.Value:F0}";
        case SensorType.Temperature:
          if (unitManager.TemperatureUnit == TemperatureUnit.Fahrenheit)
            return $"{UnitManager.CelsiusToFahrenheit(sensor.Value):F0}";
          else
            return $"{sensor.Value:F0}";
        case SensorType.Fan:
          return $"{1e-3f * sensor.Value:F1}";
        case SensorType.Flow:
          return $"{1e-3f * sensor.Value:F1}";
        case SensorType.Control:
          return $"{sensor.Value:F0}";
        case SensorType.Level:
          return $"{sensor.Value:F0}";
        case SensorType.Power:
        case SensorType.Data:
          return sensor.Value.Value < 10
            ? $"{sensor.Value:0.00}".Substring(0, 3)
            : $"{sensor.Value:F0}";
        case SensorType.Factor:
          return $"{sensor.Value:F1}";
      }
      return "-";
    }

    private bool IsFontInstalled(string fontName, float fontSize = 12) {
      using (Font fontTester = new Font(fontName, fontSize, FontStyle.Regular, GraphicsUnit.Pixel)) {
        return fontTester.Name == fontName;
      }
    }
    private Icon CreateTransparentIcon() {
      var text = GetString();
      var small = text.Length > 2;

      var defaultBackColor = Color.Transparent;
      var transparentIcon = false;
      try {
        graphics.Clear(defaultBackColor);
      } catch (Exception) {
        try {
          defaultBackColor = SystemTools.GetTaskbarColor();
        }
        catch (Exception) {
          defaultBackColor = Color.Black;
          transparentIcon = true;
        }
        graphics.Clear(defaultBackColor);
      }

      TextRenderer.DrawText(graphics, text, small ? smallFont : font,
        new Point(-4, small ? 1 : -2), color, defaultBackColor);

      var data = bitmap.LockBits(
        new Rectangle(0, 0, bitmap.Width, bitmap.Height),
        ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);

      var scan0 = data.Scan0;

      var numBytes = bitmap.Width * bitmap.Height * 4;
      var bytes = new byte[numBytes];
      Marshal.Copy(scan0, bytes, 0, numBytes);
      bitmap.UnlockBits(data);

      if (transparentIcon) {
        for (var i = 0; i < bytes.Length; i += 4) {
          var blue = bytes[i];
          var green = bytes[i + 1];
          var red = bytes[i + 2];

          bytes[i] = color.B;
          bytes[i + 1] = color.G;
          bytes[i + 2] = color.R;
          bytes[i + 3] = (byte)(0.3 * red + 0.59 * green + 0.11 * blue);
        }
      }

      return IconFactory.Create(bytes, bitmap.Width, bitmap.Height,
        PixelFormat.Format32bppArgb);
    }

    private Icon CreatePercentageIcon() {
      try {
        graphics.Clear(Color.Transparent);
      } catch (ArgumentException) {
        graphics.Clear(Color.Black);
      }
      graphics.FillRectangle(darkBrush, 0.5f, -0.5f, bitmap.Width - 2, bitmap.Height);
      var value = sensor.Value.GetValueOrDefault();
      var y = 0.16f * (100 - value);
      graphics.FillRectangle(brush, 0.5f, -0.5f + y, bitmap.Width - 2, bitmap.Height - y);
      graphics.DrawRectangle(pen, 1, 0, bitmap.Width - 3, bitmap.Height - 1);

      var data = bitmap.LockBits(
        new Rectangle(0, 0, bitmap.Width, bitmap.Height),
        ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
      var bytes = new byte[bitmap.Width * bitmap.Height * 4];
      Marshal.Copy(data.Scan0, bytes, 0, bytes.Length);
      bitmap.UnlockBits(data);

      return IconFactory.Create(bytes, bitmap.Width, bitmap.Height,
        PixelFormat.Format32bppArgb);
    }

    public void Update() {
      var icon = notifyIcon.Icon;

      switch (sensor.SensorType) {
        case SensorType.Load:
        case SensorType.Control:
        case SensorType.Level:
          notifyIcon.Icon = CreatePercentageIcon();
          break;
        default:
          notifyIcon.Icon = CreateTransparentIcon();
          break;
      }

      if (icon != null)
        icon.Dispose();

      var format = "";
      switch (sensor.SensorType) {
        case SensorType.Voltage: format = "\n{0}: {1:F2} V"; break;
        case SensorType.Clock: format = "\n{0}: {1:F0} MHz"; break;
        case SensorType.Load: format = "\n{0}: {1:F1} %"; break;
        case SensorType.Temperature: format = "\n{0}: {1:F1} °C"; break;
        case SensorType.Fan: format = "\n{0}: {1:F0} RPM"; break;
        case SensorType.Flow: format = "\n{0}: {1:F0} L/h"; break;
        case SensorType.Control: format = "\n{0}: {1:F1} %"; break;
        case SensorType.Level: format = "\n{0}: {1:F1} %"; break;
        case SensorType.Power: format = "\n{0}: {1:F0} W"; break;
        case SensorType.Data: format = "\n{0}: {1:F0} GB"; break;
        case SensorType.Factor: format = "\n{0}: {1:F3} GB"; break;
      }
      var formattedValue = string.Format(format, sensor.Name, sensor.Value);

      if (sensor.SensorType == SensorType.Temperature &&
        unitManager.TemperatureUnit == TemperatureUnit.Fahrenheit)
      {
        format = "\n{0}: {1:F1} °F";
        formattedValue = string.Format(format, sensor.Name,
          UnitManager.CelsiusToFahrenheit(sensor.Value));
      }

      var hardwareName = sensor.Hardware.Name;
      hardwareName = hardwareName.Substring(0,
        Math.Min(63 - formattedValue.Length, hardwareName.Length));
      var text = hardwareName + formattedValue;
      if (text.Length > 63)
        text = null;

      notifyIcon.Text = text;
      notifyIcon.Visible = true;
    }
  }
}
