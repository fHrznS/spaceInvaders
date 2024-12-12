using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceInvaders.Entities.Bullets;
using SpaceInvaders.Scenes;
using SpaceInvaders.Utils;

namespace SpaceInvaders.Entities.Bosses {
    internal class Gabriel : BasicBoss {
        public Gabriel(Texture2D sprite, int wave) {
            this.sprite = sprite;
            position = new(56, 8);
            hitbox = new(11, 2, 26, 51);
            hitboxOffset = hitbox.Location;
            hitbox.Location = position.ToPoint() + hitboxOffset;
            center = position + hitbox.Size.ToVector2() * new Vector2(0.5f,1);

            maxHealth = 6 * (wave + 1);
            health = maxHealth;
            attackTimerReset = 60 * 19;
            attackTimer = attackTimerReset;
            sourceRect = new(0, 0, 48, 64);

            Globals.invasionMode = true;
        }

        internal override void Update(Vector2 playerPos) {
            attackTimer--;

            if (attackTimer <= Time.ToFrames(seconds: 17) && attackTimer >= Time.ToFrames(seconds: 14) && attackTimer % 20 == 0) {
                MainGame.newEnemyBullet<Bullet>(center, new Vector2(rng.Next(-2,2)*((float)rng.NextDouble()), 1), 2, multID, bossBullet:true);
            }

            if (attackTimer <= Time.ToFrames(seconds: 9) && attackTimer >= Time.ToFrames(seconds: 6) && attackTimer % 60 == 0) {
                MainGame.newEnemyBullet<Bullet>(center, new Vector2(-0.5f  * ((float)rng.NextDouble() + 0.2f), 1.5f), 2, multID, bossBullet: true);
                MainGame.newEnemyBullet<Bullet>(center, new Vector2(0.5f   * ((float)rng.NextDouble() + 0.2f), 1.5f), 2, multID, bossBullet: true);
            }

            if (attackTimer <= Time.ToFrames(seconds: 3) && attackTimer % 30 == 0) {
                MainGame.newEnemyBullet<Bullet>(center, new Vector2(rng.Next(-2, 2) * ((float)rng.NextDouble()), 2.5f), 2, multID, bossBullet: true);
            }

            if (attackTimer == 0) {
                attackTimer = attackTimerReset;
            }
        }

        internal override void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(sprite, position, sourceRect, Color.White);
        }
    }
}
