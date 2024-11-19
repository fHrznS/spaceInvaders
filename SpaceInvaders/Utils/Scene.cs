using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceInvaders.Utils {
    internal interface IScene {
        public void LoadContent();
        
        public void Update(GameTime gameTime);

        public void Draw(SpriteBatch spriteBatch);
        public void HighResDraw(SpriteBatch spriteBatch);
    }
}
