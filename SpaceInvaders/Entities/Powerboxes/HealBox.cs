using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceInvaders.Entities.Powerboxes {
    internal class HealBox : Powerbox {
        internal override void Update() {
            nextFrameTimer--;

            if (nextFrameTimer == 0) {
                nextFrameTimer = nextFrameTimerReset;

                if (frame == 5) {
                    frame = -1;
                }
                frame++;
                frameSource.Location = new Point(16 * frame, 0);
            }
        }

        internal override void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(sprite, position, frameSource, Color.White);
        }
    }
}
