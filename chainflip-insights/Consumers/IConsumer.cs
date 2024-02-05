namespace ChainflipInsights.Consumers
{
    using System.Threading;
    using System.Threading.Tasks.Dataflow;

    public interface IConsumer
    {
        ITargetBlock<BroadcastInfo> Build(
            CancellationToken cancellationToken);
    }
}