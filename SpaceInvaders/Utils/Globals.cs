﻿namespace SpaceInvaders.Utils {
    public static class Globals {
        internal static int screenWidth = 160, screenHeight = 192; // 640x768
        internal static bool gameLost;
        internal static bool stopSpawn = true;
        internal static bool instantKillAttack = false;
        internal static bool invasionMode = false;
        internal static bool disableEnemySpawning = false;

        internal static bool debug = true;
        internal static int dbWave = 198;
        internal static int dbBSpeed = -16; // -5 base
        internal static int dbBCount = 10; // 1 base
        internal static int dbDamage = 3; // 0 base
        internal static bool god = false;
        internal static bool summonSecondBoss = false;
    }
}
