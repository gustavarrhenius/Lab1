using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lab1.Model
{
    class Logger
    {
        //Konstruktor för Klasse Logger
        public Logger() { }
        List<string> LogStrings = new List<string>();
        public void StringLog(string msg) {
            if (LogStrings.Count >= 10) {
                LogStrings.RemoveAt(0); 
            }
            LogStrings.Add(msg);
            }

        //Override metod för ToString
        public override string ToString()
        {
            int Counter = 1;
            string Text = "Last logged Commands\n";
            foreach (var String in LogStrings)  {
                Text += Counter++ + ". " + String.ToString() + "\n";
            }
            return Text;
        }

    }


}
