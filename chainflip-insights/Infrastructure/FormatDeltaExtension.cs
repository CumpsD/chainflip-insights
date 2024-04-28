namespace ChainflipInsights.Infrastructure
{
    public static class FormatDeltaExtension
    {
        public static string FormatDelta(this string delta) 
            => delta.StartsWith('-') ? $"-${delta[1..]}" : $"${delta}";
    }
}