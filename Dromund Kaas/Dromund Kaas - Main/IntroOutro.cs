//ZA NIKI

using System;

//notice the namespace is the same
namespace DromundKaas
{
    class IntroOutro
    {
        public static void PrintSomething(string s)
        {
            Console.WriteLine(s + " baba");
        }
        
        public static void Intro()
        {
            for (int i = 0; i < 10; i++)
            {
                //Play Intro Tune
                Music.PlayTune(0);

                //Print Intro
                Console.BackgroundColor = ConsoleColor.DarkMagenta; //or other BACKGROUND colors
                Console.ForegroundColor = ConsoleColor.Cyan; //or other LETTER colors
                Console.ResetColor(); //return to normal colors
                //...
            }
        }

        public static void Outro()
        {
            for (int i = 0; i < 10; i++)
            {
                //Print Outro / credits
            }
            Console.WriteLine("Fin.");
        }
    }
}
