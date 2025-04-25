using HtmlAgilityPack;

namespace Library.BiznesRadar;

public class CompanyNameHtml
{
    public async static Task<string> Get(string html)
    {
        HtmlDocument doc = new();
        doc.LoadHtml(html);
        HtmlNode h1Node = doc.DocumentNode.SelectSingleNode("//div[@id='profile-header']//h1");

        if (h1Node != null)
        {
            string extractedText = h1Node.InnerText;
            string[] textParts = extractedText.Split("strat", StringSplitOptions.RemoveEmptyEntries);
            _ = SaveTextToFile.SaveAsync("companyname", textParts[1]);
            return textParts[1].Trim();
        }

        return string.Empty;
    }
}
