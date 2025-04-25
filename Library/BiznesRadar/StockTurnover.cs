using HtmlAgilityPack;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Library.BiznesRadar;

public class StockTurnover
{
    public async static Task<string> GetTurnoverMedian(string html)
    {
        HtmlDocument doc = new();
        doc.LoadHtml(html);

        string xpathExpression = "//table[@class='qTableFull']/tr";

        HtmlNodeCollection rows = doc.DocumentNode.SelectNodes(xpathExpression);

        List<decimal> turnoverValues = [];

        if (rows != null)
        {
            foreach (HtmlNode row in rows)
            {
                HtmlNodeCollection cells = row.SelectNodes("td");

                if (cells?.Count >= 7)
                {
                    string turnoverStr = cells[6].InnerText.Trim();

                    if (decimal.TryParse(Regex.Replace(turnoverStr, @"\s+", ""), out decimal turnover))
                    {
                        turnoverValues.Add(turnover);
                    }

                    if (turnoverValues.Count >= 20)
                    {
                        break;
                    }
                }
            }
        }
        else
        {
            return "0";
        }

        if (turnoverValues.Count == 0)
        {
            return "0";
        }

        if (turnoverValues.Count < 20)
        {
            return "0";
        }

        decimal medianTurnover = turnoverValues.Count > 0 ? turnoverValues.Order().Skip((turnoverValues.Count - 1) / 2).First() : 0;
        int realMedianTurnover = (int)medianTurnover;

        return realMedianTurnover.ToString();
    }

    public async static Task<DateTime?> GetFirstDate(string html)
    {
        HtmlDocument doc = new();
        doc.LoadHtml(html);

        string xpathExpression = "//table[@class='qTableFull']/tr";

        HtmlNodeCollection rows = doc.DocumentNode.SelectNodes(xpathExpression);

        DateTime? firstDate = null;

        if (rows != null)
        {
            foreach (HtmlNode row in rows)
            {
                HtmlNodeCollection cells = row.SelectNodes("td");

                if (cells?.Count >= 7)
                {
                    string dateString = cells[0].InnerText.Trim();

                    if (DateTime.TryParseExact(dateString, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedDate))
                    {
                        firstDate = parsedDate;
                        break;
                    }
                }
            }
        }

        return firstDate;
    }
}
