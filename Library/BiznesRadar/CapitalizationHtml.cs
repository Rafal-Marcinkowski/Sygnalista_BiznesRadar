using HtmlAgilityPack;

namespace Library.BiznesRadar;

public class CapitalizationHtml
{
    public async static Task<string> Get(string html)
    {
        HtmlDocument doc = new();
        doc.LoadHtml(html);
        HtmlNode thNode = doc.DocumentNode.SelectSingleNode("//th[text()='Kapitalizacja:']");

        if (thNode != null)
        {
            HtmlNode tdNode = thNode.SelectSingleNode("following-sibling::td");
            if (tdNode != null)
            {
                string innerText = tdNode.InnerText.Trim().Replace(" ", "").Replace(".", "");
                if (long.TryParse(innerText, out long cap))
                {
                    cap /= 1_000_000;
                    _ = SaveTextToFile.SaveAsync("capitalization", innerText);
                    return $"{cap} mln";
                }
            }
        }

        return string.Empty;
    }
}
