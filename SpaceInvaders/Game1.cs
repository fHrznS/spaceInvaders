﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SpaceInvaders.Utils;

namespace SpaceInvaders {
    public class Game1 : Game {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private SceneManager sceneManager = new();
        private RenderTarget2D renderTarget;

        private int screenScale = 4;

        private KeyboardState input;
        private KeyboardState previousInput;

        enum GameStates {
            MainMenu,
            Settings,
            MainGame,
            Boss
        }
        GameStates gameState;

        public Game1() {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = false;
        }

        protected override void Initialize() {
            Window.Title = "Space Invaders   |  V0.1.0 Zhyron Appears!";

            // Initialize renderTarget to allow for the pixel style.
            renderTarget = new(GraphicsDevice, Globals.screenWidth, Globals.screenHeight);

            // Set what the player sees be "screenScale" times bigger than what's rendered to renderTarget.
            _graphics.PreferredBackBufferWidth = Globals.screenWidth * screenScale;
            _graphics.PreferredBackBufferHeight = Globals.screenHeight * screenScale;
            _graphics.ApplyChanges();

            // Set initial scene to Main Menu passing in Content to allow it to load files.
            sceneManager.AddScene(new Scenes.MainMenu(Content));
            gameState = GameStates.MainMenu;

            base.Initialize();
        }

        protected override void LoadContent() {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            sceneManager.currentScene().LoadContent();
        }

        protected override void Update(GameTime gameTime) {
            input = Keyboard.GetState();

            sceneManager.currentScene().Update(gameTime);

            // What scene control do we have?
            switch (gameState) {
                case GameStates.MainMenu:
                    MainMenu();
                    break;
                case GameStates.MainGame:
                    MainGame();
                    break;
            }

            previousInput = input;
            base.Update(gameTime);
        }

        // Scene Control: Main Menu
        void MainMenu() {
            if (input == previousInput) { return; }

            if (input.IsKeyDown(Keys.Z)) {
                sceneManager.AddScene(new Scenes.MainGame(Content));
                gameState = GameStates.MainGame;
                LoadContent();
            } else if (input.IsKeyDown(Keys.Escape)) {
                Exit();
            }
        }

        // Scene Control: Main Game
        void MainGame() {
            if (Globals.gameLost == true) {
                Globals.gameLost = false;
                sceneManager.RemoveScene();
                gameState = GameStates.MainMenu;
            }

            if (input == previousInput) { return; }

            if (input.IsKeyDown(Keys.Escape)) {
                sceneManager.RemoveScene();
                LoadContent();
                gameState = GameStates.MainMenu;
            }

        }

        protected override void Draw(GameTime gameTime) {
            // Set the Render Target to out renderTarget which is small.
            GraphicsDevice.SetRenderTarget(renderTarget);

            GraphicsDevice.Clear(Color.Black);

            _spriteBatch.Begin();

            sceneManager.currentScene().Draw(_spriteBatch);

            _spriteBatch.End();

            // Set Render Target back to main (what the user sees)
            // And draw renderTarget to the screen scaled up with "PointClamp" to keep pixelated look.
            GraphicsDevice.SetRenderTarget(null);


            _spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            
            _spriteBatch.Draw(renderTarget,
                new Rectangle(0, 0,
                    Globals.screenWidth * screenScale,
                    Globals.screenHeight * screenScale),
                Color.White);
            sceneManager.currentScene().HighResDraw(_spriteBatch);
            
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}


// TODO:
// 1. Basic Boss
// 2. Powerups
// 3. Free enemies (Not stuck to batch)
// 4. Moving BG
// 5. Differently sized (width) batches, and movability.
// 6. Music and sounds
// 7. More animations