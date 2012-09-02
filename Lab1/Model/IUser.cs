using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lab1.Model
{
    public interface IUser
    {
        string UserName { get; set; }
        Guid UserID { get; set; }
    }
}


