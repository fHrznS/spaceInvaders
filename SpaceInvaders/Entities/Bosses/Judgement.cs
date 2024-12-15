using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceInvaders.Entities.Bullets;
using SpaceInvaders.Scenes;
using SpaceInvaders.Utils;

namespace SpaceInvaders.Entities.Bosses {
    internal class Judgement : BasicBoss {
        int bulletOffset = 0;
        int variant = 0;
        int animTimer = Time.ToFrames(1);
        int bossSummonHealthBar;
        int bulletType = 9;
        Vector2 velocity = new(0.1f, 0);

        public Judgement(Texture2D sprite, int wave, int multID) {
            this.sprite = sprite;
            position = new(64, 8);
            hitbox = new(2, 5, 28, 23);
            hitboxOffset = hitbox.Location;
            hitbox.Location = position.ToPoint() + hitboxOffset;
            center = position + new Vector2(12, 30);

            maxHealth = 120 * (wave + 1);
            health = maxHealth;
            bossSummonHealthBar = maxHealth - maxHealth / 10;

            attackTimerReset = Time.ToFrames(0, minutes: 2);
            attackTimer = attackTimerReset + Time.ToFrames(10);
            sourceRect = new(0, 0, 32, 32);

            direction = new(rng.Next(0, 2) == 1 ? 1 : -1, 0);
            Globals.disableEnemyShooting = true;

            this.multID = multID;
        }

        internal override void Update(Vector2 playerPos) {
            Globals.disableEnemyShooting = true;

            ///////////////
            // Attack #1 // Lilith if she was difficult
            ///////////////
            if (attackTimer >= Time.ToFrames(50, minutes: 1) && attackTimer % 12 == 0  && attackTimer <= Time.ToFrames(0, minutes: 2)) {
                if (variant == 0) {
                    MainGame.newEnemyBullet<Bullet>(center, new(0, 1), bulletType, multID, bossBullet: true, damage: 3);
                } else if (variant == 1) {
                    MainGame.newEnemyBullet<Bullet>(center, new(-0.5f, 1), bulletType, multID, bossBullet: true, damage: 3);
                    MainGame.newEnemyBullet<Bullet>(center, new(0.5f, 1), bulletType, multID, bossBullet: true, damage: 3);
                } else if (variant == 2) {
                    MainGame.newEnemyBullet<Bullet>(center, new(0, 1), bulletType, multID, bossBullet: true, damage: 3);
                } else if (variant == 3) {
                    MainGame.newEnemyBullet<Bullet>(center, new(-0.75f, 1), bulletType, multID, bossBullet: true, damage: 3);
                    MainGame.newEnemyBullet<Bullet>(center, new(0, 1), bulletType, multID, bossBullet: true, damage: 3);
                    MainGame.newEnemyBullet<Bullet>(center, new(0.75f, 1), bulletType, multID, bossBullet: true, damage: 3);
                }

                variant++;

                if (variant == 4) {
                    variant = 0;
                }
            }

            ///////////////
            // Attack #2 // Fast walls
            ///////////////
            if (attackTimer <= Time.ToFrames(49, minutes: 1) && attackTimer >= Time.ToFrames(30, minutes: 1) && attackTimer % 30 == 0) {
                MainGame.newEnemyBullet<Bullet>(new Vector2(0 + (4 * bulletOffset),   -8), new(0, 2.5f), bulletType, multID, bossBullet: true);
                MainGame.newEnemyBullet<Bullet>(new Vector2(32 + (4 * bulletOffset),  -8), new(0, 2.5f), bulletType, multID, bossBullet: true);
                MainGame.newEnemyBullet<Bullet>(new Vector2(64 + (4 * bulletOffset),  -8), new(0, 2.5f), bulletType, multID, bossBullet: true);
                MainGame.newEnemyBullet<Bullet>(new Vector2(96 + (4 * bulletOffset),  -8), new(0, 2.5f), bulletType, multID, bossBullet: true);
                MainGame.newEnemyBullet<Bullet>(new Vector2(128 + (4 * bulletOffset), -8), new(0, 2.5f), bulletType, multID, bossBullet: true);

                bulletOffset++;

                if (bulletOffset == 5) {
                    bulletOffset = 0;
                }
            }

            ///////////////
            // Attack #3 // Bullet mixture hell
            ///////////////
            if (attackTimer <= Time.ToFrames(29, minutes: 1) && attackTimer >= Time.ToFrames(50, minutes: 0) && attackTimer % 30 == 0) {
                MainGame.newEnemyBullet<Bullet>(center, new(0, 2f), bulletType, multID, bossBullet: true, damage: 3);

                if (variant % 2 == 0) {
                    MainGame.newEnemyBullet<Bullet>(center, new(-0.6f, 1.4f), bulletType, multID, bossBullet: true, damage: 3);
                    MainGame.newEnemyBullet<Bullet>(center, new(0.6f, 1.4f), bulletType, multID, bossBullet: true, damage: 3);
                }

                if (variant % 5 == 0) {
                    MainGame.newEnemyBullet<Bullet>(new Vector2(0 + (8 * bulletOffset), -8), new(0, 1.5f), bulletType, multID, bossBullet: true);
                    MainGame.newEnemyBullet<Bullet>(new Vector2(32 + (8 * bulletOffset), -8), new(0, 1.5f), bulletType, multID, bossBullet: true);
                    MainGame.newEnemyBullet<Bullet>(new Vector2(64 + (8 * bulletOffset), -8), new(0, 1.5f), bulletType, multID, bossBullet: true);
                    MainGame.newEnemyBullet<Bullet>(new Vector2(96 + (8 * bulletOffset), -8), new(0, 1.5f), bulletType, multID, bossBullet: true);
                    MainGame.newEnemyBullet<Bullet>(new Vector2(128 + (8 * bulletOffset), -8), new(0, 1.5f), bulletType, multID, bossBullet: true);
                    bulletOffset++;
                    if (bulletOffset >= 4) {
                        bulletOffset = 0;
                    }
                }

                variant++;
                if (variant == 10) {
                    variant = 0;
                }
            }

            ///////////////
            // Attack #4 // Pay attention
            ///////////////

            if (attackTimer <= Time.ToFrames(50, minutes: 0) && attackTimer >= Time.ToFrames(30, minutes: 0) && attackTimer % 40 == 0) {
                if (variant == 0) {
                    MainGame.newEnemyBullet<Bullet>(center, new(0, 1), bulletType, multID, bossBullet: true, damage: 3);
                } else if (variant == 1) {
                    MainGame.newEnemyBullet<Bullet>(center, new(-0.5f, 1), bulletType, multID, bossBullet: true, damage: 3);
                    MainGame.newEnemyBullet<Bullet>(center, new(0.5f, 1), bulletType, multID, bossBullet: true, damage: 3);
                } else if (variant == 3) {
                    MainGame.newEnemyBullet<BulletHoming>(center, new(0, 1), bulletType, multID, bossBullet: true);
                }

                variant++;

                if (variant == 4) {
                    variant = 0;
                }
            }

            ///////////////
            // Attack #5 // Break attack
            ///////////////
            if (attackTimer == Time.ToFrames(30, minutes: 0)) {
                bulletOffset = 0;
            }
            if (attackTimer <= Time.ToFrames(30, minutes: 0) && attackTimer >= Time.ToFrames(20, minutes: 0) && attackTimer % 60 == 0) {
                MainGame.newEnemyBullet<Bullet>(new Vector2(0 + (16 * bulletOffset), -8), new(0, 1.2f), bulletType, multID, bossBullet: true);
                MainGame.newEnemyBullet<Bullet>(new Vector2(32 + (16 * bulletOffset), -8), new(0, 1.2f), bulletType, multID, bossBullet: true);
                MainGame.newEnemyBullet<Bullet>(new Vector2(64 + (16 * bulletOffset), -8), new(0, 1.2f), bulletType, multID, bossBullet: true);
                MainGame.newEnemyBullet<Bullet>(new Vector2(96 + (16 * bulletOffset), -8), new(0, 1.2f), bulletType, multID, bossBullet: true);
                MainGame.newEnemyBullet<Bullet>(new Vector2(128 + (16 * bulletOffset), -8), new(0, 1.2f), bulletType, multID, bossBullet: true);

                bulletOffset++;

                if (bulletOffset == 2) {
                    bulletOffset = 0;
                }
            }

            ///////////////
            // Attack #5 // Target practice
            ///////////////
            if (attackTimer <= Time.ToFrames(20) && attackTimer % 20 == 0) {
                if (variant == 0) {
                    MainGame.newEnemyBullet<Bullet>(center, new(-0.5f, 1), bulletType, multID, bossBullet: true, damage: 3);
                    MainGame.newEnemyBullet<Bullet>(center, new(0.5f, 1), bulletType, multID, bossBullet: true, damage: 3);
                } else if (variant == 1) {
                    MainGame.newEnemyBullet<Bullet>(center, new(playerPos.X - position.X / 60, playerPos.Y - position.Y / 60), bulletType, multID, bossBullet: true, damage: 3);
                } else if (variant == 3) {
                    MainGame.newEnemyBullet<Bullet>(center, new(-0.75f, 1), bulletType, multID, bossBullet: true, damage: 3);
                    MainGame.newEnemyBullet<Bullet>(center, new(playerPos.X - position.X / 40, playerPos.Y - position.Y / 40), bulletType, multID, bossBullet: true, damage: 3);
                    MainGame.newEnemyBullet<Bullet>(center, new(0.75f, 1), bulletType, multID, bossBullet: true, damage: 3);
                }

                variant++;

                if (variant == 4) {
                    variant = 0;
                }
            }

            // Update attack timer
            attackTimer--;

            if (attackTimer == 0) {
                attackTimer = attackTimerReset;
                variant = 0;
                bulletOffset = 0;
            }

            // Check if we should summon another boss.
            if (health < bossSummonHealthBar) {
                Globals.summonSecondBoss = true;
                bossSummonHealthBar -= maxHealth / 10;
            }

            // Update animation timer
            animTimer--;

            if (animTimer == 0) {
                animTimer = Time.ToFrames(1);
                sourceRect.X += 32;
                if (sourceRect.X == 224) { // 7 Frames
                    sourceRect.X = 0;
                }
            }

            // Movement code
            if (direction.X > 0 && position.X > 120) {
                velocity.X = 0.1f;
                direction.X = -1;
            }

            if (direction.X < 0 && position.X < 5) {
                velocity.X = 0.1f;
                direction.X = 1;
            }

            if ((position.X < 120 && direction.X > 0) || (position.X > 5 && direction.X < 0)) {
                velocity.X *= 1.02f;
            } else {
                velocity.X *= 0.9f;
            }

            if (velocity.X < 0.05f) {
                direction *= -1;
                velocity.X = 0.1f;
            }

            position += velocity * direction;

            hitbox.Location = position.ToPoint() + hitboxOffset;
            center = position + new Vector2(12, 30);

        }

        internal override void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(sprite, position, sourceRect, Color.White);
        }
    }
}
