namespace ChainflipInsights.Infrastructure.Pipelines
{
    using System;
    using System.Threading.Tasks;
    using System.Threading.Tasks.Dataflow;

    public class EncapsulatingTarget<TStart, TEnd> : ITargetBlock<TStart>
    {
        private readonly ITargetBlock<TStart> _startBlock;

        private readonly ActionBlock<TEnd> _endBlock;

        public EncapsulatingTarget(
            ITargetBlock<TStart> startBlock,
            ActionBlock<TEnd> endBlock)
        {
            _startBlock = startBlock ?? throw new ArgumentNullException(nameof(startBlock));
            _endBlock = endBlock ?? throw new ArgumentNullException(nameof(endBlock));
        }

        public Task Completion
            => _endBlock.Completion;

        public void Complete()
            => _startBlock.Complete();

        void IDataflowBlock.Fault(Exception exception)
        {
            ArgumentNullException.ThrowIfNull(exception);

            _startBlock.Fault(exception);
        }

        public DataflowMessageStatus OfferMessage(
            DataflowMessageHeader messageHeader,
            TStart messageValue,
            ISourceBlock<TStart>? source,
            bool consumeToAccept)
            => _startBlock.OfferMessage(messageHeader, messageValue, source, consumeToAccept);
    }
}