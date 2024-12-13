using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using System.Collections.Generic;

namespace SpaceInvaders.Utils {
    internal static class Sprites {
        static public List<Texture2D> bullets = new(); // Every bullet excl. bosses sprite
        static public List<Texture2D> bossBullets = new(); // Every boss bullet sprite
        static public List<Texture2D> enemies = new(); // Every enemy sprite
        static public List<Texture2D> bosses = new(); // Every boss sprite
        static public List<Texture2D> particles = new(); // Particle Sprite
        static public List<Texture2D> powerboxes = new(); // Every enemy sprite
        static public List<Texture2D> bossbar = new(); // Bossbar sprites
        static public List<Texture2D> backgrounds = new(); // Bossbar sprites
    }

    internal static class SFX {
        static public List<SoundEffect> hitSounds = new(); // All hit sounds
        static public List<SoundEffect> shootSounds = new(); // All shooting sounds
        static public List<SoundEffect> deathSounds = new(); // All death sounds
    }

    internal static class Songs {
        static public List<Song> bossSongs = new(); // All boss songs
        static public List<Song> otherSongs = new(); // All other songs
    }
}
