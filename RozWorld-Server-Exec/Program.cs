using Oddmatics.RozWorld.API.Generic;
using Oddmatics.RozWorld.API.Generic.Chat;
using Oddmatics.RozWorld.API.Server;
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
            server.FatalError +=
                delegate(object sender, EventArgs e)
                {
                    server.Logger.Out("Fatal error occurred - press any key to exit...");
                    Console.ReadKey(true);
                    shouldClose = true;
                };
            server.Stopped +=
                delegate(object sender, EventArgs e)
                {
                    server.Logger.Out("Server stopped - press any key to exit...");
                    Console.ReadKey(true);
                    shouldClose = true;
                };
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
                                server.SendCommand(server, cmd.Substring(1));
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


        public void Out(string message, bool colours = true)
        {
            if (!string.IsNullOrEmpty(Program.CliInput))
            {
                Console.SetCursorPosition(0, Console.CursorTop);
                for (int i = 0; i < Program.CharLimit; i++)
                    Console.Write(" ");
                Console.SetCursorPosition(0, Console.CursorTop);
            }
                
            Console.Write("[" + DateTime.Now.ToString() + "] ");

            bool formatCodeActive = false;

            foreach (char character in message)
            {
                if (formatCodeActive)
                {
                    string code = "&" + character;
                    switch (code.ToUpper())
                    {
                        case ChatColour.BLACK: Console.ForegroundColor = ConsoleColor.Black; break;
                        case ChatColour.DARK_BLUE: Console.ForegroundColor = ConsoleColor.DarkBlue; break;
                        case ChatColour.GREEN: Console.ForegroundColor = ConsoleColor.DarkGreen; break;
                        case ChatColour.TEAL: Console.ForegroundColor = ConsoleColor.DarkCyan; break;
                        case ChatColour.DARK_RED: Console.ForegroundColor = ConsoleColor.DarkRed; break;
                        case ChatColour.PURPLE: Console.ForegroundColor = ConsoleColor.DarkMagenta; break;
                        case ChatColour.ORANGE: Console.ForegroundColor = ConsoleColor.DarkYellow; break;
                        case ChatColour.GREY: Console.ForegroundColor = ConsoleColor.Gray; break;
                        case ChatColour.DARK_GREY: Console.ForegroundColor = ConsoleColor.DarkGray; break;
                        case ChatColour.BLUE: Console.ForegroundColor = ConsoleColor.Blue; break;
                        case ChatColour.LIME: Console.ForegroundColor = ConsoleColor.Green; break;
                        case ChatColour.CYAN: Console.ForegroundColor = ConsoleColor.Cyan; break;
                        case ChatColour.RED: Console.ForegroundColor = ConsoleColor.Red; break;
                        case ChatColour.MAGENTA: Console.ForegroundColor = ConsoleColor.Magenta; break;
                        case ChatColour.YELLOW: Console.ForegroundColor = ConsoleColor.Yellow; break;
                        case ChatColour.WHITE: Console.ForegroundColor = ConsoleColor.White; break;
                        default: Console.Write(character); break;
                    }
                    formatCodeActive = false;
                    continue;
                }
                else if (character == '&' && colours) // use colour param here to trigger colours
                {
                    formatCodeActive = true;
                    continue;
                }

                Console.Write(character);
            }

            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write(Program.CliInput);
        }

        public bool Save()
        {
            // don't use yet
            return true;
        }
    }
}
