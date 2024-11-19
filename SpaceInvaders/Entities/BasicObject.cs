using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceInvaders.Entities {
    internal abstract class BasicObject {
        public Texture2D sprite;
        public Vector2 position;
        public Vector2 direction;
        public Rectangle hitbox;
        public int id;
    }
}
