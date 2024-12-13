using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceInvaders.Entities.Bullets;
using SpaceInvaders.Scenes;
using SpaceInvaders.Utils;

namespace SpaceInvaders.Entities.Bosses {
    internal class Zhyron : BasicBoss {
        int fallAttackOffset = 0;

        public Zhyron(Texture2D sprite, int wave, int multID) {
            this.sprite = sprite;
            position = new(40, 5);
            center = new(position.X + 32, position.Y+24);
            hitbox = new(56,5,48,28);
            this.multID = multID;
            
            maxHealth = 6 * (wave + 1);
            health = maxHealth;
            attackTimerReset = Time.ToFrames(seconds: 10);
            attackTimer = attackTimerReset;
        }

        internal override void Update(Vector2 playerPos) {
            attackTimer--;
            nextFrameTimer--;

            if (attackTimer < Time.ToFrames(seconds: 9) && attackTimer > Time.ToFrames(seconds: 8) && attackTimer % 10 == 0) {
                MainGame.newEnemyBullet<Bullet>(position: center,
                    direction: new((float)(rng.Next(-3,3)*rng.NextDouble()),2.5f), 0, multID, bossBullet: true);
            }

            if (attackTimer == 350) {
                MainGame.newEnemyBullet<Bullet>(position: center, new(-1.75f,3), 0, multID, bossBullet: true);
                MainGame.newEnemyBullet<Bullet>(position: center, new(-1,    3), 0, multID, bossBullet: true);
                MainGame.newEnemyBullet<Bullet>(position: center, new(0,     3), 0, multID, bossBullet: true);
                MainGame.newEnemyBullet<Bullet>(position: center, new(1,     3), 0, multID, bossBullet: true);
                MainGame.newEnemyBullet<Bullet>(position: center, new(1.75f, 3), 0, multID, bossBullet: true);
            }

            if (attackTimer <= 200 && attackTimer >= 80 && attackTimer % 30 == 0) {
                MainGame.newEnemyBullet<Bullet>(position: new(0 + 8 * fallAttackOffset,-8), new(0,4), 0, multID, bossBullet: true);
                MainGame.newEnemyBullet<Bullet>(position: new(32 + 8 * fallAttackOffset,-8), new(0,4), 0, multID, bossBullet: true);
                MainGame.newEnemyBullet<Bullet>(position: new(64 + 8 * fallAttackOffset, -8), new(0, 4), 0, multID, bossBullet: true);
                MainGame.newEnemyBullet<Bullet>(position: new(96 + 8 * fallAttackOffset, -8), new(0, 4), 0, multID, bossBullet: true);
                MainGame.newEnemyBullet<Bullet>(position: new(128 + 8 * fallAttackOffset, -8), new(0, 4), 0, multID, bossBullet: true);
                MainGame.newEnemyBullet<Bullet>(position: new(160 + 8 * fallAttackOffset, -8), new(0, 4), 0, multID, bossBullet: true);
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
