using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using YoutubeLyric.Core;

namespace YoutubeLyric
{
    public partial class YoutubeLyric : Form
    {
        char[] token = { '-', ':' };

        public YoutubeLyric()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Process[] p = Process.GetProcessesByName("chrome");
            string[] tt = null;
            foreach (var item in p)
            {
                for (int i = 0; i < token.Length; i++)
                {
                    if (item.MainWindowTitle.Contains(token[i]))
                        tt = item.MainWindowTitle.Split(token[i]);
                }
            }
            if (tt == null)
            {
                textBox1.Text = "직접 입력"; textBox2.Text = "직접 입력";
            }
            else
            {
                textBox1.Text = tt[0].Replace(" - YouTube - Chrome", ""); textBox2.Text = tt[1].Replace(" - YouTube - Chrome", "");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            (Application.OpenForms["Lyric"] as Lyric).Opacity = int.Parse(textBox3.Text) / 100.0f;
            //string temp = textBox1.Text;
            //textBox1.Text = textBox2.Text;
            //textBox2.Text = temp;

        }

        private void button2_Click(object sender, EventArgs e)
        {

            Dictionary<string, string> data = new Dictionary<string, string>
                    {
                        { "[TITLE]", textBox1.Text },
                        { "[ARTIST]", textBox2.Text }
                    };
            List<GetLyric.LyricFormat> getLyrics = null;
            Task.Run(async () =>
            {
                getLyrics = await GetLyric.GetLyricsAsync(data);
            }).ContinueWith((Task t, object o) =>
            {
                (Application.OpenForms["Lyric"] as Lyric).SetLyric(getLyrics);
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            (Application.OpenForms["Lyric"] as Lyric).SetAlwaysOntop(((CheckBox)sender).Checked);
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            (Application.OpenForms["Lyric"] as Lyric).SetClickToMove(((CheckBox)sender).Checked);
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            int n;
            if (!int.TryParse(textBox3.Text, out n))
                textBox3.Clear();
        }
    }
}
