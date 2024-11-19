using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SpaceInvaders.Scenes;
using SpaceInvaders.Utils;

namespace SpaceInvaders.Entities {
    internal class Player : BasicObject {
        internal int health = 3;
        private int maxBullets = 1;
        public Rectangle sourceRect = new(0, 0, 16, 16);

        internal void Update() {
            playerMovement();
            Point posAsPoint = position.ToPoint();
            hitbox.Location = new(posAsPoint.X + 1, posAsPoint.Y + 1);

            canShoot();
        }

        void playerMovement() {
            if (MainGame.input.IsKeyDown(Keys.Left) && position.X > 5) {
                position.X -= 2;
            }

            if (MainGame.input.IsKeyDown(Keys.Right) && position.X < Globals.screenWidth - 21) {
                position.X += 2;
            }
        }

        void canShoot() {
            if (MainGame.input == MainGame.previousInput) { return; }
            if (MainGame.input.IsKeyDown(Keys.Z) && Bullet.bulletCount != maxBullets) {
                MainGame.newPlayerBullet((int)position.X);
                Bullet.bulletCount++;
            }
        }

        internal void registerDamage() {
            health--;

            sourceRect.Location = new(16 * 3 - 16 * health, 0);
        }
        internal void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(sprite, position, sourceRect, Color.White);
        }
    }
}
