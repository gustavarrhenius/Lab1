using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lab1.Model
{
    class Guest : IUser
    {
        public Guid UserID { get; set; }
        public String UserName { get; set; }

        public Guest()
        {
            this.UserName = "Guest";
            this.UserID = Guid.Empty;
        }
        public override string ToString()
        {
            string userString = string.Format("\tUserName: '{0}' - UserID: '{1}'", UserName, UserID);
            return userString;
        }

    }
}

