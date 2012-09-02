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

        private Model.Repository.Abstract.IRepository repo;
        private Logger logger = new Logger();
        List<User> users;
        List<Post> posts;
        IUser InloggedUser = new Guest();
        string UserName = "";
        Int32 CurrentUser = 0;

        private static bool IsUserAuthenticated = false;

        //Konstruktor för InputParser
        public InputParser(Model.Repository.Abstract.IRepository repo)
        {
            this.repo = repo;
            this.users = repo.GetUsers();
            this.posts = repo.GetPosts();
            
        }
        State ParseState = State.Default;
        private enum State 
        {
            Default,
            Create,
            ListUser,
            Exit
        }
        /// <summary>
        /// Returnerar true om ParserState är Exit
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
            else if (ParseState == State.Create || IsUserAuthenticated)
            {
                return CreatUser(input);
            }
            else if (ParseState == State.ListUser)
            {
                return ListUser(input);
            }
            else if (ParseState == State.Exit)
            {
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
            string result = "";
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
                    OutputHelper.Put(Log());
                    break;
                case "func<int,bool>":
                    result = OutputHelper.FuncExplanation;
                    break;
                case "dictionary":
                    result = OutputHelper.Dictionary;
                    break;
                case "list":
                    OutputHelper.Put(UserToStringAsc(10));
                    ParseState = State.ListUser;
                    break;
                case "listsorted":
                    OutputHelper.Put(UserToStringAsc(10));
                    break;
                case "listadmin":
                    OutputHelper.Put(UserToStringAdmin(10));
                    break;
                case "login admin":
                    IsUserAuthenticated = true;
                    InloggedUser = users.Where(u => u.Type == User.UserType.Admin).FirstOrDefault();
                    OutputHelper.Put("You are now loggedin!");
                    break;
                case "logout":
                    IsUserAuthenticated = false;
                    OutputHelper.Put("Logged out");
                    break;
                case "create":
                    if (IsUserAuthenticated) {
                        OutputHelper.Put("Create new user by writing name:[X]");
                        ParseState = State.Create;
                    } else {
                        OutputHelper.Put("You need to log in to creat a user!");
                    }
                    break;
                case "listlatesttroll":
                    OutputHelper.Put(ListLatestTroll());
                    break;
                default:
                    result = OutputHelper.ErrorInvalidInput;
                    break;
            }
            return result + OutputHelper.EnterCommand;
        }


        //Create User State
        private string CreatUser(string input)
        {
            string result = "";
            switch (input.ToLower())
            {
                case "?": 
                case "help":
                    result = OutputHelper.AdminCommandList;
                    break;
                case "exit":
                    ParseState = State.Exit; 
                    result = OutputHelper.ExitMessage("Bye!");  
                    break;
                case "save":
                    if (UserName != "")  {
                        User newUser = new User();
                        newUser.UserName = UserName;
                        repo.AddUser(newUser);
                        OutputHelper.Put("New user was created!");
                    } else {
                        OutputHelper.Put("Wrong username");
                    }
                    break;
                case "cancel":
                    OutputHelper.Put("you are now back in default state");
                    ParseState = State.Default;
                    break;
                default:
                    int InputNameStart = input.IndexOf(":");
                    if (InputNameStart > 0) {
                        UserName = input.Substring(InputNameStart + 1).Trim();
                        OutputHelper.Put("If you want the username to be '" + UserName + "' write 'save' and the user will be created!");
                    } else {
                        result = OutputHelper.ErrorInvalidInput;
                    } 
                    break;
            }

            return result + OutputHelper.EnterCommand;
        }

        //ListUser State
        private string ListUser(string input)
        {
            string result = "";
            switch (input.ToLower())
            {
                case "?":
                case "help":
                    result = OutputHelper.AdminCommandList;
                    break;
                case "exit":
                    ParseState = State.Exit; 
                    result = OutputHelper.ExitMessage("Bye!"); 
                    break;
                case "back":
                    OutputHelper.Put("You are now back in default state");
                    ParseState = State.Default;
                    CurrentUser = 0;
                    break;
                case "next":
                    CurrentUser += 10;
                    OutputHelper.Put(UserToStringAscNextPrev(CurrentUser));
                    break;
                case "prev":
                    CurrentUser -= 10;
                    if (CurrentUser <= 0) { CurrentUser = 0; }
                    OutputHelper.Put(UserToStringAscNextPrev(CurrentUser));
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

        /*Fråga 12 **
         * public string UserToString(int Nbr)
        {
            string UserListString = "";
            foreach (var user in users.Take(Nbr)) {
                UserListString += user.ToString() + "\n";
            }
            return UserListString;
        }*/

        //Returnerar en lista på users i Asc order
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

        //Returnerar en lista på users med next och prev funktionalitet
        public string UserToStringAscNextPrev(int Current)
        {
            string UserListString = "";
            
            var UsersList = users.Where(u => u.FirstName != "").OrderBy(u => u.FirstName).Skip(Current).Take(10);
            if (UsersList.Count() <= 0)
            {
                CurrentUser -= 10;
                UsersList = users.Where(u => u.FirstName != "").OrderBy(u => u.FirstName).Skip(Current-10).Take(10);
            }
            foreach (var user in UsersList){
                UserListString += user.ToString() + "\n";
            }
            return UserListString;
        }

        //Returnerar en lista på admins
        public string UserToStringAdmin(int Nbr)
        {
            string UserListString = "";
            int Admins = users.Where(u => u.Type == User.UserType.Admin).Count();
            foreach (var user in users.Where(u => u.Type == User.UserType.Admin))
            {
                UserListString += user.ToString() + "\n";
            }
            return "There are " + Admins + " Admins\n" + UserListString;
        }

        //Returnerar en lista på de senaste posterna med troll som tag
        public string ListLatestTroll()
        {
            string ListLatestTroll = posts.Where(p => p.Tags.Any(t => t == Post.PostTags.Troll)).OrderBy(p => p.CreateDate.Date).First().ToString();
            return ListLatestTroll;
        }
        
    }
}
