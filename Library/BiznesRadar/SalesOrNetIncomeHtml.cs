using HtmlAgilityPack;

namespace Library.BiznesRadar;

public class SalesOrNetIncomeHtml
{
    public static List<string> Get(string html, string node)
    {
        List<string> list = [];
        HtmlDocument doc = new();
        doc.LoadHtml(html);

        HtmlNode trNode = doc.DocumentNode.SelectSingleNode($"//tr[@data-field='{node}']");

        if (trNode != null)
        {
            HtmlNodeCollection tdNodes = trNode.SelectNodes(".//td[@class='h']");

            if (tdNodes != null)
            {
                foreach (HtmlNode tdNode in tdNodes)
                {
                    HtmlNode spanNode = tdNode.SelectSingleNode(".//span[@class='value']//span[@class='pv']//span");
                    if (spanNode != null)
                    {
                        list.Add(spanNode.InnerText.Trim());
                    }
                }
                list = [.. list.TakeLast(3)];
            }

            HtmlNode tNewest = trNode.SelectSingleNode(".//td[@class='h newest']");

            if (tNewest != null)
            {
                HtmlNode spanNode = tNewest.SelectSingleNode(".//span[@class='value']//span[@class='pv']//span");
                if (spanNode != null)
                {
                    list.Add(spanNode.InnerText.Trim().Trim());
                }
            }
        }

        list.Reverse();
        return list.ConvertAll(q => q.ConvertToMillions());
    }
}
