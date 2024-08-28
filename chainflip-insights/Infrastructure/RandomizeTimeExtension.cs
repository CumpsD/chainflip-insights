namespace ChainflipInsights.Infrastructure
{
    using System;

    public static class RandomizeTimeExtension
    {
        public static int RandomizeTime(this int originalTimeInMs)
        {
            var factor = originalTimeInMs / 30000;
            var random = factor * Random.Shared.Next(-5000, 5000);
            return originalTimeInMs + (random < 0 ? 0 : random);
        }
    }
}