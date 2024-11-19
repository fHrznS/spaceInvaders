using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SpaceInvaders.Utils;

namespace SpaceInvaders.Scenes
{
    internal class MainMenu : IScene
    {
        private ContentManager Content;

        SpriteFont text;

        public MainMenu(ContentManager contentManager) {
            Content = contentManager;
        }

        void IScene.LoadContent() {
            text = Content.Load<SpriteFont>("Font");
        }

        void IScene.Update(GameTime gameTime) { }

        void IScene.Draw(SpriteBatch spriteBatch) {
            spriteBatch.DrawString(text, "Press\nZ", new Vector2(2,0), Color.White);
        }

        void IScene.HighResDraw(SpriteBatch spriteBatch) {
        }
    }
}
