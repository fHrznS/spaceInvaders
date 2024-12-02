namespace SpaceInvaders.Utils {
    internal static class Time {
        static public int ToSeconds(int frames) {
            return frames / 60;
        }
        public static int ToFrames(int seconds, int minutes = 0) {
            return seconds * 60 + minutes * 60 * 60;
        }
    }
}
