using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace SpaceInvaders.Entities.Powerboxes {
    internal class BulletBox : Powerbox {
        public BulletBox(Vector2 position, Texture2D sprite, int fallTimer, int id) {
            this.id = id;
            this.position = position;
            fallTimerReset = fallTimer;
            this.fallTimer = fallTimerReset;
            this.sprite = sprite;

            hitbox = new(position.ToPoint(), new(16, 16));
        }

        internal override void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(sprite, position, frameSource, Color.White);
        }
    }
}
