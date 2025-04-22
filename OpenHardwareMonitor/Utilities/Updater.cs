using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Windows.Forms;

namespace sergiye.Common {
  public class GitHubRelease {
    public string Tag_name { get; set; }
    public string Name { get; set; }
    public bool Prerelease { get; set; }
    public Asset[] Assets { get; set; }
    //public Uri assets_url { get; set; }
    //public Uri html_url { get; set; }
    //public DateTime created_at { get; set; }
    //public DateTime published_at { get; set; }
  }

  public class Asset {
    public string Name { get; set; }
    public string Browser_download_url { get; set; }
    //public string State { get; set; }
    //public Uri Url { get; set; }
    //public object Label { get; set; }
    //public string Content_type { get; set; }
    //public long Size { get; set; }
    //public long Download_count { get; set; }
    //public DateTime Created_at { get; set; }
    //public DateTime Updated_at { get; set; }
  }

  internal static class Updater {
    internal static readonly string ApplicationName;
    internal static readonly string ApplicationTitle;
    internal static readonly string selfFileName;
    internal static readonly string CurrentVersion;
    internal static readonly string CurrentFileLocation;

    static Updater() {
      var asm = Assembly.GetExecutingAssembly(); //typeof(Updater).Assembly
      ApplicationName = asm.GetName().Name;
      ApplicationTitle = GetAttribute<AssemblyTitleAttribute>(asm)?.Title;
      CurrentVersion = asm.GetName().Version.ToString(3); //Application.ProductVersion;
      CurrentFileLocation = asm.Location;
      selfFileName = Path.GetFileName(CurrentFileLocation);
      ServicePointManager.Expect100Continue = false;
      ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
      ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
    }

    private static T GetAttribute<T>(ICustomAttributeProvider assembly, bool inherit = false) where T : Attribute {
      foreach (var o in assembly.GetCustomAttributes(typeof(T), inherit))
        if (o is T attribute)
          return attribute;
      return null;
    }

    /// <summary>
    /// Check for a new version
    /// </summary>
    /// <returns>True if the check was completed, False if there were errors</returns>
    internal static bool CheckForUpdates(bool silent) {
      string newVersion;
      string newVersionUrl = null;
      try {
        string jsonString;
        using (var wc = new WebClient()) {
          wc.Headers.Add("user-agent", "Mozilla/5.0 (Windows NT 11.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/100.0.500.27 Safari/537.36");
          jsonString = wc.DownloadString($"https://api.github.com/repos/sergiye/{ApplicationName}/releases").TrimEnd();
        }
        var releases = jsonString.FromJson<GitHubRelease[]>();
        if (releases == null || releases.Length == 0)
          throw new Exception("Error getting list of releases.");

        var lastRelease = releases.FirstOrDefault(r => !r.Prerelease) ?? releases[0];
        newVersion = lastRelease.Tag_name;
        var asset = lastRelease.Assets.FirstOrDefault(a => a.Name == selfFileName);
        newVersionUrl = asset?.Browser_download_url;
        if (string.IsNullOrEmpty(newVersionUrl)) {
          if (!silent)
            MessageBox.Show($"Your version is: {CurrentVersion}\nLatest released version is: {newVersion}\nNo assets found to update.", "Update", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          return true;
        }

        if (string.Compare(CurrentVersion, newVersion, StringComparison.Ordinal) >= 0) {
          if (!silent)
            MessageBox.Show($"Your version: {CurrentVersion}\nLast release: {newVersion}\nNo need to update.", "Update", MessageBoxButtons.OK,
              MessageBoxIcon.Information);
          return true;
        }
        if (MessageBox.Show($"Your version: {CurrentVersion}\nLast release: {newVersion}\nDownload this update?",
          "Update", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) {
          return true;
        }
      }
      catch (Exception ex) {
        if (!silent)
          MessageBox.Show($"Error checking for a new version.\n{ex.Message}", "Update", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        return false;
      }

      try {
        var tempPath = Path.GetTempPath();
        var updateFilePath = $"{tempPath}{selfFileName}";

        using (var wc = new WebClient())
          wc.DownloadFile(newVersionUrl, updateFilePath);
        RestartApp(3, updateFilePath);
        return true;
      }
      catch (Exception ex) {
        if (!silent)
          MessageBox.Show($"Error downloading new version\n{ex.Message}", "Update", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        return false;
      }
    }

    internal static void RestartApp(int timeout = 0, string replaceWithFile = null) {
      var cmdFilePath = Path.GetTempPath() + $"{selfFileName}_updater.cmd";
      using (var batFile = new StreamWriter(File.Create(cmdFilePath))) {
        batFile.WriteLine("@ECHO OFF");
        batFile.WriteLine($"TIMEOUT /t {timeout} /nobreak > NUL");
        batFile.WriteLine("TASKKILL /IM \"{0}\" > NUL", selfFileName);
        if (!string.IsNullOrEmpty(replaceWithFile))
          batFile.WriteLine("MOVE \"{0}\" \"{1}\"", replaceWithFile, CurrentFileLocation);
        batFile.WriteLine("DEL \"%~f0\" & START \"\" /B \"{0}\"", CurrentFileLocation);
      }
      var startInfo = new ProcessStartInfo(cmdFilePath) {
        CreateNoWindow = true,
        UseShellExecute = false,
        WorkingDirectory = Path.GetTempPath()
      };
      Process.Start(startInfo);
      Application.Exit(); // Environment.Exit(0);
    }
  }
}
