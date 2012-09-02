using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lab1.Helpers;
using Lab1.Model;
using Lab1.Infrastructure;
using Lab1.Model.Repository;

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
            var Repo = new Model.Repository.FakeRepository();
            InputParser inputParser = new InputParser(Repo);
            // parseResult används för att hålla resultatet av en tolkning från inputParser
            string parseResult;
            OutputHelper.Put(OutputHelper.GreetingMessage);
            while (!exit)
            {
                // Hämta input från användaren
                input = InputHelper.GetUserInput();
                // Tolka Användarens input och tilldela resultatet av tolkningen till parseResult.
                parseResult = inputParser.ParseInput(input);
                // Skriv ut resultatet från tolkningen
                OutputHelper.Put(parseResult);
                // Avsluta programmet om inputParser är i tillståndet "Exit"
                if (inputParser.IsStateExit)
                    exit = true;
            }
        }   
    }
}
