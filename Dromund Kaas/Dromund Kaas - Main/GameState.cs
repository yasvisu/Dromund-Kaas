using System;
using System.Collections.Generic;

namespace DromundKaas
{
    class GameState
    {
        //GENERATORS
        public Random RAND;

        //ENEMY MAP VARIABLES
        public List<char[]> ENEMY_MAT;
        public List<char[]> ENEMY_BULLET_MAT;

        //PLAYER MAP VARIABLES
        public List<char[]> PLAYER_MAT;
        public List<char[]> PLAYER_BULLET_MAT;

        //BACKGROUND MATRIX VARIABLE
        public List<char[]> BG_MAT;

        private List<char[]> PAINTING;

        //Default constructor
        public GameState()
        {
            RAND = new Random();

            LoadMat(ENEMY_MAT);
            LoadMat(ENEMY_BULLET_MAT);

            LoadMat(PLAYER_MAT);
            LoadMat(PLAYER_BULLET_MAT);

            LoadMat(BG_MAT);
        }

        public override string ToString()
        {
            return base.ToString();
        }

        //Draw the game state
        public void Draw()
        {
            LayerMats();
            foreach (var v in PAINTING)
                Console.WriteLine(string.Concat(v));
        }

        //Layer all game state mats
        private void LayerMats()
        {
            LoadMat(PAINTING);
            for (int i = 0; i < GlobalVar.CONSOLE_HEIGHT; i++)
            {
                for (int j = 0; j < GlobalVar.CONSOLE_WIDTH; j++)
                {
                    if (PLAYER_MAT[i][j] != ' ')
                        PAINTING[i][j] = PLAYER_MAT[i][j];
                    else if (ENEMY_MAT[i][j] != ' ')
                        PAINTING[i][j] = ENEMY_MAT[i][j];
                    else if (PLAYER_BULLET_MAT[i][j] != ' ')
                        PAINTING[i][j] = PLAYER_BULLET_MAT[i][j];
                    else if (ENEMY_BULLET_MAT[i][j] != ' ')
                        PAINTING[i][j] = ENEMY_BULLET_MAT[i][j];
                    else
                        PAINTING[i][j] = BG_MAT[i][j];
                }
            }
        }


        //STATIC HELPERS
        private static bool IsEmpty(char c)
        {
            return c == ' ';
        }

        //Set new and fill
        private static void LoadMat(List<char[]> mat)
        {
            mat = new List<char[]>();
            for (int i = 0; i < GlobalVar.CONSOLE_HEIGHT; i++)
                mat.Add(new char[GlobalVar.CONSOLE_WIDTH]);
        }
    }
}