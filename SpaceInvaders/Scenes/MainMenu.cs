using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SpaceInvaders.Utils;

namespace SpaceInvaders.Scenes
{
    internal class MainMenu : IScene
    {
        public enum Options {
            Start,
            Settings,
            HowTo
        }
        
        private ContentManager Content;
        
        public static Options selectedOption = Options.Start;
        private KeyboardState previousInput;

        SpriteFont text;

        public MainMenu(ContentManager contentManager) {
            Content = contentManager;
        }

        void IScene.LoadContent() {
            text = Content.Load<SpriteFont>("Font");
        }

        void IScene.Update(GameTime gameTime) {
            KeyboardState input = Keyboard.GetState();

            if (input == previousInput) { return; }

            if (input.IsKeyDown(Keys.Down)) {
                if (selectedOption == Options.Start) {
                    selectedOption = Options.Settings;
                } else if (selectedOption == Options.Settings) {
                    selectedOption = Options.HowTo;
                }
            }

            if (input.IsKeyDown(Keys.Up)) {
                if (selectedOption == Options.Settings) {
                    selectedOption = Options.Start;
                } else if (selectedOption == Options.HowTo) {
                    selectedOption = Options.Settings;
                }
            }

            previousInput = input;
        }

        void IScene.Draw(SpriteBatch spriteBatch) {
            spriteBatch.DrawString(text, selectedOption == Options.Start ? "* Play" : "Play", new Vector2(2,0), Color.White);
            spriteBatch.DrawString(text, selectedOption == Options.Settings ? "* Settings" : "Settings", new Vector2(2,24), Color.White);
            spriteBatch.DrawString(text, selectedOption == Options.HowTo ? "* Help" : "Help", new Vector2(2,48), Color.White);
        }

        void IScene.HighResDraw(SpriteBatch spriteBatch) {
            spriteBatch.DrawString(text,"Press Z to confirm choice", new Vector2(5, 742), Color.White);
        }
    }
}
