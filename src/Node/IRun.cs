using System.Threading.Tasks;

namespace Node
{
    public interface IRun
    {
        void Stop();
        Task StartAsync();
    }
}