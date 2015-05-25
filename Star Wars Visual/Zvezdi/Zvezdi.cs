using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


class Zvezdi
{
    static void Main()
    {
        //string[] text = System.IO.File.ReadLines(@"..\..\..\1.StarWarsintro\WriteStarWars.txt").ToArray();
        Console.SetBufferSize(80, 80);
        char[,] zvezdi = new char[30, 80];

        Random randomZvezdi = new Random();
        randomZvezdi.Next(0, 80);
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
                    Console.Write(zvezdi[i,j]);
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
        StarwarsInroText();

        //Queue<string> zvezdiQ = new Queue<string>();
        //for (int i = 0; i < 30; i++)
        //{
        //    string[] temp = new string[zvezdi.Length];
        //    for (int j = 0; j < 80; j++)
        //    {
        //        temp[i] = temp[i] + zvezdi[(char)i, (char)j];
        //    }
        //    zvezdiQ.Enqueue(temp[i]);
        //}
        //foreach (var item in zvezdiQ)
        //{
        //    Console.Write(allStars);
        //    Console.WriteLine(item);
        //}


        //for (int i = 0; i < zvezdi.GetLength(0); i++)
        //{
        //    for (int j = 0; j < zvezdi.GetLength(1); j++)
        //    {
        //        if (text[i,j])
        //        {

        //        }
        //    }
        //    Console.WriteLine();
        //}
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
        statText.Enqueue("these brave codders");
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
    }
}

