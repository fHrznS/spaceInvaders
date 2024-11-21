using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using SpaceInvaders.Scenes;

namespace SpaceInvaders.Entities {
    internal class Bullet : BasicObject {
        public static int bulletCount = 0;
        internal int damage;
        internal bool isEvil;

        public Bullet(Vector2 position, Point hitboxSize, Vector2 direction, Texture2D sprite, int id, int damage, bool evil = false) {
            this.sprite = sprite;
            this.position = new(position.X + 7, position.Y);
            
            this.direction = direction;
            hitbox = new((int)position.X + 7, (int)position.Y, hitboxSize.X, hitboxSize.Y);
            this.damage = damage;
            this.id = id;
            
            isEvil = evil;
        }

        public void Update() {
            position.Y += direction.Y;
            position.X += direction.X;
            hitbox.Location = position.ToPoint();

            // Do we delete?
            if (position.Y < -10 && !isEvil) {
                MainGame.deleteBullet(id, evil: false);
            } else if (position.Y > 200 && isEvil) {
                MainGame.deleteBullet(id, evil: true);
            }
        }

        public void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(sprite, position, Color.White);
        }
    }
}
