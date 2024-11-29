namespace SpaceInvaders.Utils {
    public static class Globals {
        internal static int screenWidth = 160, screenHeight = 192; // 640x768
        internal static bool gameLost;
        internal static bool stopSpawn = true;
        internal static bool instantKillAttack = false;
        internal static bool invasionMode = false;

        internal static bool debug = false;
        internal static int dbWave = 4;
        internal static int dbBSpeed = -5; // -5 base
        internal static int dbBCount = 1; // 1 base
        internal static bool god = false;
    }
}
