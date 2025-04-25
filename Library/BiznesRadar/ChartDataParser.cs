using Newtonsoft.Json;

namespace Library.BiznesRadar;

public static class ChartDataParser
{
    public static List<StockData> ParseStockData(string json)
    {
        var root = JsonConvert.DeserializeObject<Root>(json);
        return root?.Data
            .SelectMany(d => d.Quotes)
            .Select(q => new StockData
            {
                Timestamp = DateTimeOffset.FromUnixTimeSeconds(q.Ts).DateTime,
                Open = q.O,
                Close = q.C,
                Min = q.Min,
                Max = q.Max,
                Volume = q.V
            })
            .ToList() ?? [];
    }

    private class Root
    {
        public List<Data> Data { get; set; } = [];
    }

    private class Data
    {
        public List<Quote> Quotes { get; set; } = [];
    }

    private class Quote
    {
        public double Min { get; set; }
        public double Max { get; set; }
        public double O { get; set; }
        public double C { get; set; }
        public int V { get; set; }
        public long Ts { get; set; }
    }
}

public class StockData
{
    public DateTime Timestamp { get; set; }
    public double Open { get; set; }
    public double Close { get; set; }
    public double Min { get; set; }
    public double Max { get; set; }
    public int Volume { get; set; }
}