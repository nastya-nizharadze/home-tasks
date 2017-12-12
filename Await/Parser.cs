using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;

namespace Await
{
    public class Parser
    {

        public struct Page
        {
            public string URL;
            public string web_page;
        }


        public async Task<bool> GetPages(string s)
        {
            try
            {
                Page page = new Page();
                using (WebClient client = new WebClient())
                {
                    page.URL = s;
                    page.web_page = await GetWebPage(s);

                }

                var result = await GetList(page.web_page);

                foreach (var p in result)
                {
                    var result1 = await GetList(p.web_page);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return true;
        }

        public async Task<List<Page>> GetList(string s)
        {
            List<Page> listOfPages = new List<Page>();
            Regex r = new Regex(@"<a.*? href=""(?<url>https?[\w\.:?&-_=#/]*)""+?");
            Match m = r.Match(s);
            while (m.Success)
            {
                string s1 = m.Groups["url"].Value;
                var result = await GetWebPage(s1);
                Page page = new Page
                {
                    URL = s1,
                    web_page = result
                };
                listOfPages.Add(page);
                m = m.NextMatch();
            }
            return listOfPages;
        }

        public async Task<string> GetWebPage(string s)
        {
            using (WebClient client = new WebClient())
            {
                string reply = "";
                try
                {
                    reply = await client.DownloadStringTaskAsync(s);
                    Console.WriteLine("Page {0} - {1} page size", s, reply.Length);
                }
                catch (Exception e)
                {
                    Console.WriteLine("{0} - {1}", s, e.Message);
                }
                return (reply);
            }
        }
    }   
}
