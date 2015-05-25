using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
class StarWarsIntro
{
    static void Main()
    {
        Console.SetBufferSize(80, 40);
        Console.CursorVisible = false;
        System.Threading.Thread.Sleep(500);
        WriteDromundKaas();
        System.Threading.Thread.Sleep(3000);
        WritePresents();
        System.Threading.Thread.Sleep(3000);
        Console.Clear();
        WriteInAGalaxy();
        System.Threading.Thread.Sleep(5000);
        Console.Clear();
        WriteStarWars();
        for (int x = 0; x < 2; x++)
        {
            //StarBeepIntro();
        };
        //TheImperialMarch();
        System.Threading.Thread.Sleep(2000);
        StarwarsInroText();
        WriteTheConsoleGame();
        Console.Write("BEGIN  THE  GAME");
        Console.WriteLine();
    }


    private static void TheImperialMarch()
    {
        Console.Beep(440, 500);
        Console.Beep(440, 500);
        Console.Beep(440, 500);
        Console.Beep(349, 350);
        Console.Beep(523, 150);
        Console.Beep(440, 500);
        Console.Beep(349, 350);
        Console.Beep(523, 150);
        Console.Beep(440, 1000);
        Console.Beep(659, 500);
        Console.Beep(659, 500);
        Console.Beep(659, 500);
        Console.Beep(698, 350);
        Console.Beep(523, 150);
        Console.Beep(415, 500);
        Console.Beep(349, 350);
        Console.Beep(523, 150);
        Console.Beep(440, 1000);
    }

    private static void WriteTheConsoleGame()
    {
        for (int i = 0; i < 37; i = i + 9)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write("BEGIN  THE  GAME".PadLeft(0 + i, ' '));
            System.Threading.Thread.Sleep(380);
            for (int j = 0; j < 16; j++)
            {
                Console.Write("\b");
            }
            Console.ForegroundColor = ConsoleColor.White;
        }
    }

    static void WriteInAGalaxy()
    {
        string text = System.IO.File.ReadAllText(@"..\..\WriteInAGalaxy.txt");
        Console.WriteLine(text);
    }

    static void WritePresents()
    {
        string text = System.IO.File.ReadAllText(@"..\..\WritePresents.txt");
        Console.WriteLine(text);
    }

    static void WriteDromundKaas()
    {
        string text = System.IO.File.ReadAllText(@"..\..\WriteDromundKaas.txt");
        Console.WriteLine(text);
    }

    static void WriteStarWars()
    {
        char[,] zvezdi = new char[30, 80];

        Random randomZvezdi = new Random();
        //randomZvezdi.Next(0, 80);
        char[][] allStars = new char[19][];

        using (var textRead = new StreamReader(@"..\..\..\1.StarWarsintro\WriteStarWars.txt"))
        {
            for (int i = 0; i < 19; i++)
            {
                var line = textRead.ReadLine();
                allStars[i] = line.ToCharArray();
            }
        }

        for (int i = 0; i < 150; i++)
        {
            zvezdi[randomZvezdi.Next(0, 30), randomZvezdi.Next(0, 80)] = '.';
        }

        for (int i = 0; i < allStars.Length; i++)
        {
            for (int j = 0; j < allStars[i].Length; j++)
            {
                if (allStars[i][j] == ' ')
                {
                    Console.Write(zvezdi[i, j]);
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write(allStars[i][j]);
                    Console.ForegroundColor = ConsoleColor.White;
                }
            }
            Console.WriteLine();
        }
        
        //string text = System.IO.File.ReadAllText(@"..\..\WriteStarWars.txt");
        //Console.WriteLine(text);

        //Console.WriteLine(@"");
        //string text = System.IO.File.ReadAllText(@"C:\Users\Nicks\Desktop\StarWarsLogo.txt");
        //Console.WriteLine("{0}", text);           // TAka se vadi ot tekstovi fail
        // "proj/bin/Debug/blabla.exe"
        //->
        // "proj/x/baba"
        //-> "./../../x/baba"
    }
    static void StarBeepIntro()
    {
        rerere(200);
        Console.Beep(783, 1200);
        Console.Beep(1174, 1200);
        dosila(200);
        Console.Beep(1567, 1200);
        Console.Beep(1174, 600);
        dosila(200);
        Console.Beep(1567, 1200);
        Console.Beep(1174, 600);
        dosido(200);
        Console.Beep(880, 1200);
    }
    static void rerere(int d)
    {
        for (int x = 0; x < 3; x++)
        {
            Console.Beep(523, d);
        }
    }
    static void dosila(int d)
    {
        Console.Beep(1046, d);
        Console.Beep(987, d);
        Console.Beep(880, d);
    }
    static void dosido(int d)
    {
        Console.Beep(1046, d);
        Console.Beep(987, d);
        Console.Beep(1046, d);
    }
    private static void StarwarsInroText()
    {
        Console.WriteLine();
        Queue<string> statText = new Queue<string>();
        statText.Enqueue("A long time ago, in a galaxy far,");
        statText.Enqueue("far away....");
        statText.Enqueue("  ");
        statText.Enqueue("It is a period of serious code writing.");
        statText.Enqueue("Rebel new programmers, striking");
        statText.Enqueue("from a hidden base, at SoftUni");
        statText.Enqueue("had their first victory against");
        statText.Enqueue("the Programming Basics Exam.");
        statText.Enqueue("  ");
        statText.Enqueue("After the battle, rebel");
        statText.Enqueue("codders managed to write");
        statText.Enqueue("their entire homeworks!!!");
        statText.Enqueue("During this intensive code writing");
        statText.Enqueue("they were able to create");
        statText.Enqueue("an entirely new Console Game");
        statText.Enqueue("Which will be able to impress");
        statText.Enqueue("the judging commissions' hearts!");
        statText.Enqueue(" ");
        statText.Enqueue("Pursued by the Nakov empire's");
        statText.Enqueue("strick and powerful lectors");
        statText.Enqueue("these brave new codders");
        statText.Enqueue("are preparing for ");
        statText.Enqueue("the C# Advanced Exam.");
        statText.Enqueue("Succeeding in this task");
        statText.Enqueue("will bring peace and freedom");
        statText.Enqueue("to them.....");
        statText.Enqueue(" ");
        statText.Enqueue(" ");
        statText.Enqueue("at least for a couple of days");


        Console.ForegroundColor = ConsoleColor.Yellow;
        int move = 0;
        foreach (var item in statText)
        {
            if (move > 28)
            {
                break;
            }
            Console.SetCursorPosition(20, 20 + move);
            Console.WriteLine("{0}", item.PadRight(20));
            System.Threading.Thread.Sleep(500);
            move++;
        }
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine();
    }
}

