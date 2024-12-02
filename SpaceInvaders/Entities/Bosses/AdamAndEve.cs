using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceInvaders.Entities.Bosses {
    internal class AdamAndEve : BasicBoss {
        Vector2 AdamCenter, EveCenter;
        int AdamBullet = 4,
            EveBullet = 5;

        public AdamAndEve(Texture2D sprite, int wave) {
            this.sprite = sprite;
            position = new(40, 8);
            hitbox = new(8, 4, 28, 45);
            
            hitboxOffset = hitbox.Location;
            
            adamHitbox = hitbox;
            adamHitbox.Location = position.ToPoint() + hitboxOffset;
            eveHitbox = adamHitbox;
            eveHitbox.X += adamHitbox.Width + 5;
            eveHitbox.Width -= 3;
            
            AdamCenter = position + hitbox.Size.ToVector2() * new Vector2(0.2f, 1.1f);
            EveCenter = position + hitbox.Size.ToVector2() * new Vector2(0.6f, 1.1f);

            maxHealth = 10 * (wave + 1);
            health = maxHealth;
            adamHealth = maxHealth / 2;
            eveHealth = adamHealth;

            attackTimerReset = 60 * 60;
            attackTimer = attackTimerReset;
            sourceRect = new(0, 0, 64, 64);

            // direction = new(rng.Next(0, 2) == 1 ? 1 : -1, 0);
        }


        internal override void Update() {
            health = adamHealth + eveHealth;

            if (eveHealth > 0) {
                eveUpdate();
            }
            if (adamHealth > 0) {
                adamUpdate();
            }
        }

        private void adamUpdate() {}
        private void eveUpdate() {}

        internal override void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(sprite, position, Color.White);
        }
    }
}
