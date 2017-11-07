using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ner71
{
    public partial class TranferringReport : Form {
        private string m_File;

        public TranferringReport(string m_BucketPath)
        {
            InitializeComponent();

            new Thread(() => {
                m_File = Path.Combine(m_BucketPath, "..", Text + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".zip");
                ZipFile.CreateFromDirectory(m_BucketPath, m_File, CompressionLevel.Optimal, false);
                Invoke(new Action(() => {
                    label1.Text = "ü";
                    label4.Text = "è";
                    progressBar1.Style = ProgressBarStyle.Continuous;
                }));

                var webClient = new WebClient();
                webClient.UploadProgressChanged += (sender, args) => {
                    Invoke(new Action(() => {
                        progressBar1.Value = args.ProgressPercentage;
                        label5.Text = String.Format("Transferred {0:0.00}MB of {1:0.00}MB", args.BytesSent / 1024f / 1024f, args.TotalBytesToSend / 1024f / 1024f);
                    }));
                };
                webClient.UploadFileCompleted += (sender, args) => Close();
                webClient.UploadFileAsync(Properties.Settings.Default.Ner71_ErrorReportUri, "POST", m_File);
            }).Start();
        }

        private void TranferringReport_FormClosing(object sender, FormClosingEventArgs e)
        {
            File.Delete(m_File);
        }

        private void button1_Click(object sender, EventArgs e) {
            Close();
        }
    }
}
