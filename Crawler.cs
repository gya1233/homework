using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WebCrawlerLib
{
    public class Crawler
    {
        public event Action<string> PageDownloaded;
        public event Action<string> PageFailed;

        private HttpClient httpClient = new HttpClient();
        private HashSet<string> visitedUrls = new HashSet<string>();
        private Queue<string> urls = new Queue<string>();
        private string hostFilter = "";
        private static readonly string[] validExtensions = { ".html", ".htm", ".aspx", ".jsp", ".php" };

        public async Task Start(string startUrl)
        {
            visitedUrls.Clear();
            urls.Clear();

            try
            {
                Uri baseUri = new Uri(startUrl);
                hostFilter = baseUri.Host;

                urls.Enqueue(startUrl);
                visitedUrls.Add(startUrl);

                while (urls.Count > 0)
                {
                    string current = urls.Dequeue();
                    string html = await DownloadAsync(current);
                    if (html == null)
                    {
                        PageFailed?.Invoke(current);
                        continue;
                    }
                    PageDownloaded?.Invoke(current);

                    if (!IsHtmlPage(current))
                        continue;

                    foreach (string link in ParseUrls(html, current))
                    {
                        if (!visitedUrls.Contains(link) && new Uri(link).Host == hostFilter)
                        {
                            urls.Enqueue(link);
                            visitedUrls.Add(link);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                PageFailed?.Invoke($"错误：{ex.Message}");
            }
        }

        private async Task<string> DownloadAsync(string url)
        {
            try
            {
                return await httpClient.GetStringAsync(url);
            }
            catch
            {
                return null;
            }
        }

        private bool IsHtmlPage(string url)
        {
            string path = new Uri(url).AbsolutePath.ToLower();
            foreach (var ext in validExtensions)
            {
                if (path.EndsWith(ext))
                    return true;
            }
            return false;
        }

        private IEnumerable<string> ParseUrls(string html, string baseUrl)
        {
            var matches = Regex.Matches(html, @"href\s*=\s*["'](?<url>[^"'#>]+)["']", RegexOptions.IgnoreCase);
            Uri baseUri = new Uri(baseUrl);

            foreach (Match match in matches)
            {
                string link = match.Groups["url"].Value;
                if (string.IsNullOrWhiteSpace(link)) continue;

                string fixedUrl = FixUrl(link, baseUri);
                if (Uri.IsWellFormedUriString(fixedUrl, UriKind.Absolute))
                    yield return fixedUrl;
            }
        }

        private string FixUrl(string url, Uri baseUri)
        {
            if (Uri.TryCreate(url, UriKind.Absolute, out Uri result))
                return result.ToString();
            return new Uri(baseUri, url).ToString();
        }
    }
}