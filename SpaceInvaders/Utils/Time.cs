namespace SpaceInvaders.Utils {
    internal static class Time {
        /// <summary>
        /// Util for converting seconds (60 FPS) to and int (frames).
        /// Vice versa if you wanna convert frames to seconds.
        /// </summary>

        static public int ToSeconds(int frames) {
            return frames / 60;
        }
        public static int ToFrames(int seconds, int minutes = 0) {
            return seconds * 60 + minutes * 60 * 60;
        }
    }
}
