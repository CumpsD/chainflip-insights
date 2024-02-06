namespace ChainflipInsights.Feeders
{
    using System.Threading.Tasks;

    public interface IFeeder
    {
        Task Start();
    }
}