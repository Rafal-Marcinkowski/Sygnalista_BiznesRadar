namespace Library;

internal static class StringExtensions
{
    public static string ConvertToMillions(this string numberString)
    {
        string cleanedNumberString = numberString.Replace(" ", "");

        if (!decimal.TryParse(cleanedNumberString, out decimal number))
        {
            return "Invalid input";
        }

        decimal result = number / 1000;
        return $"{result:F2} mln";
    }
}
