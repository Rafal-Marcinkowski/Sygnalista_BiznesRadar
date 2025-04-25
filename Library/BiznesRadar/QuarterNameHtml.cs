using HtmlAgilityPack;

namespace Library.BiznesRadar;

public class QuarterNameHtml
{
    public static List<string> Get(string html)
    {
        HtmlDocument doc = new();
        doc.LoadHtml(html);
        HtmlNodeCollection thNodes = doc.DocumentNode.SelectNodes("//th[@class='thq h']");
        HtmlNode thNewest = doc.DocumentNode.SelectSingleNode("//th[@class='thq h newest']");
        _ = SaveTextToFile.SaveAsync("thnodes", thNewest.InnerText.Trim());
        List<string> quarterNameList = [];

        if (thNodes != null && thNewest is not null)
        {
            foreach (HtmlNode thNode in thNodes)
            {
                quarterNameList.Add(new string([.. thNode.InnerText.Trim().TakeWhile(q => !char.IsWhiteSpace(q))]));
            }
            quarterNameList.Add(new string([.. thNewest.InnerText.Trim().TakeWhile(q => !char.IsWhiteSpace(q))]));
        }

        quarterNameList.Reverse();

        foreach (var item in quarterNameList)
        {
            _ = SaveTextToFile.AddAsync("quarternamelist", item + "\n");
        }

        return quarterNameList;
    }
}
