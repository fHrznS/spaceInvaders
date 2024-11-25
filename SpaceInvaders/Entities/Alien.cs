using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace SpaceInvaders.Entities {
    /// <summary>
    /// This is the ground class for an Alien.
    /// It contains all variables an alien needs to fully function.
    /// Additionally it has an Alien count. (to be deleted due to redundancy with <List>.Count)
    /// </summary>

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
