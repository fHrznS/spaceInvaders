using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceInvaders.Scenes;
using System;

namespace SpaceInvaders.Entities.Bosses {
    internal class Zhyron : BasicBoss {
        Vector2 center;
        int fallAttackOffset = 0;
        Rectangle sourceRect = new(0, 0, 80, 32);
        int nextFrameTimer = 60;
        int onFrame = 0;

        public Zhyron(Texture2D sprite, int wave) {
            this.sprite = sprite;
            position = new(40, 5);
            center = new(position.X + 32, position.Y+24);
            hitbox = new(56,5,48,28);
            
            maxHealth = 6 * (wave + 1);
            health = maxHealth;
            attackTimerReset = 60 * 10;
            attackTimer = attackTimerReset;
        }

        internal override void Update() {
            attackTimer--;
            nextFrameTimer--;

            if (attackTimer < 540 && attackTimer > 480 && attackTimer % 10 == 0) {
                MainGame.newEnemyBullet(position: center,
                    direction: new((float)(rng.Next(-3,3)*rng.NextDouble()),2.5f), 1, bossBullet: true);
            }

            if (attackTimer == 350) {
                MainGame.newEnemyBullet(position: center, new(-1.75f,3), 1, bossBullet: true);
                MainGame.newEnemyBullet(position: center, new(-1,    3), 1, bossBullet: true);
                MainGame.newEnemyBullet(position: center, new(0,     3), 1, bossBullet: true);
                MainGame.newEnemyBullet(position: center, new(1,     3), 1, bossBullet: true);
                MainGame.newEnemyBullet(position: center, new(1.75f, 3), 1, bossBullet: true);
            }

            if (attackTimer <= 200 && attackTimer >= 80 && attackTimer % 30 == 0) {
                MainGame.newEnemyBullet(position: new(0 + 8 * fallAttackOffset,-8), new(0,4), 1, bossBullet: true);
                MainGame.newEnemyBullet(position: new(32 + 8 * fallAttackOffset,-8), new(0,4), 1, bossBullet: true);
                MainGame.newEnemyBullet(position: new(64 + 8 * fallAttackOffset, -8), new(0, 4), 1, bossBullet: true);
                MainGame.newEnemyBullet(position: new(96 + 8 * fallAttackOffset, -8), new(0, 4), 1, bossBullet: true);
                MainGame.newEnemyBullet(position: new(128 + 8 * fallAttackOffset, -8), new(0, 4), 1, bossBullet: true);
                MainGame.newEnemyBullet(position: new(160 + 8 * fallAttackOffset, -8), new(0, 4), 1, bossBullet: true);
                fallAttackOffset += 1;
            }

            if (attackTimer == 0) {
                attackTimer = attackTimerReset;
                fallAttackOffset = 0;
            }

            if (nextFrameTimer == 0) {
                nextFrameTimer = 60;
                onFrame++;
                if (onFrame == 4) { onFrame = 0; }
                sourceRect.Location = new Point(80 * onFrame, 0);
            }
        }


        internal override void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(sprite, position, sourceRect, Color.White);
        }
    }
}
