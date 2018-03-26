using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using YoutubeLyric.Core;

namespace YoutubeLyric
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            YoutubeLyric form = new YoutubeLyric();
            Lyric lyricform = new Lyric();
            lyricform.Show();
            Application.Run(form);
        }
    }
}
