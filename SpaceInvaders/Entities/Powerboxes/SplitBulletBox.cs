﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceInvaders.Entities.Powerboxes {
    internal class SplitBulletBox : Powerbox {
        public SplitBulletBox(Vector2 position, Texture2D sprite, int fallTimer, int id) {
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
