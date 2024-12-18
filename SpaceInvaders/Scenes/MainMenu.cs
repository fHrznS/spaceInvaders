﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using SpaceInvaders.Utils;
using System;
using System.IO;
using System.Text;

namespace SpaceInvaders.Scenes
{
    internal class MainMenu : IScene
    {
        public enum Options {
            Start,
            Easy,
            Multiplayer,
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
            
            MediaPlayer.Stop();
        }

        void IScene.Update(GameTime gameTime) {
            KeyboardState input = Keyboard.GetState();

            if (input == previousInput) { return; }

            if (input.IsKeyDown(Keys.Down)) {
                if (selectedOption == Options.Start) {
                    selectedOption = Options.Multiplayer;
                } else if (selectedOption == Options.Multiplayer) {
                    selectedOption = Options.Settings;
                } else if (selectedOption == Options.Settings) {
                    selectedOption = Options.HowTo;
                }
            }

            if (input.IsKeyDown(Keys.Up)) {
                if (selectedOption == Options.Multiplayer) {
                    selectedOption = Options.Start;
                } else if (selectedOption == Options.Settings) {
                    selectedOption = Options.Multiplayer;
                } else if (selectedOption == Options.HowTo) {
                    selectedOption = Options.Settings;
                }
            }

            if (input.IsKeyDown(Controls.P1moveRight)) {
                selectedOption = Options.Easy;
            }
            else if (input.IsKeyDown(Controls.P1moveLeft)) {
                selectedOption = Options.Start;
            }

            previousInput = input;
        }

        void IScene.Draw(SpriteBatch spriteBatch) {
            if (Globals.isLoading) {
                spriteBatch.DrawString(text, "Loading...", new(3, 1), Color.White);
                return;
            }

            spriteBatch.DrawString(text, 
                selectedOption == Options.Easy ? "* Easy" : (selectedOption == Options.Start ? "* Play" : "Play"), // My least favorite line in the program
                new Vector2(2,0), Color.White);
            spriteBatch.DrawString(text, selectedOption == Options.Multiplayer? "* Multiplayer" : "Multiplayer", new Vector2(2,24), Color.White);
            spriteBatch.DrawString(text, selectedOption == Options.Settings ? "* Settings" : "Settings", new Vector2(2,48), Color.White);
            spriteBatch.DrawString(text, selectedOption == Options.HowTo ? "* Help" : "Help", new Vector2(2,72), Color.White);
        }

        void IScene.HighResDraw(SpriteBatch spriteBatch) {
            if (!Globals.isLoading) {
                spriteBatch.DrawString(text,"Press " + Controls.P1shoot.ToString() + " to confirm choice", new Vector2(5, 742), Color.White);
                spriteBatch.DrawString(text,"Highscore: Wave " + (Globals.highscore + 1).ToString(), new Vector2(5, 742-24), Color.White);
            }
        }
    }
}
