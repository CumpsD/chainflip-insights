namespace ChainflipInsights.Infrastructure.Pipelines
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading;
    using System.Threading.Tasks.Dataflow;

    public class Pipeline<T>
    {
        public BufferBlock<T> Source { get; }

        public CancellationToken CancellationToken { get; }

        public Pipeline(
            BufferBlock<T> source,
            [NotNull] CancellationToken? ct)
        {
            Source = source ?? throw new ArgumentNullException(nameof(source));
            CancellationToken = ct ?? throw new ArgumentNullException(nameof(ct));
        }
    }
}