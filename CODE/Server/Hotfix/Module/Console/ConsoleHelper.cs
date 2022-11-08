using System;

namespace ET
{
    public static class ConsoleHelper
    {
        public static void WriteLine(string value)
        {
            if (Game.Scene.GetComponent<ConsoleComponent>() != null)
            {
                if (Game.Scene.GetComponent<ConsoleComponent>().GetComponent<ModeContex>() != null)
                {
                    return;
                }
            }

            Console.WriteLine(value);
        }

        public static void Write(string value)
        {
            if (Game.Scene.GetComponent<ConsoleComponent>() != null)
            {
                if (Game.Scene.GetComponent<ConsoleComponent>().GetComponent<ModeContex>() != null)
                {
                    return;
                }
            }

            Console.Write(value);
        }
    }
}