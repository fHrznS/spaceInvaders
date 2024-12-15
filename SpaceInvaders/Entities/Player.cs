using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SpaceInvaders.Scenes;
using SpaceInvaders.Utils;
using System.Collections.Generic;
using System.Linq;

namespace SpaceInvaders.Entities {
    internal class Player : BasicObject {
        internal int health = 3;
        internal int maxBullets = 1;
        internal int bulletArmour = 0;
        internal bool splitBullet = false;
        internal int bulletSpeed = -5;
        internal int sheild = 0;
        internal int invincibility = 0;

        List<SoundEffectInstance> shootSounds = new();

        public Rectangle sourceRect = new(0, 0, 16, 16);

        public Player() {
            if (Globals.debug) {
                bulletSpeed = Globals.dbBSpeed;
                maxBullets = Globals.dbBCount;
                bulletArmour = Globals.dbDamage;
            }

            position = new Vector2(72, 160);
            hitbox = new(1, 4, 14, 6);
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
            if (multID == 0) {
                if (Globals.isMultiplayer) {
                    if (MainGame.input.IsKeyDown(Keys.A) && position.X > 5) {
                        position.X -= 2;
                    }

                    if (MainGame.input.IsKeyDown(Keys.D) && position.X < Globals.screenWidth - 21) {
                        position.X += 2;
                    }
                } else {
                    if (MainGame.input.IsKeyDown(Controls.P1moveLeft) && position.X > 5) {
                        position.X -= 2;
                    }

                    if (MainGame.input.IsKeyDown(Controls.P1moveRight) && position.X < Globals.screenWidth - 21) {
                        position.X += 2;
                    }
                }
            } else if (multID == 1) {
                if (MainGame.input.IsKeyDown(Keys.J) && position.X > 5) {
                    position.X -= 2;
                }

                if (MainGame.input.IsKeyDown(Keys.L) && position.X < Globals.screenWidth - 21) {
                    position.X += 2;
                }
            }
        }

        void canShoot() {
            if (shootSounds.Count > 5) {
                shootSounds.RemoveAt(0);
            }

            if (MainGame.input == MainGame.previousInput && multID == 0) { return; }
            if (MainGame.input == MainGame.P2previousInput && multID == 1) { return; }

            if (multID == 0) {
                // Can the player shoot?
                if ((Globals.isMultiplayer ? MainGame.input.IsKeyDown(Keys.S) : MainGame.input.IsKeyDown(Controls.P1shoot))
                    && MainGame.P1bullets.Count != maxBullets) {
                    if (maxBullets - MainGame.P1bullets.Count >= 2 && splitBullet) {
                        MainGame.newPlayerBullet((int)position.X, new Vector2(0.5f * bulletSpeed / -5, bulletSpeed), 1 + bulletArmour, 0);
                        MainGame.newPlayerBullet((int)position.X, new Vector2(-0.5f * bulletSpeed / -5, bulletSpeed), 1 + bulletArmour, 0);

                        shootSounds.Add(SFX.shootSounds[0].CreateInstance());
                        shootSounds.Last().Volume = (float)Globals.sfxVolume;
                        shootSounds.Last().Play();
                    } else {
                        MainGame.newPlayerBullet((int)position.X, new Vector2(0, bulletSpeed), 1 + bulletArmour, 0);

                        shootSounds.Add(SFX.shootSounds[0].CreateInstance());
                        shootSounds.Last().Volume = (float)Globals.sfxVolume;
                        shootSounds.Last().Play();
                    }
                }
            }
            else if (multID == 1) {
                // Can the player shoot?
                if (MainGame.input.IsKeyDown(Keys.K) && MainGame.P2bullets.Count != maxBullets) {
                    if (maxBullets - MainGame.P2bullets.Count >= 2 && splitBullet) {
                        MainGame.newPlayerBullet((int)position.X, new Vector2(0.5f * bulletSpeed / -5, bulletSpeed), 1 + bulletArmour, 1);
                        MainGame.newPlayerBullet((int)position.X, new Vector2(-0.5f * bulletSpeed / -5, bulletSpeed), 1 + bulletArmour, 1);

                        shootSounds.Add(SFX.shootSounds[0].CreateInstance());
                        shootSounds.Last().Volume = (float)Globals.sfxVolume;
                        shootSounds.Last().Play();
                    } else {
                        MainGame.newPlayerBullet((int)position.X, new Vector2(0, bulletSpeed), 1 + bulletArmour, 1);

                        shootSounds.Add(SFX.shootSounds[0].CreateInstance());
                        shootSounds.Last().Volume = (float)Globals.sfxVolume;
                        shootSounds.Last().Play();
                    }
                }
            }
        }

        internal void registerDamage(int damage) {
            SFX.hitSounds[0].Play();
            if (invincibility != 0) { return; }
            if (sheild != 0) {
                if (damage != 0) {
                    sheild--;
                }
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
