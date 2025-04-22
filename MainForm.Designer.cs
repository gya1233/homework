namespace WebCrawlerApp
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;
        private TextBox txtUrl;
        private Button btnStart;
        private ListBox listSuccess;
        private ListBox listFail;
        private Label label1;
        private Label label2;

        private void InitializeComponent()
        {
            txtUrl = new TextBox { Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right, Width = 400, Top = 10, Left = 10 };
            btnStart = new Button { Text = "开始爬取", Top = 10, Left = 420 };
            btnStart.Click += btnStart_Click;

            label1 = new Label { Text = "成功网址", Top = 50, Left = 10 };
            listSuccess = new ListBox { Top = 70, Left = 10, Width = 300, Height = 300, Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left };

            label2 = new Label { Text = "失败网址", Top = 50, Left = 320 };
            listFail = new ListBox { Top = 70, Left = 320, Width = 300, Height = 300, Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left };

            Controls.Add(txtUrl);
            Controls.Add(btnStart);
            Controls.Add(label1);
            Controls.Add(listSuccess);
            Controls.Add(label2);
            Controls.Add(listFail);

            Text = "简易爬虫";
            Width = 650;
            Height = 450;
        }
    }
}