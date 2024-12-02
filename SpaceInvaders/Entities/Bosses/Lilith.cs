using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceInvaders.Scenes;
using SpaceInvaders.Utils;

namespace SpaceInvaders.Entities.Bosses {
    internal class Lilith : BasicBoss {
        bool firstMove = true;
        int offsetBulltet = 0;
        int bulletType = 3;
        Vector2 velocity = new(0.1f, 0);

        public Lilith(Texture2D sprite, int wave) {
            this.sprite = sprite;
            position = new(64, 8);
            hitbox = new(7, 0, 18, 23);
            hitboxOffset = hitbox.Location;
            hitbox.Location = position.ToPoint() + hitboxOffset;
            center = position + hitbox.Size.ToVector2() * new Vector2(0.5f, 1);
            
            maxHealth = 2 * (wave + 1);
            health = maxHealth;
            attackTimerReset = 60 * 60;
            attackTimer = attackTimerReset;
            sourceRect = new(0, 0, 64, 64);

            direction = new(rng.Next(0,2) == 1 ? 1 : -1 , 0);

            Globals.stopSpawn = true;
        }

        internal override void Update() {
            // Attack
            attackTimer--;
            if (attackTimer <= Time.ToFrames(seconds: 58) && attackTimer >= Time.ToFrames(seconds: 51) && attackTimer % 80 == 0) {
                for (int i = 0; i < 6; i++) {
                    MainGame.newEnemyBullet(
                        position: new(
                            -16 + 32 * i + 16 * offsetBulltet,
                            Globals.screenHeight + 16),
                        direction: new(0, -0.45f),
                        bulletType,
                        bossBullet: true);
                }

                offsetBulltet++;
                if (offsetBulltet == 2) { offsetBulltet = 0; }
            }

            if (attackTimer <= Time.ToFrames(seconds: 54) && attackTimer >= Time.ToFrames(seconds: 51) && attackTimer % 30 == 0) {
                MainGame.newEnemyBullet(center, new(rng.Next(-2,3) * (float)rng.NextDouble() , 2), bulletType, bossBullet: true);
            }

            if (attackTimer <= Time.ToFrames(seconds: 47) && attackTimer >= Time.ToFrames(seconds: 42) && attackTimer % 15 == 0) {
                MainGame.newEnemyBullet(center, new(1.2f * direction.X, 1.4f), bulletType, bossBullet: true);
                MainGame.newEnemyBullet(center, new(0.35f * direction.X, 1.4f), bulletType, bossBullet: true);


                MainGame.newEnemyBullet(center, new(1.2f * -direction.X, 1.4f), bulletType, bossBullet: true);
                MainGame.newEnemyBullet(center, new(0.35f * -direction.X, 1.4f), bulletType, bossBullet: true);
            }

            if (attackTimer <= Time.ToFrames(seconds: 40) && attackTimer >= Time.ToFrames(seconds: 35) && attackTimer % 10 == 0) {
                MainGame.newEnemyBullet(center,
                    new(rng.Next(-4,4) * (float)rng.NextDouble(), 2.7f),
                    bulletType, bossBullet: true);
            }

            if (attackTimer <= Time.ToFrames(seconds: 38) && attackTimer >= Time.ToFrames(seconds: 20) && attackTimer % 60 == 0) {
                MainGame.newEnemyBullet(center,
                    new(0, 2f),
                    bulletType, bossBullet: true);
                MainGame.newEnemyBullet(center,
                    new(-1.5f, 2f),
                    bulletType, bossBullet: true);
                MainGame.newEnemyBullet(center,
                    new(1.5f, 2f),
                    bulletType, bossBullet: true);
            }

            if (attackTimer <= Time.ToFrames(seconds: 32) && attackTimer >= Time.ToFrames(seconds: 20) && attackTimer % 10 == 0) {
                MainGame.newEnemyBullet(center, new(0.4f * direction.X, 1), bulletType, bossBullet: true);
                MainGame.newEnemyBullet(center, new(0.4f * -direction.X, 1), bulletType, bossBullet: true);
            }

            if (attackTimer <= Time.ToFrames(seconds: 18) && attackTimer >= Time.ToFrames(seconds: 10) && attackTimer % 20 == 0) {
                MainGame.newEnemyBullet(center, new(0.4f * direction.X, 1), bulletType, bossBullet: true);
                MainGame.newEnemyBullet(center, new(0.4f * -direction.X, 1), bulletType, bossBullet: true);
                MainGame.newEnemyBullet(center, new(rng.Next(-2, 3) * (float)rng.NextDouble(), 2), bulletType, bossBullet: true);
            }

            if (attackTimer < Time.ToFrames(seconds: 7) && attackTimer % 30 == 0) {

                MainGame.newEnemyBullet(center, new(1.2f * direction.X, 1.4f), bulletType, bossBullet: true);
                MainGame.newEnemyBullet(center, new(0.35f * direction.X, 1.4f), bulletType, bossBullet: true);

                MainGame.newEnemyBullet(center, new(0, 1.4f), bulletType, bossBullet: true);

                MainGame.newEnemyBullet(center, new(1.2f * -direction.X, 1.4f), bulletType, bossBullet: true);
                MainGame.newEnemyBullet(center, new(0.35f * -direction.X, 1.4f), bulletType, bossBullet: true);
            }

            if (attackTimer == 0) {
                attackTimer = attackTimerReset;
            }


            // Movement
            position += velocity * direction;

            if (direction.X > 0 && position.X > 120) {
                velocity.X = 0.1f;
                direction.X = -1;
            }

            if (direction.X < 0 && position.X < 10) {
                velocity.X = 0.1f;
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
        }
        internal override void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(sprite, position, sourceRect, Color.White);
        }
    }
}
