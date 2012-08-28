using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lab1.Helpers;
using Lab1.Model;

namespace Lab1
{
    class Program
    {
        static void Main(string[] args)
        {
            // när exit sätts till true skall programmet avslutas.
            bool exit = false;
            // input används för att hålla input från användaren.
            string input;
            // ett InputParser-objekt som har till uppgift att tolka och utföra kommandon från användaren.
            InputParser inputParser = new InputParser();
            // parseResult används för att hålla resultatet av en tolkning från inputParser
            string parseResult;

            Console.WriteLine(OutputHelper.GreetingMessage);
            while (!exit)
            {
                // Hämta input från användaren
                input = InputHelper.GetUserInput();
                
                // Tolka Användarens input och tilldela resultatet av tolkningen till parseResult.
                parseResult = inputParser.ParseInput(input);

                // Skriv ut resultatet från tolkningen
                Console.WriteLine(parseResult);

                // Avsluta programmet om inputParser är i tillståndet "Exit"
                if (inputParser.IsStateExit)
                    exit = true;
            }
        }
    }
}
