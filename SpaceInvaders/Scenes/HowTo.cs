using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SpaceInvaders.Utils;

namespace SpaceInvaders.Scenes {
    internal class HowTo : IScene {
        ContentManager Content;

        SpriteFont text;

        public HowTo(ContentManager contentManager) {
            Content = contentManager;
        }

        public void LoadContent() {
            text = Content.Load<SpriteFont>("Font");
        }

        public void Update(GameTime gameTime) {
        }
        public void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(Sprites.powerboxes[0], new Vector2(50, 40), new(0, 0, 16, 16), Color.White);
            spriteBatch.Draw(Sprites.powerboxes[1], new Vector2(50, 56), new(0, 0, 16, 16), Color.White);
            spriteBatch.Draw(Sprites.powerboxes[2], new Vector2(50, 72), new(0, 0, 16, 16), Color.White);
            spriteBatch.Draw(Sprites.powerboxes[3], new Vector2(50, 88), new(0, 0, 16, 16), Color.White);
            spriteBatch.Draw(Sprites.powerboxes[4], new Vector2(50, 103), new(0, 0, 16, 16), Color.White);
            spriteBatch.Draw(Sprites.powerboxes[5], new Vector2(50, 119), new(0, 0, 16, 16), Color.White);
        }

        public void HighResDraw(SpriteBatch spriteBatch) {
            spriteBatch.DrawString(text, "Controls:\nMove Left: " + Controls.P1moveLeft +
                "\nMove Right: " + Controls.P1moveRight +
                "\nShoot: " + Controls.P1shoot +
                "\nPause: " + Controls.P1pause, new(4,4), Color.White);
            spriteBatch.DrawString(text, "Powerboxes:\n\n- Heal\n\n\n- More Bullets\n\n\n- Faster Bullets\n\n\n- Split Bullets\n\n\n- Sheild\n\n\n- More Damage\non Bosses", new(0, 143), Color.White);
        }
    }
}
