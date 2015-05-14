using System;

namespace DromundKaas
{
    class Music
    {
        public static void PlayTune(int n)
        {
            switch(n)
            {
                case 0:
                    IntroTune();
                    break;
                case 1:
                    EnemyTune();
                    break;
                case 2:
                    BossTune();
                    break;
                    //...
            }
        }

        private static void IntroTune()
        {
            Console.Beep();
            //...
        }

        private static void EnemyTune()
        {
            int frequency = 0;
            int duration = 0;
            Console.Beep(frequency, duration);
        }

        private static void BossTune()
        {
            Console.Beep();
            Console.Beep();
        }

        //...

    }
}
