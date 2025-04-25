using Library;
using Library.BiznesRadar;
using PogromcaBiznesRadar.MVVM.Views;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace PogromcaBiznesRadar.Services;

public class ChartManager
{
    public async static Task DrawChart(string jsonData)
    {
        try
        {
            var stockData = ChartDataParser.ParseStockData(jsonData);

            if (Application.Current.MainWindow is not MainWindowView mainWindow)
            {
                return;
            }

            var canvas = mainWindow.StockChartCanvas;
            if (canvas == null)
            {
                return;
            }

            canvas.Children.Clear();

            double canvasWidth = canvas.ActualWidth;
            double canvasHeight = canvas.ActualHeight;

            if (canvasWidth <= 0 || canvasHeight <= 0)
            {
                return;
            }

            double minPrice = stockData.Min(d => d.Min);
            double maxPrice = stockData.Max(d => d.Max);

            double xStep = canvasWidth / (stockData.Count - 1);
            double yScale = canvasHeight / (maxPrice - minPrice);

            for (int i = 0; i < stockData.Count - 1; i++)
            {
                var startPoint = new Point(i * xStep, canvasHeight - (stockData[i].Close - minPrice) * yScale);
                var endPoint = new Point((i + 1) * xStep, canvasHeight - (stockData[i + 1].Close - minPrice) * yScale);

                var line = new Line
                {
                    X1 = startPoint.X,
                    Y1 = startPoint.Y,
                    X2 = endPoint.X,
                    Y2 = endPoint.Y,
                    Stroke = Brushes.LightGray,
                    StrokeThickness = 2
                };

                canvas.Children.Add(line);
            }

            void DrawScaleLine(double price, Brush strokeBrush, string labelText, bool isCurrentValue = false)
            {
                double yPosition = canvasHeight - (price - minPrice) * yScale;

                var scaleLine = new Line
                {
                    X1 = 0,
                    Y1 = yPosition,
                    X2 = canvasWidth,
                    Y2 = yPosition,
                    Stroke = strokeBrush,
                    StrokeThickness = 1,
                    StrokeDashArray = [4, 2]
                };

                canvas.Children.Add(scaleLine);

                var label = new TextBlock
                {
                    Text = labelText,
                    Foreground = Brushes.Black,
                    FontSize = 16,
                    FontWeight = FontWeights.DemiBold,
                    Margin = new Thickness(canvasWidth - 40, yPosition - 10, 0, 0)
                };

                if (isCurrentValue)
                {
                    label.Margin = new Thickness(canvasWidth, yPosition - 10, 0, 0);
                }

                canvas.Children.Add(label);
            }

            DrawScaleLine(maxPrice, Brushes.Gray, maxPrice.ToString("N2"));
            DrawScaleLine(minPrice, Brushes.Gray, minPrice.ToString("N2"));

            if (stockData.Count > 0)
            {
                double lastPrice = stockData[^2].Close;
                DrawScaleLine(lastPrice, Brushes.DarkOrange, lastPrice.ToString("N2"), isCurrentValue: true);
            }

        }
        catch (Exception ex)
        {
            _ = SaveTextToFile.SaveAsync("ErrorInDrawChart", ex.Message);
        }
    }
}
