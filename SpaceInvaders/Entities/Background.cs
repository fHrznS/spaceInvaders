using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceInvaders.Entities {
    internal class Background : BasicObject {
        public Background(Texture2D sprite, Vector2 offset) {
            this.sprite = sprite;
            this.position = offset;
        }
    }
}
