using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lab1.Model
{
    public class User
    {
        public User() { }
        public User(string name, UserType type)
        {
            UserID = Guid.NewGuid();
            UserName = name;
            Type = type;
        }

        public Guid UserID { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string FullName
        {
            get
            {
                if (string.IsNullOrEmpty(FirstName) && string.IsNullOrEmpty(LastName))
                    return "NoName";
                else if (string.IsNullOrEmpty(FirstName))
                    return LastName;
                else if (string.IsNullOrEmpty(LastName))
                    return FirstName;
                else
                    return string.Format("{0} {1}", FirstName, LastName);
            }
        }

        public UserType Type { get; set; }

        public string ToString(bool ShortFormat = true)
        {
            string userString = string.Format("\tFullName: '{0}' - UserID: '{1}'", FullName, UserID);
            if (!ShortFormat)
                userString += string.Format("\n\t\tUserName: '{0}' - UserType: '{1}'", UserName, Type);
            return userString;
        }

        public enum UserType
        {
            User,
            Admin,
            SuperUser
        }
    }
}
