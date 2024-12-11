using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceInvaders.Entities.Bullets;
using SpaceInvaders.Entities.Powerboxes;
using SpaceInvaders.Scenes;
using SpaceInvaders.Utils;

namespace SpaceInvaders.Entities.Bosses {
    internal class Saigai : BasicBoss {
        int bulletType = 1;
        bool firstMove = true;
        Vector2 velocity = new(0.05f, 0);
        int emptySlot;

        int bulletOffset = 0;
        int waveBulletOffsetRight = 0;
        int waveBulletOffsetLeft = 0;

        Vector2 rightHand = new(132, -8), leftHand = new(10, -8);

        public Saigai(Texture2D sprite, int wave) {
            this.sprite = sprite;
            sourceRect = new(0, 0, 32, 64);
            position = new(64, 8);
            hitbox = new(0, 12, 31, 31);
            hitboxOffset = hitbox.Location;
            hitbox.Location = position.ToPoint() + hitboxOffset;
            center = position + hitbox.Size.ToVector2() * new Vector2(0.5f, 1);

            maxHealth = 50 * (wave + 1);
            health = maxHealth;
            attackTimerReset = Time.ToFrames(0, minutes: 5);
            attackTimer = attackTimerReset;
            direction.X = rng.Next(0,2) == 1 ? 1 : -1;
            
            emptySlot = rng.Next(0, 9);
        }

        internal override void Update(Vector2 playerPos) {
            // Fade in spawn animation
            if (spawnTimer > 1) {
                spawnTimer -= 0.3f;

                if (spawnTimer < 1) { spawnTimer = 1; }
            }

            ///// Attacks

            ///////////////
            // Attack #1 // Lilith 4 bullet hell but slow
            ///////////////
            if (attackTimer <= Time.ToFrames(seconds: 55, minutes: 4) && attackTimer >= Time.ToFrames(seconds: 40, minutes: 4) && attackTimer % 40 == 0) {
                MainGame.newEnemyBullet<Bullet>(center, new(1.2f * direction.X, 1.4f), bulletType, healBoss: true, bossBullet: true);
                MainGame.newEnemyBullet<Bullet>(center, new(0.35f * direction.X, 1.4f), bulletType, healBoss: true, bossBullet: true);

                MainGame.newEnemyBullet<Bullet>(center, new(1.2f * -direction.X, 1.4f), bulletType, healBoss: true, bossBullet: true);
                MainGame.newEnemyBullet<Bullet>(center, new(0.35f * -direction.X, 1.4f), bulletType, healBoss: true, bossBullet: true);
            }
            if (attackTimer <= Time.ToFrames(seconds: 45, minutes: 4) && attackTimer >= Time.ToFrames(seconds: 35, minutes: 4) && attackTimer % 30 == 0) {
                MainGame.newEnemyBullet<Bullet>(center, new(rng.Next(-2,3) * (float)rng.NextDouble() , 2), bulletType, healBoss: true, bossBullet: true);
            }
            
            if (attackTimer <= Time.ToFrames(seconds: 35, minutes: 4) && attackTimer >= Time.ToFrames(seconds: 30, minutes: 4) && attackTimer % 20 == 0) {
                MainGame.newEnemyBullet<Bullet>(center, new(rng.Next(-2, 3) * (float)rng.NextDouble(), 2), bulletType, healBoss: true, bossBullet: true);
            }

            ///////////////
            // Attack #2 // Hole in wall (Aka Seraphim hardmode)
            ///////////////
            if (attackTimer <= Time.ToFrames(seconds: 30, minutes: 4) && attackTimer >= Time.ToFrames(seconds: 10, minutes: 4) && attackTimer % 40 == 0 && attackTimer % (40 * 6) != 0) {
                Globals.disableEnemyShooting = true;
                
                if (emptySlot <= 3) {
                    emptySlot += rng.Next(
                    emptySlot == 0 ? 0 : -1,
                    emptySlot == 8 ? 0 : 3);
                }
                else if (emptySlot >= 5) {
                    emptySlot += rng.Next(
                    emptySlot == 0 ? 0 : -2,
                    emptySlot == 8 ? 0 : 2);
                } else {
                    emptySlot += rng.Next(
                        emptySlot == 0 ? 0 : -2,
                        emptySlot == 8 ? 0 : 3);
                }
                
                
                if (emptySlot < 0) { emptySlot = 0; }
                if (emptySlot > 8) { emptySlot = 8; }
                for (int i = 0; i < 9; i++) {
                    if (i == emptySlot) { continue; }
                    MainGame.newEnemyBullet<Bullet>(new(8 + 16 * i, -8), new(0, 1.75f), bulletType, bossBullet: true, healBoss: true, damage: 2);
                }
            } if (attackTimer == Time.ToFrames(20, minutes: 4)) {
                Globals.disableEnemyShooting = false;
            }

            ///////////////
            // Attack #3 // Mothership PTSD
            ///////////////
            if (attackTimer < Time.ToFrames(5, minutes: 4) && attackTimer > Time.ToFrames(40, minutes: 3) && attackTimer % 30 == 0) {
                MainGame.newEnemyBullet<Bullet>(
                    rightHand,
                    new(0 - 1 * waveBulletOffsetRight, 3.3f),
                    bulletType,
                    damage: 3, healBoss: true, bossBullet: true);

                waveBulletOffsetRight++;
                if (waveBulletOffsetRight == 5) {
                    waveBulletOffsetRight = 0;
                }
            }

            if (attackTimer < Time.ToFrames(10, minutes: 4) && attackTimer > Time.ToFrames(40, minutes: 3) && attackTimer % 30 == 0) {
                MainGame.newEnemyBullet<Bullet>(
                    leftHand,
                    new(0 + 0.5f * waveBulletOffsetLeft, 2.3f),
                    bulletType,
                    damage: 3, healBoss: true, bossBullet: true);

                waveBulletOffsetLeft++;
                if (waveBulletOffsetLeft == 7) {
                    waveBulletOffsetLeft = 0;
                }
            }

            if (attackTimer <= Time.ToFrames(0, minutes: 4) && attackTimer >= Time.ToFrames(seconds: 40, minutes: 3) && attackTimer % 60 == 0) {
                MainGame.newEnemyBullet<Bullet>(
                    position: leftHand,
                    direction: new Vector2((playerPos.X - leftHand.X) / 55, (playerPos.Y + 8 - leftHand.Y) / 55),
                    bulletType,
                    damage: 3,
                    healBoss: true,
                    bossBullet: true);
                MainGame.newEnemyBullet<Bullet>(
                    position: rightHand,
                    direction: new((playerPos.X - rightHand.X) / 55, (playerPos.Y + 8 - rightHand.Y) / 55),
                    bulletType,
                    damage: 3,
                    healBoss: true,
                    bossBullet: true);
            }

            ///////////////
            // Attack #4 // Side to side w/ targeted shooting
            ///////////////
            if (attackTimer <= Time.ToFrames(30, minutes: 3) && attackTimer >= Time.ToFrames(seconds: 0, minutes: 3) && attackTimer % 60 == 0) {
                Globals.disableEnemyShooting = true;
                for (int i = 0; i < 6; i++) {
                    MainGame.newEnemyBullet<Bullet>(
                        new Vector2(16 * i + (16*4 * bulletOffset), -8),
                        new(0, 3.2f),
                        bulletType,
                        healBoss: true, bossBullet: true, damage: 2);
                }

                bulletOffset++;
                if (bulletOffset == 2) { bulletOffset = 0; }
            }
            if (attackTimer <= Time.ToFrames(seconds: 20, minutes: 3) && attackTimer >= Time.ToFrames(seconds: 0, minutes: 3) && attackTimer % 30 == 0) {
                MainGame.newEnemyBullet<Bullet>(
                    center,
                    new((playerPos.X - center.X) / 50, (playerPos.Y - center.Y) / 50),
                    bulletType,
                    healBoss: true, bossBullet: true, damage: 3);
            }

            if (attackTimer == Time.ToFrames(seconds: 30, minutes: 2)) {
                Globals.disableEnemyShooting = false;
            }

            ///////////////
            // Attack #5 // Rain
            ///////////////
            if (attackTimer <= Time.ToFrames(seconds: 45, minutes: 2) && attackTimer >= Time.ToFrames(seconds: 35, minutes: 2) && attackTimer % 10 == 0) {
                int slot = rng.Next(0,10);
                Globals.disableEnemyShooting = true;
                MainGame.newEnemyBullet<Bullet>(
                    new Vector2(16 * slot, -8),
                    new(0, 3),
                    bulletType,
                    healBoss: true, bossBullet: true, damage: 3);
            }
            if (attackTimer == Time.ToFrames(seconds: 35, minutes: 2)) {
                Globals.disableEnemyShooting = false;
            }

            ///////////////
            // Attack #6 // Heal Hell
            ///////////////
            if (attackTimer <= Time.ToFrames(seconds: 30, minutes: 2) && attackTimer >= Time.ToFrames(seconds: 0, minutes: 2) && attackTimer % 80 == 0) {
                Globals.disableEnemyShooting = true;

                for (int i = 0; i < 9; i++) {
                    float speed = rng.Next(2, 4) * (float)rng.NextDouble();
                    if (speed <= 1.05f) { speed = 2 + (float)rng.NextDouble(); }
                    MainGame.newEnemyBullet<Bullet>(new(8 + 16 * i, -8), new(0, speed), bulletType, bossBullet: true, healBoss: true, damage: 0);
                }
            }
            if (attackTimer == Time.ToFrames(0, minutes: 2)) {
                Globals.disableEnemyShooting = false;
            }

            ///////////////
            // Attack #7 // Long boi
            ///////////////

            // Wall of bullets
            if (attackTimer <= Time.ToFrames(seconds: 50, minutes: 1) && attackTimer >= Time.ToFrames(seconds: 50, minutes: 0) && attackTimer % 90 == 0) {
                Globals.disableEnemyShooting = true;
                
                MainGame.newEnemyBullet<Bullet>(new Vector2(0 + (16 * bulletOffset), -8), new(0, 3f), bulletType, bossBullet: true, damage: 0);
                MainGame.newEnemyBullet<Bullet>(new Vector2(32 + (16 * bulletOffset), -8), new(0, 3f), bulletType, bossBullet: true, damage: 0);
                MainGame.newEnemyBullet<Bullet>(new Vector2(64 + (16 * bulletOffset), -8), new(0, 3f), bulletType, bossBullet: true, damage: 0);
                MainGame.newEnemyBullet<Bullet>(new Vector2(96 + (16 * bulletOffset), -8), new(0, 3f), bulletType, bossBullet: true, damage: 0);
                MainGame.newEnemyBullet<Bullet>(new Vector2(128 + (16 * bulletOffset), -8), new(0, 3f), bulletType, bossBullet: true, damage: 0);

                bulletOffset++;
                if (bulletOffset == 2) { bulletOffset = 0; }
            }

            // Homing bullets
            if (attackTimer <= Time.ToFrames(40, minutes: 1) && attackTimer >= Time.ToFrames(50) && attackTimer % 45 == 0) {
                MainGame.newEnemyBullet<BulletHoming>(center, new Vector2(0, 1.4f), bulletType, bossBullet: true, healBoss: true);
            }

            // Downwards bullet
            if (attackTimer <= Time.ToFrames(20, minutes: 1) && attackTimer >= Time.ToFrames(50) && attackTimer % 60 == 0) {
                MainGame.newEnemyBullet<Bullet>(center, new Vector2(0, 2f), bulletType, bossBullet: true, healBoss: true);
            }

            if (attackTimer <= Time.ToFrames(seconds: 0, minutes: 1) && attackTimer >= Time.ToFrames(seconds: 50) && attackTimer % 45 == 0) {
                int slot = rng.Next(0, 10);
                MainGame.newEnemyBullet<Bullet>(
                    new Vector2(16 * slot, -8),
                    new(0, 3),
                    bulletType,
                    bossBullet: true);
            }
            if (attackTimer == Time.ToFrames(seconds: 50)) {
                Globals.disableEnemyShooting = false;
            }

            ///////////////
            // Attack #7 // Long boi [END]
            ///////////////

            ///////////////
            // Attack #8 // Die or heal please :)
            ///////////////
            
            if (attackTimer <= Time.ToFrames(seconds: 40) && attackTimer % 15 == 0) {
                int slot = rng.Next(0, 10);
                Globals.disableEnemyShooting = true;
                MainGame.newEnemyBullet<Bullet>(
                    new Vector2(16 * slot, -8),
                    new(0, 3),
                    bulletType,
                    healBoss: true, bossBullet: true);
            }
            
            if (attackTimer <= Time.ToFrames(seconds: 30) && attackTimer % Time.ToFrames(3) == 0) {
                Globals.disableEnemyShooting = true;

                for (int i = 0; i < 9; i++) {
                    if (i % 2 == 0) {
                        float speed = 3 * (float)rng.NextDouble();
                        if (speed <= 1.2f) { speed += 0.2f; }
                        MainGame.newEnemyBullet<Bullet>(new(8 + 16 * i, -8), new(0, speed), bulletType, bossBullet: true, healBoss: true, damage: 0);
                    }
                }
            }

            if (attackTimer <= Time.ToFrames(10) && attackTimer % 80 == 0) {
                MainGame.newEnemyBullet<BulletHoming>(
                    position: center,
                    direction: new Vector2((playerPos.X - leftHand.X) / 55, (playerPos.Y + 8 - leftHand.Y) / 55),
                    bulletType,
                    healBoss: true,
                    bossBullet: true,
                    damage: 0);
            }

            if (attackTimer == Time.ToFrames(5)) {
                Globals.disableEnemyShooting = false;
            }

            ///////////////
            // Attack #8 // Die or heal please :) [END]
            ///////////////



            // Movement code (stolen Lilith code because I'm lazy)
            if (spawnTimer > 1) { return; }
            position += velocity * direction;

            if (direction.X > 0 && position.X > 125) {
                velocity.X = 0.05f;
                direction.X = -1;
            }

            if (direction.X < 0 && position.X < 5) {
                velocity.X = 0.05f;
                direction.X = 1;
            }

            if ((position.X < 90 - (firstMove ? 0 : 20) && direction.X > 0) || (position.X > 40 + (firstMove ? 0 : 20) && direction.X < 0)) {
                velocity.X *= 1.07f;
            } else {
                velocity.X *= 0.93f;
            }

            if (velocity.X < 0.05f) {
                direction *= -1;
                velocity.X = 0.1f;
                firstMove = false;
            }

            hitbox.Location = position.ToPoint() + hitboxOffset;
            center = position + hitbox.Size.ToVector2() * new Vector2(0.5f, 1);
            
            attackTimer--;
            if (attackTimer == 0) {
                attackTimer = attackTimerReset;
            }
        }

        internal override void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(sprite, position, sourceRect, Color.White * (1f / spawnTimer));
        }
    }
}
