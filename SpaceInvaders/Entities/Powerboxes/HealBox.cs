using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceInvaders.Entities.Powerboxes {
    internal class HealBox : Powerbox {
        public HealBox(Vector2 position, Texture2D sprite, int fallTimer, int id) {
            this.id = id;
            this.position = position;
            fallTimerReset = fallTimer;
            this.fallTimer = fallTimerReset;
            this.sprite = sprite;

            hitbox = new(position.ToPoint(), new(16, 16));
        }

        internal override void Update() {
            
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

        internal override void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(sprite, position, frameSource, Color.White);
        }
    }
}
