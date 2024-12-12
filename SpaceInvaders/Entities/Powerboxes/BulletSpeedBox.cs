﻿using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace SpaceInvaders.Entities.Powerboxes {
    internal class BulletSpeedBox : Powerbox {
        public BulletSpeedBox(Vector2 position, Texture2D sprite, int fallTimer, int id, int multID) {
            this.id = id;
            this.position = position;
            fallTimerReset = fallTimer;
            this.fallTimer = fallTimerReset;
            this.sprite = sprite;
            this.multID = multID;

            hitbox = new(position.ToPoint(), new(16, 16));
        }
        internal override void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(sprite, position, frameSource, Color.White);
        }
    }
}
