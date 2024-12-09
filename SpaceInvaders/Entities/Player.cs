using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using SpaceInvaders.Scenes;
using SpaceInvaders.Utils;
using System;

namespace SpaceInvaders.Entities {
    internal class Player : BasicObject {
        internal int health = 3;
        internal int maxBullets = 1;
        internal int bulletArmour = 0;
        internal bool splitBullet = true;
        internal int bulletSpeed = -5;
        internal int sheild = 0;
        internal int invincibility = 0;


        public Rectangle sourceRect = new(0, 0, 16, 16);

        public Player() {
            if (Globals.debug) {
                bulletSpeed = Globals.dbBSpeed;
                maxBullets = Globals.dbBCount;
                bulletArmour = Globals.dbDamage;
            }
        }

        internal void Update() {
            playerMovement();
            Point posAsPoint = position.ToPoint();
            hitbox.Location = new(posAsPoint.X + 1, posAsPoint.Y + 5);

            canShoot();

            if (MainGame.input.IsKeyDown(Keys.G) && MainGame.input != MainGame.previousInput && Globals.debug) {
                Globals.god = !Globals.god;
            }

            if (invincibility > 0) {
                invincibility--;
            }
        }

        void playerMovement() {
            if (MainGame.input.IsKeyDown(Controls.moveLeft) && position.X > 5) {
                position.X -= 2;
            }

            if (MainGame.input.IsKeyDown(Controls.moveRight) && position.X < Globals.screenWidth - 21) {
                position.X += 2;
            }
        }

        void canShoot() {
            if (MainGame.input == MainGame.previousInput) { return; }

            // Can the player shoot?
            if (MainGame.input.IsKeyDown(Controls.shoot) && Bullet.bulletCount != maxBullets) {
                if (maxBullets - Bullet.bulletCount >= 2 && splitBullet) {
                    MainGame.newPlayerBullet((int)position.X, new Vector2(0.5f * bulletSpeed / -5, bulletSpeed), 1 + bulletArmour);
                    MainGame.newPlayerBullet((int)position.X, new Vector2(-0.5f * bulletSpeed / -5, bulletSpeed), 1 + bulletArmour);
                    Bullet.bulletCount++;
                    SFX.shootSounds[0].Play();
                } else {
                    MainGame.newPlayerBullet((int)position.X, new Vector2(0, bulletSpeed), 1 + bulletArmour);
                    SFX.shootSounds[0].Play();
                }
                Bullet.bulletCount++;
            }
        }

        internal void registerDamage(int damage) {
            SFX.hitSounds[0].Play();
            if (invincibility != 0) { return; }
            if (sheild != 0) {
                sheild--;
                invincibility = Time.ToFrames(seconds: 1);
                updateSprite();
                return;
            }

            if (!Globals.god) {
                health -= damage;
                invincibility = Time.ToFrames(seconds: 2);
            }

            if (health <= 0) {
                health = 0;
                SFX.deathSounds[0].Play();
            }
            updateSprite();
        }
        internal void updateSprite() {
            if (sheild == 0) {
                sourceRect.Location = new(16 * 3 - 16 * health, 0);
            } else {
                sourceRect.Location = new(0, 16);
            }
        }

        internal void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(sprite, position, sourceRect, Color.White * ( 0 + 1 / (invincibility % 20 + 1f) + 0.3f) );
        }
    }
}
