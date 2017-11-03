using System;
using System.IO;
using Mediator.Services;

namespace Mediator
{
    internal class Program
    {
        public static string ConfigPath = Path.Combine(Directory.GetCurrentDirectory(), "mediator-config.xml");
        
        public static void Main(string[] args)
        {
            var config = new Config(ConfigPath);
            var mediatorService = new MediatorService(config);
            Console.ReadLine();
        }
    }
}