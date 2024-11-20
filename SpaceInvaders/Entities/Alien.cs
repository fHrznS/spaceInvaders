using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace SpaceInvaders.Entities {
    internal abstract class Alien : BasicObject {
        internal Random rng = new();
        internal int type;
        internal bool free;
        internal int shootTimer, shootTimerReset;
        internal int timer, timerReset = 60 * 2;
        public int gridPosition = 0;

        internal bool isImmune = false;
        internal Point hitboxOffset;

        public static int count = 0;

        internal abstract void Update(GameTime gameTime);

        internal abstract void Draw(SpriteBatch spriteBatch);

        internal string getTextureName() {
            return "AlienSprites/type" + type.ToString();
        }
    }
}
