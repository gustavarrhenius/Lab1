using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lab1.Helpers
{
    /// <summary>
    /// En hjälp-klass som hanterar utskrift till konsolen
    /// </summary>
    public class OutputHelper
    {
        /// <summary>
        /// Property som innehåller en textsträng som berättar vilka kommandon som finns tillgängliga
        /// då användaren startat programmet.
        /// </summary>
        public static string RootCommandList {
            get
            {
                string returnString = "\n\nList of Commands:";
                returnString += "\n\t?/help:\tPrints this list of commands.";
                returnString += "\n\texit:\tExits the program.";

                return returnString;
            }
        }

        /// <summary>
        /// Metod som returnerar det meddelande som skall visas då programmet avslutas
        /// </summary>
        /// <param name="message">[Optional] En sträng som läggs till i slutet på befintligt meddelande</param>
        /// <returns></returns>
        public static string ExitMessage(string message = "") {
            return string.Format("\n\nProgram closing. {0}", message);
        }

        /// <summary>
        /// Property som innehåller felmeddelande som skall ges om användaren matat in ett kommando som inte kan hanteras
        /// av programmet.
        /// </summary>
        public static string ErrorInvalidInput
        {
            get
            {
                return string.Format("\n\nInvalid Input! (type (? or help) and [enter] for help.)");
            }
        }

        /// <summary>
        /// Property som innehåller felmeddelande som skall ges om programmet inte vet vilket
        /// tillstånd det befinner sig i.
        /// </summary>
        public static string ErrorLostState
        {
            get
            {
                return string.Format("\n\nError! I've lost my state! Returning to default state.");
            }
        }

        /// <summary>
        /// Property som innehåller det meddelande som skall ges för att be användaren mata in ett kommando
        /// </summary>
        public static string EnterCommand
        {
            get
            {
                return string.Format("\n\nPlease Enter command + [enter] (help: ?):");
            }
        }

        /// <summary>
        /// Property som innehåller ett välkomstmeddelande
        /// </summary>
        public static string GreetingMessage
        {
            get
            {
                return string.Format("\n\nWelcome! {0}", EnterCommand);
            }
        }

        public static void Put(String PutString)
        {
            Console.WriteLine(PutString);
        }
    }
}
