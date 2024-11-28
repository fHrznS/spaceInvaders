using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceInvaders.Scenes;
using SpaceInvaders.Utils;

namespace SpaceInvaders.Entities.Bosses {
    internal class Seraphim : BasicBoss {
        public Seraphim(Texture2D sprite, int wave) {
            this.sprite = sprite;
            position = new(60,8);
            hitbox = new(4, 0, 32, 55);
            hitboxOffset = hitbox.Location;
            hitbox.Location = position.ToPoint() + hitboxOffset;

            maxHealth = 9 * (wave + 1);
            health = maxHealth;
            attackTimerReset = 60 * 35;
            attackTimer = attackTimerReset;
            sourceRect = new(0, 0, 40, 60);
        }

        internal override void Update() {
            attackTimer--;
            nextFrameTimer--;

            if (attackTimer <= 60 * 30 && attackTimer >= 60 * 25 && attackTimer % 120 == 0) {
                Globals.instantKillAttack = true;
                int emptySlot = rng.Next(0, 9);
                
                for (int i = 0; i < 9; i++) {
                    if (i == emptySlot) { continue; }
                    MainGame.newEnemyBullet(new(8+16*i, position.Y), new(0,1), 1, bossBullet: true, damage: 3);
                }
            } if (attackTimer < 60 * 22) {
                Globals.instantKillAttack = false;
            }

            if (attackTimer <= 60*15 && attackTimer >= 60 * 10 && attackTimer % 25 == 0) {
                MainGame.newEnemyBullet(position: new(80, 68),
                    direction: new((float)(rng.Next(-3, 3) * rng.NextDouble()), 1.5f), 1, bossBullet: true, damage: 3);
            }

            if (attackTimer <= 60 * 6) {
                Globals.instantKillAttack = true;
            }
            if (attackTimer <= 60 * 5 && attackTimer % 40 == 0) {
                MainGame.newEnemyBullet(position: new(-8, rng.Next(50,100)),
                    direction: new(1, rng.Next(1,3)), 1, bossBullet: true, damage: 3);
            }

            if (attackTimer == 0) {
                attackTimer = attackTimerReset;
                Globals.instantKillAttack = false;
            }

            if (nextFrameTimer == 0) {
                nextFrameTimer = 25;
                onFrame++;
                if (onFrame == 8) { onFrame = 0; }
                sourceRect.Location = new(40*onFrame,0);
            }
        }

        internal override void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(sprite, position, sourceRect, Color.White);
        }

    }
}
