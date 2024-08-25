namespace ChainflipInsights.Infrastructure
{
    public static class FormatSs58Extension
    {
        public static string FormatSs58(this string ss58)
            => $"{ss58[..8]}...{ss58.Substring(ss58.Length - 8, 8)}";
    }
}