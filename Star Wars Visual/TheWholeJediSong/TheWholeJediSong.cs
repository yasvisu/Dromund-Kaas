//Stars Wars
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class TheWholeJediSong
{

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
    static void partI_starwars()
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
    static void reremimi()
    {
        Console.Beep(523, 400);
        Console.Beep(523, 200);
        Console.Beep(659, 900);
        Console.Beep(659, 300);
    }

    static void frase000()
    {
        Console.Beep(1046, 300);
        Console.Beep(987, 300);
        Console.Beep(880, 300);
        Console.Beep(783, 300);

        Console.Beep(783, 300);
        Console.Beep(880, 150);
        Console.Beep(987, 150);
        Console.Beep(880, 300);
        Console.Beep(659, 300);
        Console.Beep(733, 600);
    }

    static void frase001()
    {
        Console.Beep(1046, 300);
        Console.Beep(987, 300);
        Console.Beep(880, 300);
        Console.Beep(783, 300);

        Console.Beep(1174, 900);
        Console.Beep(880, 300);
        Console.Beep(880, 600);
    }

    static void frase002()
    {
        Console.Beep(1174, 400);
        Console.Beep(1174, 200);
        Console.Beep(1567, 400);
        Console.Beep(1396, 200);
        Console.Beep(1244, 400);
        Console.Beep(1174, 200);
        Console.Beep(1046, 400);
        Console.Beep(923, 200); 
        Console.Beep(880, 400);  
        Console.Beep(783, 200);  
        Console.Beep(1174, 600);
        for (int x = 0; x < 3; x++)
        {
            Console.Beep(880, 200);
        }
        Console.Beep(880, 600);
    }
    static void partII_starwars()
    {
        reremimi(); frase000();
        reremimi(); frase001();
        reremimi(); frase000();
        frase002();
    }
    static void final()
    {
        for (int x = 0; x < 3; x++)
        {
            Console.Beep(1174, 200);
        }
        Console.Beep(1567, 1800);

        for (int x = 0; x < 3; x++)
        {
            Console.Beep(783, 200);
        }
        Console.Beep(783, 1800);
    }
    static void Main()
    {
        
        for (int x = 0; x < 2; x++) { partI_starwars(); }
        
        partII_starwars();
       
        for (int x = 0; x < 2; x++) { partI_starwars(); }
        
        final();
    }

}
