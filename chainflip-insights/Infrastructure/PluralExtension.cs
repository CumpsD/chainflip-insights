namespace ChainflipInsights.Infrastructure
{
    public static class PluralExtension
    {
        public static string Plural(this int value, string single, string other) 
            => value == 1 ? single : other;
        
        public static string Plural(this long value, string single, string other) 
            => value == 1 ? single : other;
    }
}