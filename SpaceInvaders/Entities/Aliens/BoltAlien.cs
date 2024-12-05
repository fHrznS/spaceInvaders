using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceInvaders.Utils;
using System;

namespace SpaceInvaders.Entities.Aliens {
    internal class BoltAlien : Alien {
        public BoltAlien(int alienType, Vector2 startPosition, Rectangle hitbox, int id) {
            this.id = id;
            type = alienType;
            position = startPosition;
            hitboxOffset = hitbox.Location;

            this.hitbox = new Rectangle(startPosition.ToPoint() + hitboxOffset, hitbox.Size);
            direction = new(0, 1);

            timerReset = 30;
            timer = timerReset;
        }

        internal override void Update(GameTime gameTime) {
            timer--;

            offscreenBools();

            if (timer == 0) {
                position.Y += 8 * direction.Y / (1 * (Globals.disableEnemyShooting ? 3 : 1)); // Slow down if instant kill attack
                timer = timerReset;
            }
            hitbox.Location = position.ToPoint() + hitboxOffset;

        }

        void offscreenBools() {
            if (position.Y < 0) {
                isImmune = true;
            } else {
                isImmune = false;
            }
        }

        internal override void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(sprite, position, Color.White);
        }
    }
}
