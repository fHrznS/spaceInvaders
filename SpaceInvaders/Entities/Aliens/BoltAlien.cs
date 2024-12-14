using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceInvaders.Utils;
using System;

namespace SpaceInvaders.Entities.Aliens {
    internal class BoltAlien : Alien {
        public BoltAlien(int alienType, Vector2 startPosition, Rectangle hitbox, int id, int multID) {
            this.id = id;
            type = alienType;
            position = startPosition;
            this.multID = multID;

            hitboxOffset = hitbox.Location;
            this.hitbox = new Rectangle(position.ToPoint() + hitboxOffset, hitbox.Size);
            
            direction = new(0, 1);

            timerReset = 30;
            timer = timerReset;
        }

        internal override void Update(GameTime gameTime) {
            timer--;

            offscreenBools();

            if (timer == 0) {
                position.Y += 8 * direction.Y / (1 * (Globals.disableEnemyShooting ? 3 : 1)); // Slow down if boss says so
                timer = timerReset;
            }
            hitbox.Location = position.ToPoint() + hitboxOffset;

        }

        void offscreenBools() {
            if (position.Y < -14) {
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
