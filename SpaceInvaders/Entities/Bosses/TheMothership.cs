using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceInvaders.Entities.Bullets;
using SpaceInvaders.Scenes;
using SpaceInvaders.Utils;

namespace SpaceInvaders.Entities.Bosses {
    internal class TheMothership : BasicBoss {
        int bossSummonHealthBar;
        int bulletOffset = 0;
        int waveBulletOffsetRight = 0;
        int waveBulletOffsetLeft = 0;
        int handToShootFrom = 0;
        int bullet = 6;

        Vector2 rightHand = new(132, 50), leftHand = new(10, 50);

        public TheMothership(Texture2D sprite, int wave) {
            this.sprite = sprite;
            position = new(0,0);
            hitbox = new(0, 0, 160, 40);
            hitboxOffset = hitbox.Location;
            hitbox.Location = position.ToPoint() + hitboxOffset;
            center = position + hitbox.Size.ToVector2() * new Vector2(0.5f, 1);

            maxHealth = 250 * (wave + 1);
            health = maxHealth;
            bossSummonHealthBar = health - maxHealth / 10;

            attackTimerReset = Time.ToFrames(0, minutes: 6);
            attackTimer = attackTimerReset;
            sourceRect = new(0, 0, 48, 64);

            Globals.invasionMode = true;
        }

        internal override void Update(Vector2 playerPos) {
              /////////////////
             // Passive Atk // Wall of bullets fall down
            /////////////////
            if (attackTimer % Time.ToFrames(30) == 0) {
                MainGame.newEnemyBullet<Bullet>(new Vector2(0 + (16 *   bulletOffset), -8), new(0, 3.5f), bullet, bossBullet: true);
                MainGame.newEnemyBullet<Bullet>(new Vector2(32 + (16 *  bulletOffset), -8), new(0, 3.5f), bullet, bossBullet: true);
                MainGame.newEnemyBullet<Bullet>(new Vector2(64 + (16 *  bulletOffset), -8), new(0, 3.5f), bullet, bossBullet: true);
                MainGame.newEnemyBullet<Bullet>(new Vector2(96 + (16 *  bulletOffset), -8), new(0, 3.5f), bullet, bossBullet: true);
                MainGame.newEnemyBullet<Bullet>(new Vector2(128 + (16 * bulletOffset), -8), new(0, 3.5f), bullet, bossBullet: true);

                bulletOffset++;
                if (bulletOffset == 2) { bulletOffset = 0; }
            }

            ///////////////
            // Attack #1 // Wavey like shoot pattern
            ///////////////
            if (attackTimer < Time.ToFrames(50, minutes: 5) && attackTimer > Time.ToFrames(40, minutes:5) && attackTimer % 35 == 0) {
                MainGame.newEnemyBullet<Bullet>(rightHand, new(0 - 1 * waveBulletOffsetRight, 3.5f), bullet, bossBullet: true);

                waveBulletOffsetRight++;
                if (waveBulletOffsetRight == 5) {
                    waveBulletOffsetRight = 0;
                }
            }

            if (attackTimer < Time.ToFrames(55, minutes: 5) && attackTimer > Time.ToFrames(40, minutes: 5) && attackTimer % 20 == 0) {
                MainGame.newEnemyBullet<Bullet>(leftHand, new(0 + 0.5f * waveBulletOffsetLeft, 2.5f), bullet, bossBullet: true);

                waveBulletOffsetLeft++;
                if (waveBulletOffsetLeft == 7) {
                    waveBulletOffsetLeft = 0;
                }
            }

            ///////////////
            // Attack #2 // Shot bullet at random direction.
            ///////////////
            if (attackTimer < Time.ToFrames(38, minutes: 5) && attackTimer > Time.ToFrames(30, minutes: 5) && attackTimer % 20 == 0) {
                if (handToShootFrom == 0) {
                    MainGame.newEnemyBullet<Bullet>(leftHand, new(rng.Next(1,6) * (float)rng.NextDouble(), 2.5f), bullet, bossBullet: true);
                    handToShootFrom = 1;
                } else {
                    MainGame.newEnemyBullet<Bullet>(rightHand, new(rng.Next(-5, 0) * (float)rng.NextDouble(), 2.5f), bullet, bossBullet: true);
                    handToShootFrom = 0;
                }
            }

            ///////////////
            // Attack #3 // Lilith be like:
            ///////////////
            if (attackTimer <= Time.ToFrames(20, minutes: 5) && attackTimer >= Time.ToFrames(seconds: 30, minutes: 4) && attackTimer % 120 == 0) {
                for (int i = 0; i < 6; i++) {
                    MainGame.newEnemyBullet<Bullet>(
                        position: new(
                            -16 + 32 * i + 16 * bulletOffset,
                            Globals.screenHeight + 16),
                        direction: new(0, -0.45f),
                        bullet,
                        bossBullet: true);
                }

                bulletOffset++;
                if (bulletOffset == 2) { bulletOffset = 0; }
            }

            ///////////////
            // Attack #4 // Targeted bullets
            ///////////////
            if (attackTimer <= Time.ToFrames(0, minutes: 4) && attackTimer >= Time.ToFrames(seconds: 30, minutes: 3) && attackTimer % 40 == 0) {
                if (playerPos.X <= 72) {
                    MainGame.newEnemyBullet<Bullet>(
                        position: leftHand,
                        direction: new Vector2((playerPos.X - leftHand.X) / 30, (playerPos.Y + 8 - leftHand.Y) / 30),
                        bullet,
                        bossBullet: true);
                }
                if (playerPos.X >= 72) {
                    MainGame.newEnemyBullet<Bullet>(
                        position: rightHand,
                        direction: new((playerPos.X - rightHand.X) / 30, (playerPos.Y + 8 - rightHand.Y) / 30),
                        bullet,
                        bossBullet: true);
                }
            }

            if (attackTimer <= Time.ToFrames(30, minutes: 3) && attackTimer >= Time.ToFrames(seconds: 0, minutes: 3) && attackTimer % 30 == 0) {
                MainGame.newEnemyBullet<Bullet>(
                    position: leftHand,
                    direction: new Vector2((playerPos.X - leftHand.X) / 40, (playerPos.Y + 8 - leftHand.Y) / 40),
                    bullet,
                    bossBullet: true);
                MainGame.newEnemyBullet<Bullet>(
                    position: rightHand,
                    direction: new((playerPos.X - rightHand.X) / 40, (playerPos.Y + 8 - rightHand.Y) / 40),
                    bullet,
                    bossBullet: true);
            }

            ///////////////
            // Attack #5 // Hell
            ///////////////
            if (attackTimer < Time.ToFrames(55, minutes: 2) && attackTimer > Time.ToFrames(50, minutes: 1) && attackTimer % 40 == 0) {
                MainGame.newEnemyBullet<Bullet>(rightHand, new(0 - 1 * waveBulletOffsetRight, 3.5f), bullet, bossBullet: true);

                waveBulletOffsetRight++;
                if (waveBulletOffsetRight == 5) {
                    waveBulletOffsetRight = 0;
                }
            }

            if (attackTimer < Time.ToFrames(55, minutes: 2) && attackTimer > Time.ToFrames(50, minutes: 1) && attackTimer % 30 == 0) {
                MainGame.newEnemyBullet<Bullet>(leftHand, new(0 + 0.5f * waveBulletOffsetLeft, 2.5f), bullet, bossBullet: true);

                waveBulletOffsetLeft++;
                if (waveBulletOffsetLeft == 7) {
                    waveBulletOffsetLeft = 0;
                }
            }

            if (attackTimer <= Time.ToFrames(20, minutes: 2) && attackTimer >= Time.ToFrames(seconds: 30, minutes: 1) && attackTimer % 60 == 0) {
                MainGame.newEnemyBullet<Bullet>(
                    position: leftHand,
                    direction: new Vector2((playerPos.X - leftHand.X) / 40, (playerPos.Y + 8 - leftHand.Y) / 40),
                    bullet,
                    bossBullet: true);
                MainGame.newEnemyBullet<Bullet>(
                    position: rightHand,
                    direction: new((playerPos.X - rightHand.X) / 40, (playerPos.Y + 8 - rightHand.Y) / 40),
                    bullet,
                    bossBullet: true);
            }

            ///////////////
            // Attack #6 // Random hand shooting but *fast*
            ///////////////
            if (attackTimer < Time.ToFrames(50) && attackTimer > Time.ToFrames(20, minutes: 1) && attackTimer % 10 == 0) {
                if (handToShootFrom == 0) {
                    MainGame.newEnemyBullet<Bullet>(leftHand, new(rng.Next(1, 6) * (float)rng.NextDouble(), 2.5f), bullet, bossBullet: true);
                    handToShootFrom = 1;
                } else {
                    MainGame.newEnemyBullet<Bullet>(rightHand, new(rng.Next(-5, 0) * (float)rng.NextDouble(), 2.5f), bullet, bossBullet: true);
                    handToShootFrom = 0;
                }
            }


            attackTimer--;
            if (health < bossSummonHealthBar) {
                Globals.summonSecondBoss = true;
                bossSummonHealthBar -= maxHealth / 10;
            }
        }
        internal override void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(sprite, position, Color.White);
        }
    }
}
