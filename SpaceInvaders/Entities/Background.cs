using Microsoft.Xna.Framework.Graphics;

namespace SpaceInvaders.Entities {
    internal class Background : BasicObject {
        int subPixel;
        public Background(Texture2D sprite) {
            this.sprite = sprite;
            position = new(0,-384);
        }

        public void Update() {
            subPixel++;

            if (subPixel == 2) {
                subPixel = 0;
                position.Y++;
            }

            if (position.Y == 0) {
                position.Y = -384;
            }
        }
    }
}
