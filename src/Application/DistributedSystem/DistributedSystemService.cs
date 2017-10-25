using System.IO;
using System.Threading.Tasks;
using MessageBroker.Infrastructure;

namespace DistributedSystem
{
    public class DistributedSystemService : IRun
    {
        
        private static readonly string ConfigFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Application/DistrubutedSystem/config.xml");

        public DistributedSystemService()
        {
           
        }
        
        
        public void Start()
        {
            throw new System.NotImplementedException();
        }

        public Task StartAsync()
        {
            throw new System.NotImplementedException();
        }

        public void Stop()
        {
            throw new System.NotImplementedException();
        }
    }
}