﻿using Golden_Ticket.Utilities;
using System;
using System.Net;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;
using System.Drawing.Drawing2D;

namespace Golden_Ticket.Windows
{
    public partial class UpdateWindow : Form
    {
        UpdateChecker updateChecker = new UpdateChecker();
        PathUtils pathUtils = new PathUtils();

        public UpdateWindow()
        {
            InitializeComponent();
        }

        private void UpdateWindow_Load(object sender, EventArgs e)
        {
            CurrentVersionLabel.Text = "Current version: " + updateChecker.LauncherVersion;
            NewVersionLabel.Text = "New version: " + updateChecker.JsonVersion;
            DownloadUpdate();
        }

        void DownloadUpdate()
        {
            WebClient client = new WebClient();
            client.DownloadProgressChanged += new DownloadProgressChangedEventHandler(client_DownloadProgressChanged);
            client.DownloadFileCompleted += new AsyncCompletedEventHandler(client_DownloadFileCompleted);

            client.DownloadFileAsync(new Uri(updateChecker.DownloadURL), pathUtils.goldenTicketTempFolder + "\\Update.exe");
        }

        private void client_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            // Start the installer
            Process.Start(pathUtils.goldenTicketTempFolder + "\\Update.exe" + "/S");
            // Close the launcher
            Application.Exit();
        }

        private void client_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            double bytesIn = double.Parse(e.BytesReceived.ToString());
            double totalBytes = double.Parse(e.TotalBytesToReceive.ToString());
            double percentage = bytesIn / totalBytes * 100;

            DownloadProgressBar.Value = e.ProgressPercentage;
        }
    }
}
