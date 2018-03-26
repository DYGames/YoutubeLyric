using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using YoutubeLyric.Core;

namespace YoutubeLyric
{
    public partial class Lyric : Form
    {
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();


        public Lyric()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
            TopMost = true;

            this.MouseDown += Lyric_Click;
            this.textBox1.MouseDown += Lyric_Click;


            this.StartPosition = FormStartPosition.Manual;

            this.Location = new System.Drawing.Point(1100, 900);
        }


        private void Lyric_Click(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        public void SetLyric(List<GetLyric.LyricFormat> data)
        {
            if (data == null)
            {
                textBox1.Text = "가사가 검색되지 않았습니다. 다시 검색해 주세요.";
            }
            else
            {
                string text = "";
                foreach (var item in data)
                {
                    text += item.Text;
                    text += Environment.NewLine;
                }

                textBox1.Text = text;
            }
        }

        public void SetAlwaysOntop(bool isOnTop)
        {
            TopMost = isOnTop;
        }

        public void SetClickToMove(bool isClickToMove)
        {
            if (isClickToMove)
            {
                this.MouseDown += Lyric_Click;
                this.textBox1.MouseDown += Lyric_Click;
            }
            else
            {
                this.MouseDown -= Lyric_Click;
                this.textBox1.MouseDown -= Lyric_Click;
            }
        }
    }
}
