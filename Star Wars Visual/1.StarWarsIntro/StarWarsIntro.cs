using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

class StarWarsIntro
{
    static void Main()
    {

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
            StarBeepIntro();
        };
        //TheImperialMarch();
        WriteTheConsoleGame();
        Console.Write("THE CONSOLE GAME");
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
            Console.Write("THE CONSOLE GAME".PadLeft(0 + i, ' '));
            System.Threading.Thread.Sleep(380);
            for (int j = 0; j < 16; j++)
            {
                Console.Write("\b");
            }
        }
    }

    static void WriteInAGalaxy()
    {
        Console.WriteLine(@"

  ___                  ____       _                  
 |_ _|_ __     __ _   / ___| __ _| | __ ___  ___   _ 
  | || '_ \   / _` | | |  _ / _` | |/ _` \ \/ / | | |
  | || | | | | (_| | | |_| | (_| | | (_| |>  <| |_| |
 |___|_| |_|  \__,_|  \____|\__,_|_|\__,_/_/\_\\__, |
                                               |___/ 

  _____                _____               _                                  
 |  ___|_ _ _ __      |  ___|_ _ _ __     / \__      ____ _ _   _             
 | |_ / _` | '__|     | |_ / _` | '__|   / _ \ \ /\ / / _` | | | |            
 |  _| (_| | |     _  |  _| (_| | |     / ___ \ V  V / (_| | |_| |  _   _   _ 
 |_|  \__,_|_|    ( ) |_|  \__,_|_|    /_/   \_\_/\_/ \__,_|\__, | (_) (_) (_)
                  |/                                        |___/             
");
    }

    static void WritePresents()
    {
        Console.WriteLine(@"
         _____  _____  ______  _____ ______ _   _ _______ _____      
         |  __ \|  __ \|  ____|/ ____|  ____| \ | |__   __/ ____|     
         | |__) | |__) | |__  | (___ | |__  |  \| |  | | | (___       
         |  ___/|  _  /|  __|  \___ \|  __| | . ` |  | |  \___ \      
         | |    | | \ \| |____ ____) | |____| |\  |  | |  ____) | _ _ 
         |_|    |_|  \_\______|_____/|______|_| \_|  |_| |_____(_|_|_)
");
    }

    static void WriteDromundKaas()
    {

        Console.WriteLine(@"  
                      _______                   
                     |__   __|                  
                        | | ___  __ _ _ __ ___  
                        | |/ _ \/ _` | '_ ` _ \ 
                        | |  __/ (_| | | | | | |
                        |_|\___|\__,_|_| |_| |_|
  _ _ _____                                      _   _  __              _ _ 
 ( | )  __ \                                    | | | |/ /             ( | )
  V V| |  | |_ __ ___  _ __ ___  _   _ _ __   __| | | ' / __ _  __ _ ___V V 
     | |  | | '__/ _ \| '_ ` _ \| | | | '_ \ / _` | |  < / _` |/ _` / __|   
     | |__| | | | (_) | | | | | | |_| | | | | (_| | | . \ (_| | (_| \__ \   
     |_____/|_|  \___/|_| |_| |_|\__,_|_| |_|\__,_| |_|\_\__,_|\__,_|___/   
");
    }

    static void WriteStarWars()
    {
        Console.WriteLine(@"             .               .    .          .              .   .         .
               _________________      ____         __________
 .       .    /                 |    /    \    .  |          \
     .       /    ______   _____| . /      \      |    ___    |     .     .
             \    \    |   |       /   /\   \     |   |___>   |
           .  \    \   |   |      /   /__\   \  . |         _/               .
 .     ________>    |  |   | .   /            \   |   |\    \_______    .
      |            /   |   |    /    ______    \  |   | \           |
      |___________/    |___|   /____/      \____\ |___|  \__________|    .
  .     ____    __  . _____   ____      .  __________   .  _________
       \    \  /  \  /    /  /    \       |          \    /         |      .
        \    \/    \/    /  /      \      |    ___    |  /    ______|  .
         \              /  /   /\   \ .   |   |___>   |  \    \
   .      \            /  /   /__\   \    |         _/.   \    \            +
           \    /\    /  /            \   |   |\    \______>    |   .
            \  /  \  /  /    ______    \  |   | \              /          .
 .       .   \/    \/  /____/      \____\ |___|  \____________/  
                               .                                        .
     .                           .         .               .                 .
");
        //string text = System.IO.File.ReadAllText(@"C:\Users\Nicks\Desktop\StarWarsLogo.txt");
        //Console.WriteLine("{0}", text);           // TAka se vadi ot tekstovi fail
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
}

