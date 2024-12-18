﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceInvaders.Entities.Bullets;
using SpaceInvaders.Scenes;
using SpaceInvaders.Utils;

namespace SpaceInvaders.Entities.Bosses {
    internal class Seraphim : BasicBoss {
        public Seraphim(Texture2D sprite, int wave, int multID) {
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
            this.multID = multID;
        }

        internal override void Update(Vector2 playerPos) {
            attackTimer--;
            nextFrameTimer--;

            if (attackTimer <= Time.ToFrames(seconds: 30) && attackTimer >= Time.ToFrames(seconds: 25) && attackTimer % 120 == 0) {
                Globals.disableEnemyShooting = true;
                int emptySlot = rng.Next(0, 9);
                
                for (int i = 0; i < 9; i++) {
                    if (i == emptySlot) { continue; }
                    MainGame.newEnemyBullet<Bullet>(new(8+16*i, position.Y), new(0,1), 1, multID, bossBullet: true, damage: 3);
                }
            } if (attackTimer < Time.ToFrames(seconds: 22)) {
                Globals.disableEnemyShooting = false;
            }

            if (attackTimer <= Time.ToFrames(seconds: 15) && attackTimer >= Time.ToFrames(seconds: 10) && attackTimer % 25 == 0) {
                MainGame.newEnemyBullet<Bullet>(position: new(80, 68),
                    direction: new((float)(rng.Next(-3, 3) * rng.NextDouble()), 1.5f), 1, multID, bossBullet: true, damage: 3);
            }

            if (attackTimer <= Time.ToFrames(seconds: 6)) {
                Globals.disableEnemyShooting = true;
            }
            if (attackTimer <= Time.ToFrames(seconds: 5) && attackTimer % 40 == 0) {
                MainGame.newEnemyBullet<Bullet>(position: new(-8, rng.Next(50,100)),
                    direction: new(1, rng.Next(1,3)), 1, multID, bossBullet: true, damage: 3);
            }

            if (attackTimer == 0) {
                attackTimer = attackTimerReset;
                Globals.disableEnemyShooting = false;
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
