using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceInvaders.Scenes;

namespace SpaceInvaders.Entities.Bullets {
    internal class BulletHoming : BasicBullet {
        public override void NewBullet(Vector2 position, Point hitboxSize, Vector2 direction, Texture2D sprite, int id, int damage, int multID, bool healBoss = false, bool evil = false) {
            this.sprite = sprite;
            this.position = new(position.X + 7, position.Y);

            this.direction = direction;
            hitbox = new((int)position.X + 7, (int)position.Y, hitboxSize.X, hitboxSize.Y);
            this.damage = damage;
            this.id = id;

            isEvil = evil;
            this.healBoss = healBoss;
            this.multID = multID;
        }
        internal override void Update(Vector2 playerPos) {
            direction.X = playerPos.X - position.X + 5;
            if (direction.X > 0.8f) {
                direction.X = 0.8f;
            }
            if (direction.X < -0.8f) {
                direction.X = -0.8f;
            }

            position += direction;
            hitbox.Location = position.ToPoint();

            // Do we delete?
            if (position.Y < -10 && !isEvil) {
                MainGame.deleteBullet(id, multID, evil: false);
            } else if ((position.Y > 230 || position.Y < -32) && isEvil) {
                MainGame.deleteBullet(id, multID, evil: true);
            }
        }
    }
}
