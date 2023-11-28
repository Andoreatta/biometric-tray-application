using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace BiometricTrayApplicationWinForms
{
    internal static class Program
    {
        private static Process? biometricServiceProcess;

        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();

            NotifyIcon notifyIcon = new NotifyIcon
            {
                Icon = SystemIcons.Application,
                Visible = true
            };

            ContextMenuStrip contextMenuStrip = new();
            ToolStripMenuItem initializeMenuItem = new("Iniciar API");
            ToolStripMenuItem stopMenuItem = new("Parar API");
            ToolStripMenuItem exitMenuItem = new("Sair");
            contextMenuStrip.Items.Add(initializeMenuItem);
            contextMenuStrip.Items.Add(stopMenuItem);
            contextMenuStrip.Items.Add(exitMenuItem);

            notifyIcon.ContextMenuStrip = contextMenuStrip;

            initializeMenuItem.Enabled = true;
            stopMenuItem.Enabled = false;

            initializeMenuItem.Click += (sender, e) =>
            {
                initializeMenuItem.Enabled = false;
                stopMenuItem.Enabled = true;

                string currentDirectory = Directory.GetCurrentDirectory();
                string biometricServicePath = Path.Combine(currentDirectory, "BiometricService", "BiometricService.exe");

                biometricServiceProcess = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = biometricServicePath,
                        UseShellExecute = false,
                        CreateNoWindow = true
                    }
                };
                biometricServiceProcess.Start();
            };

            stopMenuItem.Click += (sender, e) =>
            {
                initializeMenuItem.Enabled = true;
                stopMenuItem.Enabled = false;

                biometricServiceProcess?.Kill();
                biometricServiceProcess = null;
            };

            exitMenuItem.Click += (sender, e) =>
            {
                biometricServiceProcess?.Kill();
                Application.Exit();
            };

            Application.Run();
        }
    }
}