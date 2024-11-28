using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceInvaders.Scenes;
using SpaceInvaders.Utils;

namespace SpaceInvaders.Entities.Bosses {
    internal class Gabriel : BasicBoss {
        Vector2 center;

        public Gabriel(Texture2D sprite, int wave) {
            this.sprite = sprite;
            position = new(56, 8);
            hitbox = new(11, 0, 26, 51);
            hitboxOffset = hitbox.Location;
            hitbox.Location = position.ToPoint() + hitboxOffset;
            center = position - hitbox.Size.ToVector2() * new Vector2(0.5f,0);

            maxHealth = 7 * (wave + 1);
            health = maxHealth;
            attackTimerReset = 60 * 15;
            attackTimer = attackTimerReset;
            sourceRect = new(0, 0, 48, 64);

            Globals.invasionMode = true;
        }

        internal override void Update() {
            attackTimer--;

            if (attackTimer <= 60 * 13 && attackTimer >= 60 * 10 && attackTimer % 20 == 0) {
                MainGame.newEnemyBullet(center, new Vector2(rng.Next(-2,2)*((float)rng.NextDouble()), 1), 2, bossBullet:true);
            }

            if (attackTimer <= 60 * 7 && attackTimer >= 60 * 5 && attackTimer % 20 == 0) {
                MainGame.newEnemyBullet(center, new Vector2(-0.5f  * ((float)rng.NextDouble()), 1.5f), 2, bossBullet: true);
                MainGame.newEnemyBullet(center, new Vector2(-0.75f * ((float)rng.NextDouble()), 1), 2, bossBullet: true);
                MainGame.newEnemyBullet(center, new Vector2(0.5f   * ((float)rng.NextDouble()), 1.5f), 2, bossBullet: true);
                MainGame.newEnemyBullet(center, new Vector2(0.75f  * ((float)rng.NextDouble()), 1), 2, bossBullet: true);
            }

            if (attackTimer <= 60 * 3 && attackTimer % 10 == 0) {
                MainGame.newEnemyBullet(center, new Vector2(rng.Next(-2, 2) * ((float)rng.NextDouble()), 0.5f), 2, bossBullet: true);
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
