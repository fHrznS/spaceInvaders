using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceInvaders.Entities.Powerboxes {
    internal abstract class Powerbox : BasicObject {
        internal Rectangle frameSource = new(0, 0, 16, 16);
        internal int frame = 0, // 6 Frames, 0-5
            nextFrameTimer = 30,
            nextFrameTimerReset = 30;

        abstract internal void Update();
        abstract internal void Draw(SpriteBatch spriteBatch);
    }
}
