using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceInvaders.Scenes;
using SpaceInvaders.Utils;

namespace SpaceInvaders.Entities.Bosses {
    internal class AdamAndEve : BasicBoss {
        Vector2 AdamCenter, EveCenter;
        int adamBullet = 4,
            eveBullet = 5;
        int adamShootTimer = Time.ToFrames(20);
        int eveShootTimer = Time.ToFrames(15);

        int adamBulletXOffset = 0;

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
            
            AdamCenter = position + hitbox.Size.ToVector2() * new Vector2(0.5f, 1.1f);
            EveCenter =  adamHitbox.Location.ToVector2() + eveHitbox.Size.ToVector2() * new Vector2(1.2f, 1.1f);

            maxHealth = 6 * (wave + 1);
            health = maxHealth;
            adamHealth = maxHealth / 2;
            eveHealth = adamHealth;

            attackTimerReset = 60 * 60;
            attackTimer = attackTimerReset;
            sourceRect = new(0, 0, 80, 64);

            Globals.stopSpawn = true;
        }


        internal override void Update() {
            health = adamHealth + eveHealth;

            if (eveHealth > 0) {
                eveUpdate();
            } else {
                sourceRect.Location = new Point(0,128);
                eveHitbox.X = -1000;
            }
            if (adamHealth > 0) {
                adamUpdate();
            } else {
                adamHitbox.X = -1000;
                sourceRect.Location = new Point(0,64);
            }
        }

        private void adamUpdate() {
            adamShootTimer--;

            if (adamShootTimer % Time.ToFrames(5) == 0) {
                MainGame.newEnemyBullet(AdamCenter, new Vector2(-0.8f, 2), adamBullet, bossBullet: true);
                MainGame.newEnemyBullet(AdamCenter, new Vector2(-0.3f, 1.8f), adamBullet, bossBullet: true);
                MainGame.newEnemyBullet(AdamCenter, new Vector2(0.3f, 1.8f), adamBullet, bossBullet: true);
                MainGame.newEnemyBullet(AdamCenter, new Vector2(0.8f, 2), adamBullet, bossBullet: true);
                MainGame.newEnemyBullet(AdamCenter, new Vector2(1.5f, 2.2f), adamBullet, bossBullet: true);
            }

            if (adamShootTimer % Time.ToFrames(2) == 0) {
                MainGame.newEnemyBullet(AdamCenter, new(rng.Next(-1,3) * (float)rng.NextDouble(), 3), adamBullet, bossBullet: true);
            }

            if (adamShootTimer % Time.ToFrames(3) == 0) {
                MainGame.newEnemyBullet(new Vector2(0 + (16 * adamBulletXOffset), -8), new(0, 3.5f), adamBullet, bossBullet: true);
                MainGame.newEnemyBullet(new Vector2(32 + (16 * adamBulletXOffset), -8), new(0, 3.5f), adamBullet, bossBullet: true);
                MainGame.newEnemyBullet(new Vector2(64 + (16 * adamBulletXOffset), -8), new(0, 3.5f), adamBullet, bossBullet: true);
                MainGame.newEnemyBullet(new Vector2(96 + (16 * adamBulletXOffset), -8), new(0, 3.5f), adamBullet, bossBullet: true);
                MainGame.newEnemyBullet(new Vector2(128 + (16 * adamBulletXOffset), -8), new(0, 3.5f), adamBullet, bossBullet: true);
            
                if (adamBulletXOffset == 0) { adamBulletXOffset = 1; }
                else { adamBulletXOffset = 0; }
            }

            if (adamShootTimer == 0) {
                adamShootTimer = Time.ToFrames(20);
            }
        }
        private void eveUpdate() {
            eveShootTimer--;

            if (eveShootTimer % Time.ToFrames(4) == 0) {
                MainGame.newEnemyBullet(EveCenter, new(rng.Next(-4, 2) * (float)rng.NextDouble(), 3), eveBullet, bossBullet: true);
            }

            if (eveShootTimer < Time.ToFrames(10) && eveShootTimer % 30 == 0) {
                MainGame.newEnemyBullet(EveCenter, new(rng.Next(-5, 5) * (float)rng.NextDouble(), 4), eveBullet, bossBullet: true);
            }

            if (eveShootTimer == 60) {
                MainGame.newEnemyBullet(EveCenter, new(-1.5f, 2), eveBullet, bossBullet: true);
                MainGame.newEnemyBullet(EveCenter, new(-0.5f, 2), eveBullet, bossBullet: true);
                MainGame.newEnemyBullet(EveCenter, new(0.5f, 2), eveBullet, bossBullet: true);
                MainGame.newEnemyBullet(EveCenter, new(1.5f, 2), eveBullet, bossBullet: true);
            }

            if (eveShootTimer == 0) {
                eveShootTimer = Time.ToFrames(15);
            }
        }

        internal override void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(sprite, position, sourceRect, Color.White);
        }
    }
}
