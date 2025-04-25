using HtmlAgilityPack;
using Library;
using Library.BiznesRadar;

namespace PogromcaBiznesRadar.Services;

public class HtmlProcessingManager : BindableBase
{
    public HtmlProcessingManager()
    {
        companyName = string.Empty;
        capitalization = string.Empty;
        turnoverMedian = string.Empty;
        biznesRadarHtml = string.Empty;

        quarters = [];
        salesIncomes = [];
        netIncomes = [];
    }

    private string companyName;
    public string CompanyName
    {
        get => companyName;
        set => SetProperty(ref companyName, value);
    }

    private string capitalization;
    public string Capitalization
    {
        get => capitalization;
        set => SetProperty(ref capitalization, value);
    }

    private string turnoverMedian;
    public string TurnoverMedian
    {
        get => turnoverMedian;
        set => SetProperty(ref turnoverMedian, value);
    }

    private List<string> quarters;
    public List<string> Quarters
    {
        get => quarters;
        set => SetProperty(ref quarters, value);
    }

    private List<string> salesIncomes;
    public List<string> SalesIncomes
    {
        get => salesIncomes;
        set => SetProperty(ref salesIncomes, value);
    }

    private List<string> netIncomes;
    public List<string> NetIncomes
    {
        get => netIncomes;
        set => SetProperty(ref netIncomes, value);
    }

    public static string SymbolOId { get; set; } = string.Empty;

    private string biznesRadarHtml;

    public async Task Clear()
    {
        Quarters = [];
        SalesIncomes = [];
        NetIncomes = [];
        CompanyName = "";
        Capitalization = "";
    }

    public async Task DownloadHtmlAsync(string companyCode)
    {
        biznesRadarHtml = await DownloadHtml.DownloadHtmlAsync($"https://www.biznesradar.pl/raporty-finansowe-rachunek-zyskow-i-strat/{companyCode}");
        SymbolOId = await ExtractOidFromHtml(biznesRadarHtml);
        _ = SaveTextToFile.SaveAsync("biznesradar", biznesRadarHtml);
    }

    public async Task ProcessHtmlAsync(string companyCode)
    {
        if (!string.IsNullOrEmpty(biznesRadarHtml))
        {
            CompanyName = await CompanyNameHtml.Get(biznesRadarHtml);

            if (!CompanyName.Contains("bank"))
            {
                Capitalization = await CapitalizationHtml.Get(biznesRadarHtml);

                if (CompanyName.Contains(companyCode))
                {
                    CompanyName = companyCode;
                }

                Quarters = QuarterNameHtml.Get(biznesRadarHtml);
                SalesIncomes = SalesOrNetIncomeHtml.Get(biznesRadarHtml, "IncomeRevenues");
                NetIncomes = SalesOrNetIncomeHtml.Get(biznesRadarHtml, "IncomeNetProfit");
            }
        }

        var html = await DownloadHtml.DownloadHtmlAsync($"https://www.biznesradar.pl/notowania-historyczne/{companyCode}", "");
        TurnoverMedian = await StockTurnover.GetTurnoverMedian(html);
    }

    public static Task<string> ExtractOidFromHtml(string html)
    {
        var doc = new HtmlDocument();
        doc.LoadHtml(html);

        var anchor = doc.DocumentNode.SelectSingleNode("//a[contains(@onclick, 'symbol_oid')]");

        if (anchor != null)
        {
            string onclickAttribute = anchor.GetAttributeValue("onclick", "");

            var match = System.Text.RegularExpressions.Regex.Match(onclickAttribute, @"symbol_oid:\s*(\d+)");

            if (match.Success)
            {
                if (match.Success)
                {
                    return Task.FromResult(match.Groups[1].Value);
                }
            }

        }
        return Task.FromResult("");
    }
}
