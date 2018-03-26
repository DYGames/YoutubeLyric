using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using YoutubeLyric.Properties;

namespace YoutubeLyric.Core
{
    public class GetLyric
    {
        public class LyricFormat
        {
            public double Time { get; private set; }
            public string Text { get; private set; }

            public LyricFormat(string data)
            {
                var lyric = data.Split(new[] { ']' }, 2);
                var time = lyric[0].Substring(1).Split(':');
                Time = Convert.ToDouble(time[0]) * 60 + Convert.ToDouble(time[1]);
                Text = lyric[1].Trim();
            }

            public LyricFormat()
            {
                Text = "";
            }

            public LyricFormat(double time, string text)
            {
                Time = time;
                Text = text;
            }
        }

        public static async Task<List<LyricFormat>> GetLyricsAsync(IDictionary<string, string> data)
        {
            try
            {
                string act = "GetResembleLyric2";
                Debug.WriteLine(Resources.ResourceManager.GetString(act));
                var content = data.Aggregate(Resources.ResourceManager.GetString(act), (o, i) => o.Replace(i.Key, i.Value));
                Debug.WriteLine(content);
                var wr = (HttpWebRequest)WebRequest.Create(@"http://lyrics.alsong.co.kr/alsongwebservice/service1.asmx");
                wr.ServicePoint.Expect100Continue = false;
                wr.Method = "POST";
                wr.UserAgent = "gSOAP";
                wr.ContentType = "application/soap+xml; charset=UTF-8";
                wr.Headers.Add("SOAPAction", "ALSongWebServer/" + act);
                using (var rq = new StreamWriter(wr.GetRequestStream()))
                {
                    rq.Write(content);
                }
                using (var rp = new StreamReader((await wr.GetResponseAsync()).GetResponseStream()))
                {
                    return WebUtility.HtmlDecode(rp.ReadToEnd().Split(new[] { "<strLyric>", "</strLyric>" }, StringSplitOptions.None)[1])
                        .Split(new[] { "<br>" }, StringSplitOptions.RemoveEmptyEntries)
                        .Select(i => new LyricFormat(i))
                        .Where(i => i.Text.Length != 0)
                        .ToList();
                }
            }
            catch
            {
                return null;
            }
        }
    }
}
