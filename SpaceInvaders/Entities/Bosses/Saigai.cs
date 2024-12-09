using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceInvaders.Scenes;
using SpaceInvaders.Utils;

namespace SpaceInvaders.Entities.Bosses {
    internal class Saigai : BasicBoss {
        int bulletType = 1;
        bool firstMove = true;
        Vector2 velocity = new(0.05f, 0);
        int emptySlot;

        public Saigai(Texture2D sprite, int wave) {
            this.sprite = sprite;
            sourceRect = new(0, 0, 32, 64);
            position = new(64, 8);
            hitbox = new(0, 12, 31, 31);
            hitboxOffset = hitbox.Location;
            hitbox.Location = position.ToPoint() + hitboxOffset;
            center = position + hitbox.Size.ToVector2() * new Vector2(0.5f, 1);

            maxHealth = 300 * (wave + 1);
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
                MainGame.newEnemyBullet(center, new(1.2f * direction.X, 1.4f), bulletType, bossBullet: true);
                MainGame.newEnemyBullet(center, new(0.35f * direction.X, 1.4f), bulletType, bossBullet: true);

                MainGame.newEnemyBullet(center, new(1.2f * -direction.X, 1.4f), bulletType, bossBullet: true);
                MainGame.newEnemyBullet(center, new(0.35f * -direction.X, 1.4f), bulletType, bossBullet: true);
            }
            if (attackTimer <= Time.ToFrames(seconds: 45, minutes: 4) && attackTimer >= Time.ToFrames(seconds: 35, minutes: 4) && attackTimer % 30 == 0) {
                MainGame.newEnemyBullet(center, new(rng.Next(-2,3) * (float)rng.NextDouble() , 2), bulletType, bossBullet: true);
            }
            
            if (attackTimer <= Time.ToFrames(seconds: 35, minutes: 4) && attackTimer >= Time.ToFrames(seconds: 25, minutes: 4) && attackTimer % 20 == 0) {
                MainGame.newEnemyBullet(center, new(rng.Next(-2, 3) * (float)rng.NextDouble(), 2), bulletType, bossBullet: true);
            }

            ///////////////
            // Attack #2 // Hole in wall (Aka Seraphim hardmode)
            ///////////////
            if (attackTimer <= Time.ToFrames(seconds: 30, minutes: 4) && attackTimer >= Time.ToFrames(seconds: 45, minutes: 3) && attackTimer % 40 == 0) {
                Globals.disableEnemyShooting = true;
                emptySlot += rng.Next(
                    emptySlot == 0 ? 0 : -2,
                    emptySlot == 8 ? 0 : 3);
                if (emptySlot < 0) { emptySlot = 0; }
                if (emptySlot > 8) { emptySlot = 8; }
                for (int i = 0; i < 9; i++) {
                    if (i == emptySlot) { continue; }
                    MainGame.newEnemyBullet(new(8 + 16 * i, -8), new(0, 1.75f), bulletType, bossBullet: true, damage: 2);
                }
            } if (attackTimer == Time.ToFrames(3, minutes: 40)) {
                Globals.disableEnemyShooting = false;
            }

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
