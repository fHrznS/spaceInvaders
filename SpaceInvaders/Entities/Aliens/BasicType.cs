using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using SpaceInvaders.Utils;
using SpaceInvaders.Entities.Bullets;

namespace SpaceInvaders.Entities.Aliens {
    internal class BasicAlien : Alien {
        bool canShoot;

        public BasicAlien(int alienType, Vector2 startPosition, Rectangle hitbox, int id, bool free=false) {
            type = alienType;
            this.free = free;
            this.id = id;

            shootTimerReset = rng.Next(Time.ToFrames(seconds: 2) * type, Time.ToFrames(4) * type);
            shootTimer = shootTimerReset;

            direction = new Vector2(1, 1);
            position = startPosition;
            hitboxOffset = hitbox.Location;
            this.hitbox = new Rectangle(startPosition.ToPoint() + hitboxOffset, hitbox.Size);

            timerReset /= alienType;
            timer = timerReset;

        }

        internal override void Update(GameTime gameTime) {

            timer--;
            shootTimer--;

            offscreenBools();

            if (timer == 0) {
                if (free) {
                    if (position.X == 0 || position.X == 144) {
                        direction.X *= 1;
                        position.Y += 16 * direction.Y;
                    }
                } else {
                    if (gridPosition == 2 || gridPosition == -2) {
                        direction.X *= -1;
                        position.Y += 16 * direction.Y;
                    }
                }

                position.X += 8 * direction.X;
                gridPosition += (int)direction.X;
                timer = timerReset;
            }

            hitbox.Location = position.ToPoint() + hitboxOffset;

            if (shootTimer == 0) {
                if (canShoot && !Globals.disableEnemyShooting) {
                    Scenes.MainGame.newEnemyBullet<Bullet>(position, new(0, 2), type, damage: 1);
                }
                shootTimer = shootTimerReset;
            }
        }

        void offscreenBools() {
            if (position.Y < 0) {
                isImmune = true;
            } else {
                isImmune = false;
            }

            if (position.Y < -16) {
                canShoot = false;
            } else {
                canShoot = true;
            }
        }

        internal override void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(sprite, position, Color.White);
        }
    }
}
