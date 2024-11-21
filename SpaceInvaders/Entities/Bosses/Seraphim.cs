using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceInvaders.Scenes;

namespace SpaceInvaders.Entities.Bosses {
    internal class Seraphim : BasicBoss {
        public Seraphim(Texture2D sprite, int wave) {
            this.sprite = sprite;
            position = new(60,8);
            hitbox = new(4, 0, 32, 55);
            hitboxOffset = hitbox.Location;
            hitbox.Location = position.ToPoint();

            maxHealth = 10 * (wave + 1);
            health = maxHealth;
            attackTimerReset = 60 * 30;
            attackTimer = attackTimerReset;
        }

        internal override void Update() {
            attackTimer--;

            if (attackTimer == 60 * 25) {
                MainGame.newEnemyBullet(new(position.X, position.Y), new(0,1), "2", bossBullet: true, damage: 3);
            }

            if (attackTimer == 0) {
                attackTimer = attackTimerReset;
            }
        }

        internal override void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(sprite, position, Color.White);
        }

    }
}
