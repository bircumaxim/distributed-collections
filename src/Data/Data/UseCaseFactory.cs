using System;
using Data.Events;
using Data.Mappers;
using Data.Mappers.Messages;
using Domain.Models;
using Domain.UseCases;
using log4net;
using Messages.Payload;

namespace Data
{
    public class UseCaseFactory
    {
        private readonly ILog _logger;
        private readonly Transport _transport;

        public UseCaseFactory(Transport transport)
        {
            _logger = LogManager.GetLogger(GetType());
            _transport = transport;
        }

        public IUseCase GetUseCaseFor(RemoteApplicationMessageReceivedEventArgs args)
        {
            //TODO add here usecases instantiation
            return null;
        }
    }
}