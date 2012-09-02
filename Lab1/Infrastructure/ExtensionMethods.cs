using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lab1.Infrastructure
{
    //Skriver ut en sträng med två radbrytningar innan
    public static class ExtensionMethods
    {
        public static string PrefixDoubleNewLine(this string Str)
        {
            return "\n\n" + Str;
        }
    }
}
