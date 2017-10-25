using System.Threading.Tasks;
using Data.Commands;
using Data.Configuration;
using Data.Events;
using Data.Mappers;
using Data.Mappers.Messages;
using Domain.GateWays;
using Domain.Models;
using log4net;
using Messages;
using Messages.Connection;
using Messages.Payload;
using Messages.ServerInfo;

namespace Data
{
    public class Transport : ITransportGateWay
    {
        private readonly ILog _logger;

        public Transport(IConfiguration configuration)
        {
            _logger = LogManager.GetLogger(GetType());
        }

        public void Start()
        {
            //TODO implement this.
        }

        public Task StartAsync()
        {
            //TODO implement this.
           return new Task(() => { });
        }

        public void Stop()
        {    
            //TODO implement this.
        }
    }
}