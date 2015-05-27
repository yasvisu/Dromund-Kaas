using System;

namespace DromundKaas
{
    /// <summary>
    /// Global Variables class.
    /// </summary>
    static class GlobalVar
    {
        //CONSOLE
        public const int CONSOLE_WIDTH = 80;
        public const int CONSOLE_HEIGHT = 25;

        //CONSOLE COLORS
        public const ConsoleColor ENEMY_BULLET_COLOR = ConsoleColor.Red;
        public const ConsoleColor PLAYER_BULLET_COLOR = ConsoleColor.White;

        //FILES
        public const string FILE_ENTITY_TYPES_DK = "../../EntityTypes.dk";
        public const string FILE_LEVELS_DK = "../../Levels.dk";

        //COUNTERS
        public static uint IDCounter;

    }
}
