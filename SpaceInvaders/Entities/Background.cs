using Microsoft.Xna.Framework.Graphics;

namespace SpaceInvaders.Entities {
    internal class Background : BasicObject {
        public Background(Texture2D sprite) {
            this.sprite = sprite;
            position = new(0,-384);
        }

        public void Update() {
            position.Y++;

            if (position.Y == 0) {
                position.Y = -384;
            }
        }
    }
}
