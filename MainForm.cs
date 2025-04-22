using System;
using System.Windows.Forms;
using WebCrawlerLib;

namespace WebCrawlerApp
{
    public partial class MainForm : Form
    {
        private Crawler crawler = new Crawler();

        public MainForm()
        {
            InitializeComponent();
            crawler.PageDownloaded += url => Invoke(new Action(() => listSuccess.Items.Add(url)));
            crawler.PageFailed += url => Invoke(new Action(() => listFail.Items.Add(url)));
        }

        private async void btnStart_Click(object sender, EventArgs e)
        {
            listSuccess.Items.Clear();
            listFail.Items.Clear();
            btnStart.Enabled = false;

            string startUrl = txtUrl.Text.Trim();
            if (!startUrl.StartsWith("http"))
                startUrl = "http://" + startUrl;

            await crawler.Start(startUrl);
            btnStart.Enabled = true;
        }
    }
}