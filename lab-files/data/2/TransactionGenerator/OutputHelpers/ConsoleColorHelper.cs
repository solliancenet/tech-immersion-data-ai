using System;
using System.Collections.Generic;
using System.Text;

namespace TransactionGenerator.OutputHelpers
{
    public static class ConsoleColorHelper
    {
        public static ConsoleColor ToConsoleColor(this Color color)
        {
            switch (color)
            {
                case Color.White:
                case Color.Default:
                    return ConsoleColor.White;
                case Color.Green:
                    return ConsoleColor.Green;
                case Color.Magenta:
                    return ConsoleColor.Magenta;
                case Color.Red:
                    return ConsoleColor.Red;
                case Color.Yellow:
                    return ConsoleColor.Yellow;
                case Color.Cyan:
                    return ConsoleColor.Cyan;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
