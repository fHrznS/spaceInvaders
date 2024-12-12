using System.Collections.Generic;

namespace SpaceInvaders.Utils {
    public static class Globals {
        internal static int screenWidth = 160, screenHeight = 192; // 640x768
        internal static bool gameLost;
        internal static bool stopSpawn = true;
        internal static bool disableEnemyShooting = false;
        internal static bool invasionMode = false;
        internal static bool disableEnemySpawning = false;

        internal static bool isMultiplayer = false;

        internal static bool debug = false;
        internal static int dbWave = 49; // 298
        internal static int dbBSpeed = -17; // -5 base
        internal static int dbBCount = 5; // 1 base
        internal static int dbDamage = 3; // 0 base
        internal static bool god = false;
        internal static bool summonSecondBoss = false;

        public static bool isLoading = false;

        internal static bool easyDifficulty = false;
        internal static bool isWorthy = true;
        internal static Dictionary<string, int> SaveData = new Dictionary<string, int>();
    }
}
