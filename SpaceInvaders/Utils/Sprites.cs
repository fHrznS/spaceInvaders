using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace SpaceInvaders.Utils {
    internal static class Sprites {
        static public List<Texture2D> bullets = new(); // Every bullet excl. bosses sprite
        static public List<Texture2D> bossBullets = new(); // Every boss bullet sprite
        static public List<Texture2D> enemies = new(); // Every enemy sprite
        static public List<Texture2D> bosses = new(); // Every boss sprite
        static public List<Texture2D> particles = new(); // Particle Sprite
        static public List<Texture2D> powerboxes = new(); // Every enemy sprite
    }
}
