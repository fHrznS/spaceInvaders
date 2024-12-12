using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceInvaders.Entities {
    internal abstract class BasicObject {
        /// <summary>
        /// This class is used as the grounds for every entity.
        /// All it contains are variables all entity has in common, such as sprite and position.
        /// </summary>

        public Texture2D sprite;
        public Vector2 position;
        public Vector2 direction;
        public Rectangle hitbox;
        public int id;
        public int multID;
    }
}
