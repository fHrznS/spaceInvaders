using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceInvaders.Utils;
using System;

namespace SpaceInvaders.Entities.Aliens {
    internal class SkyDiverAlien : Alien {
        public SkyDiverAlien(int alienType, Vector2 startPosition, Rectangle hitbox, int id, int multID) {
            this.id = id;
            type = alienType;
            position = startPosition;
            hitboxOffset = hitbox.Location;
            this.multID = multID;

            this.hitbox = new Rectangle(position.ToPoint() + hitboxOffset, hitbox.Size);
            direction = new(1, 1);

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
            if (timer % 15 == 0) {
                if (position.X == 0 || position.X == 144) {
                    direction.X *= -1;
                }

                position.X += 8 * direction.X;
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