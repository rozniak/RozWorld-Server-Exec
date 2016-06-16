using Oddmatics.RozWorld.API.Server;
using Oddmatics.RozWorld.API.Generic;
using Oddmatics.RozWorld.Server;
using System;

namespace Oddmatics.RozWorld.ServerExecutive
{
    class Program
    {
        public const int CharLimit = 100;
        public static string CliInput { get; private set; }

        static void Main(string[] args)
        {
            var server = new RwServer();
            bool shouldClose = false;

            CliInput = String.Empty;
            server.Logger = new ConsoleLogger();
            server.Stopping += delegate(object sender, EventArgs e)
                                { shouldClose = true; };
            RwCore.SetServer(server);
            server.Start();

            while (!shouldClose)
            {
                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo keyPressed = Console.ReadKey();

                    switch (keyPressed.KeyChar)
                    {
                            // Enter key
                        case '\r':
                            // Reason for this first bit is so that the Logger moves on the next line
                            // and doesn't bother rewriting the command (want it to remain in the Logger)
                            string cmd = CliInput;
                            CliInput = String.Empty;
                            Console.WriteLine();

                            if (cmd.StartsWith("/") && cmd.Length > 1)
                                server.SendCommand(server.ServerAccount, cmd.Substring(1));
                            else
                                server.Logger.Out("[ERR] Unknown command.");

                            break;

                            // Backspace
                        case '\b':
                            Console.Write(" ");
                            Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);

                            if (CliInput.Length > 0)
                                CliInput = CliInput.Substring(0, CliInput.Length - 1);
                            break;

                            // Any other key and backspace
                        default:
                            if (CliInput.Length < Program.CharLimit)
                                CliInput += keyPressed.KeyChar;
                            break;
                    }
                }
            }
        }
    }

    class ConsoleLogger : ILogger
    {
        public ConsoleLogger()
        {
            Console.SetBufferSize(160, 80);
        }


        public void Out(string message)
        {
            if (!string.IsNullOrEmpty(Program.CliInput))
            {
                Console.SetCursorPosition(0, Console.CursorTop);
                for (int i = 0; i < Program.CharLimit; i++)
                    Console.Write(" ");
                Console.SetCursorPosition(0, Console.CursorTop);
            }
                
            Console.WriteLine("[" + DateTime.Now.ToString() + "] " + message);
            Console.Write(Program.CliInput);
        }

        public bool Save()
        {
            // don't use yet
            return true;
        }
    }
}
