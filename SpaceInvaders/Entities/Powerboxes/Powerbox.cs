using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceInvaders.Entities.Powerboxes {
    internal abstract class Powerbox : BasicObject {
        internal Rectangle frameSource = new(0, 0, 16, 16);
        internal int frame = 0, // 6 Frames, 0-5
            nextFrameTimer = 30,
            nextFrameTimerReset = 30;
        internal int fallTimer, fallTimerReset;

        internal void Update() {
            nextFrameTimer--;
            fallTimer--;
            hitbox.Location = position.ToPoint();

            if (nextFrameTimer == 0) {
                nextFrameTimer = nextFrameTimerReset;

                if (frame == 5) {
                    frame = -1;
                }
                frame++;
                frameSource.Location = new Point(16 * frame, 0);
            }

            if (fallTimer == 0) {
                fallTimer = fallTimerReset;
                position.Y += 8;
            }
        }
        abstract internal void Draw(SpriteBatch spriteBatch);
    }
}
