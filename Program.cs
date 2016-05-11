using System;
using System.Diagnostics;
using System.Windows.Forms;
using Microsoft.Win32;

namespace NoDtarVisualStudioVersionSelector
{
    static class Program
    {
        static string appName = "VSLauncher.exe";

        [STAThread]
        static void Main()
        {
            string[] args = Environment.GetCommandLineArgs();
            if (args.Length < 2)
            {
                return;
            }

            string txt = Environment.GetFolderPath(Environment.SpecialFolder.Windows);
            Environment.CurrentDirectory = txt;

            string slnPath = string.Format("\"{0}\"", args[1]);

            string vscmd = GetVSLauncherCmd();
            if (string.IsNullOrEmpty(vscmd) == true)
            {
                MessageBox.Show("NOT Found: " + appName);
                return;
            }

            Process.Start(vscmd, slnPath);   
        }

        private static string GetVSLauncherCmd()
        {
            using (RegistryKey regKey = Registry.ClassesRoot.OpenSubKey(@"Applications\" + appName + @"\Shell\Open\Command"))
            {
                string path = regKey.GetValue(null) as string;

                int idx = path.IndexOf(appName, 0, path.Length, StringComparison.OrdinalIgnoreCase);
                if (idx == -1)
                {
                    return null;
                }

                int separatorPos = path.IndexOf(' ', idx);
                if (separatorPos == -1)
                {
                    return path;
                }

                return path.Substring(0, separatorPos);
            }
        }
    }
}
