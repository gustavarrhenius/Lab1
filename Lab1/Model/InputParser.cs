using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lab1.Helpers;
using Lab1.Model;

namespace Lab1.Model
{
    /// <summary>
    /// Input Parser ansvarar för att tolka och utföra de kommandon användaren matar in
    /// </summary>
    public class InputParser
    {
        /// <summary>
        /// ParserState används för att hålla reda på vilket tillstånd InputParser-objektet
        /// befinner sig i. 
        /// 
        /// Anledningen till att vi har olika tillstånd är för att veta vilka kommandon som skall finnas 
        /// tillgängliga för användaren.
        /// 
        /// Ett tillstånd skulle kunna vara att användaren har listat Users och därmed har tillgång
        /// till kommandon för att lista detaljer för en User. Ett annat tillstånd skulle kunna vara 
        /// att användaren håller på och lägger in en ny User och därmed har tillgång till kommandon
        /// för att sätta namn, etc, för användaren.
        /// 
        /// Som koden ser ut nu så finns endast två tillgängliga tillstånd, 1, som är Default State.
        /// Och -1 som är det tillståndet som InputParser går in i när programmet skall avslutas
        /// Ifall nya tillstånd implementeras skulle de kunna vara 2, 3, 4, etc.
        /// </summary>
        /*
         * 
         * Fråga 1
         *  private int ParserState { get; set; }
            public InputParser()
            {
                SetDefaultParserState();
            }*/
        
        private Repository.Repository repo;
        private Logger logger = new Logger();
        List<User> users;
        
        public InputParser(Repository.Repository repo)
        {
            this.repo = repo;
            this.users = repo.GetUsers();
        }

        State ParseState = State.Default;
        private enum State
        {
            Default,
            Exit
        }
        /// <summary>
        /// Returnerar true om ParserState är Exit (eller rättare sagt -1)
        /// </summary>
        public bool IsStateExit
        {
            get
            {
                return ParseState == State.Exit;
            }
        }

        /// <summary>
        /// Tolka input baserat på vilket tillstånd (ParserState) InputParser-objektet befinner sig i.
        /// </summary>
        /// <param name="input">Input sträng som kommer från användaren.</param>
        /// <returns></returns>
        public string ParseInput(string input)
        {
            logger.StringLog(input);
            if (ParseState == State.Default)
            {
                return ParseDefaultStateInput(input);
            }
            else if (ParseState == State.Exit)
            {
                // Do nothing - program should exit
                return "";
            }
            else
            {
                ParseState = State.Default;
                return OutputHelper.ErrorLostState;
            }
        }

        /// <summary>
        /// Tolka och utför de kommandon som ges när InputParser är i Default State
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private string ParseDefaultStateInput(string input)
        {
            string result;
            switch (input.ToLower())
            {
                case "?": // Inget break; eller return; => ramlar igenom till nästa case (dvs. ?/help hanteras likadant)
                case "help":
                    result = OutputHelper.RootCommandList;
                    break;
                case "exit":
                    ParseState = State.Exit; // Lägg märke till att vi utför en Action här.
                    result = OutputHelper.ExitMessage("Bye!"); // Det går bra att skicka parametrar
                    break;
                case "log":
                    result = OutputHelper.Put(Log());
                    break;
                case "func<int,bool>":
                    result = OutputHelper.FuncExplanation;
                    break;
                case "dictionary":
                    result = OutputHelper.Dictionary;
                    break;
                case "list":
                    result = OutputHelper.Put(UserToString(10));
                    break;
                case "listsorted":
                    result = OutputHelper.Put(UserToStringAsc(10));
                    break;
                case "listadmin":
                    result = OutputHelper.Put(UserToStringAdmin(HowMany()));
                    break;
                default:
                    result = OutputHelper.ErrorInvalidInput;
                    break;
            }
            
            return result + OutputHelper.EnterCommand;
        }

        public string Log()
        {
            return logger.ToString();
        }

        public int HowMany()
        {
            OutputHelper.Put("How Many? (Max 10)");
            string input = InputHelper.GetUserInput();
            OutputHelper.Put(input);
            int x = 10;
            try
            {
                x = Int32.Parse(input);
            }
            catch (FormatException e)
            {
                Console.WriteLine(e.Message);
            }
            return x;
        }

        public string UserToString(int Nbr)
        {
            string UserListString = "";
            foreach (var user in users.Take(Nbr)) {
                UserListString += user.ToString() + "\n";
            }
            return UserListString;
        }

        public string UserToStringAsc(int Nbr)
        {
            string UserListString = "";
            var query = from u in users
                        where u.FirstName != ""
                        orderby u.FirstName
                        select u;
            foreach (var user in query.Take(Nbr).ToList())
            {
                UserListString += user.ToString() + "\n";
            }
            return UserListString;
        }
        public string UserToStringAdmin(int Nbr)
        {
            string UserListString = "";
            if (Nbr > 10) {
                string ToMany = "Max 10 users! Not " + Nbr + "Users";
                Nbr = 10;
            }
            foreach (var user in users.Take(Nbr).Where(u => u.Type == User.UserType.Admin))
            {
                UserListString += user.ToString() + "\n";
            }
            return UserListString;
        }
        
    }
}
