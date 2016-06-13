using Oddmatics.RozWorld.API.Server;
using Oddmatics.RozWorld.API.Generic;
using Oddmatics.RozWorld.Server;
using System;

namespace Oddmatics.RozWorld.ServerExecutive
{
    class Program
    {
        static void Main(string[] args)
        {
            var server = new RwServer();
            server.Logger = new ConsoleLogger();
            RozWorld.API.Generic.RozWorld.SetServer(server);
            server.Start();
            Console.ReadKey(true);
        }
    }

    class ConsoleLogger : ILogger
    {

        public void Out(string message)
        {
            Console.WriteLine("[" + DateTime.Now.ToString() + "] " + message);
        }

        public bool Save()
        {
            // don't use yet
            return true;
        }
    }
}
