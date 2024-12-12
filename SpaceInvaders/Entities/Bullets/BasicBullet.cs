using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceInvaders.Entities {
    internal abstract class BasicBullet : BasicObject {
        //public static int bulletCount = 0;
        internal int damage;
        internal bool isEvil;
        internal bool healBoss;
        public abstract void NewBullet(Vector2 position, Point hitboxSize, Vector2 direction, Texture2D sprite, int id, int damage, int multID, bool healBoss = false, bool evil = false);

        internal abstract void Update(Vector2 playerPos);
        internal void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(sprite, position, Color.White);
        }
    }
}
