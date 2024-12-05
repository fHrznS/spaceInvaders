using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceInvaders.Utils;

namespace SpaceInvaders.Entities.Bosses {
    internal class Saigai : BasicBoss {
        public Saigai(Texture2D sprite, int wave) {
            this.sprite = sprite;
            position = new(40, 5);
            center = new(position.X + 32, position.Y + 24);
            hitbox = new(56, 5, 48, 28);

            maxHealth = 5 * (wave + 1);
            health = maxHealth;
            attackTimerReset = Time.ToFrames(0, minutes: 5);
            attackTimer = attackTimerReset;
        }

        internal override void Update(Vector2 playerPos) {
            if (spawnTimer > 1) {
                spawnTimer -= 0.3f;

                if (spawnTimer < 1) { spawnTimer = 1; }
            }
        }

        internal override void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(sprite, position, sourceRect, Color.White * (1f / spawnTimer));
        }
    }
}
