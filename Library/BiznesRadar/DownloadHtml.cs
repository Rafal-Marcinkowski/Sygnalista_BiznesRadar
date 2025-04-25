using System.Text;

namespace Library.BiznesRadar;

public class DownloadHtml
{
    public async static Task<string> DownloadHtmlAsync(string urlFragment, string quarterAtTheEnd = ",Q")
    {
        string url = $"{urlFragment}{quarterAtTheEnd}";

        HttpClientHandler handler = new()
        {
            ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true
        };

        using HttpClient client = new(handler);

        while (true)
        {
            using HttpResponseMessage response = await client.GetAsync(url);
            string requestUri = response.RequestMessage.RequestUri.ToString();

            if (requestUri != url)
            {
                url = $"{requestUri},Q";
                continue;
            }

            using HttpContent content = response.Content;
            var json = await content.ReadAsStringAsync();
            return json;
        }
    }

    private static readonly HttpClient client = new();

    public static async Task<string> GetChartDataAsync(string companyCode)
    {
        var url = "https://www.biznesradar.pl/get-quotes-json/";

        var postData = $"oid={companyCode}&range=1r&type=lin&without_operations=0&currency_exchange=0";

        var request = new HttpRequestMessage(HttpMethod.Post, url)
        {
            Content = new StringContent(postData, Encoding.UTF8, "application/x-www-form-urlencoded")
        };

        request.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/133.0.0.0 Safari/537.36");
        request.Headers.Add("Referer", $"https://www.biznesradar.pl/notowania/ALL");
        request.Headers.Add("X-Requested-With", "XMLHttpRequest");

        var response = await client.SendAsync(request);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadAsStringAsync();
    }
}
