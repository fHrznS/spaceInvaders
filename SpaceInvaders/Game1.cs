﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SpaceInvaders.Scenes;
using SpaceInvaders.Utils;
using System;
using System.IO;
using System.Text;

namespace SpaceInvaders {
    public class Game1 : Game {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private SceneManager sceneManager = new();
        private RenderTarget2D renderTarget;
        private RenderTarget2D renderTarget2;

        private int screenScale = 4;

        private KeyboardState input;
        private KeyboardState previousInput;


        private string appData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        private string filePath;
        private bool hasLoaded = false;
        enum GameStates {
            MainMenu,
            Settings,
            MainGame,
            HowTo,
        }
        GameStates gameState;

        public Game1() {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = false;
        }

        protected override void Initialize() {
            Window.Title = "Space Invaders   |  V1.0.0 Aliens taking over";

            // Initialize renderTarget to allow for the pixel style.
            renderTarget = new(GraphicsDevice, Globals.screenWidth, Globals.screenHeight);
            renderTarget2 = new(GraphicsDevice, Globals.screenWidth, Globals.screenHeight);

            // Set what the player sees be "screenScale" times bigger than what's rendered to renderTarget.
            _graphics.PreferredBackBufferWidth = Globals.screenWidth * screenScale;
            _graphics.PreferredBackBufferHeight = Globals.screenHeight * screenScale;
            _graphics.ApplyChanges();

            // Set initial scene to Main Menu passing in Content to allow it to load files.
            sceneManager.AddScene(new MainMenu(Content));
            gameState = GameStates.MainMenu;

            filePath = Path.Combine(appData, "fHrznSpaceInvaders", "save.txt");

            base.Initialize();
        }

        protected override void LoadContent() {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // Load every Powerbox sprite (For How To menu game)
            Sprites.powerboxes.Add(Content.Load<Texture2D>("Powerbox/Heal"));
            Sprites.powerboxes.Add(Content.Load<Texture2D>("Powerbox/Bullet"));
            Sprites.powerboxes.Add(Content.Load<Texture2D>("Powerbox/BulletSpeed"));
            Sprites.powerboxes.Add(Content.Load<Texture2D>("Powerbox/BulletSplit"));
            Sprites.powerboxes.Add(Content.Load<Texture2D>("Powerbox/Resistance"));
            Sprites.powerboxes.Add(Content.Load<Texture2D>("Powerbox/SheildBreaker"));

            // If in multiplayer load 2 games, elsewise only load 1.
            sceneManager.currentScene().LoadContent();
            if (Globals.isMultiplayer) {
                sceneManager.peekDownTo(2).LoadContent();
            }

            if (hasLoaded) {
                return;
            }

            // Try and load save file, elsewise set to default and make a save file.
            try {
                Globals.highscore = int.Parse(File.ReadAllText(filePath));
            } catch (DirectoryNotFoundException) {
                Globals.highscore = -1;
            }
            saveData();

            hasLoaded = true;

        }
        void saveData() {
            // Create the directory and file and write player's highscore at AppData/Local/fHrznSpaceInvaders/save.txt
            Directory.CreateDirectory(Path.Combine(appData, "fHrznSpaceInvaders"));
            using (FileStream fs = File.Create(filePath)) {
                byte[] buf = Encoding.ASCII.GetBytes(Globals.highscore.ToString());
                fs.Write(buf, 0, Globals.highscore.ToString().Length);
            };
        }

        protected override void Update(GameTime gameTime) {
            input = Keyboard.GetState();

            sceneManager.currentScene().Update(gameTime);
            if (Globals.isMultiplayer) {
                sceneManager.peekDownTo(2).Update(gameTime);
            }

            // What scene control do we have?
            switch (gameState) {
                case GameStates.MainMenu:
                    MainMenu();
                    break;
                case GameStates.MainGame:
                    MainGame();
                    break;
                case GameStates.Settings:
                case GameStates.HowTo:
                    SettingsAndHowToMenu();
                    break;
            }

            previousInput = input;
            base.Update(gameTime);
        }

        void loadMainGame() {
            if (Globals.isMultiplayer) {
                sceneManager.AddScene(new MainGame(Content, 1));
                sceneManager.AddScene(new MainGame(Content, 0));
            } else {
                sceneManager.AddScene(new MainGame(Content, 0));
            }
            gameState = GameStates.MainGame;
            Globals.isLoading = true;
            LoadContent();
        }

        // Scene Control: Main Menu
        void MainMenu() {
            if (input == previousInput) { return; }

            if (Globals.isLoading) {
                loadMainGame();
            }

            if (input.IsKeyDown(Controls.P1shoot) && Scenes.MainMenu.selectedOption == Scenes.MainMenu.Options.Start) {
                Globals.isLoading = true;
                Draw(new GameTime());

                Globals.isWorthy = true;
                Globals.easyDifficulty = false;
                Globals.isMultiplayer = false;
                _graphics.PreferredBackBufferWidth = Globals.screenWidth * screenScale;
                _graphics.ApplyChanges();

            } else if (input.IsKeyDown(Controls.P1shoot) && Scenes.MainMenu.selectedOption == Scenes.MainMenu.Options.Easy) {
                Globals.isLoading = true;
                Draw(new GameTime());

                Globals.isWorthy = false;
                Globals.easyDifficulty = true;
                Globals.isMultiplayer = false;
                _graphics.PreferredBackBufferWidth = Globals.screenWidth * screenScale;
                _graphics.ApplyChanges();
            } else if (input.IsKeyDown(Controls.P1shoot) && Scenes.MainMenu.selectedOption == Scenes.MainMenu.Options.Settings) {
                sceneManager.AddScene(new Settings(Content));
                gameState = GameStates.Settings;
                LoadContent();
            } else if (input.IsKeyDown(Controls.P1shoot) && Scenes.MainMenu.selectedOption == Scenes.MainMenu.Options.HowTo) {
                sceneManager.AddScene(new HowTo(Content));
                gameState = GameStates.HowTo;
                LoadContent();
            } else if (input.IsKeyDown(Controls.P1shoot) && Scenes.MainMenu.selectedOption == Scenes.MainMenu.Options.Multiplayer) {
                Globals.isLoading = true;
                Draw(new GameTime());

                Globals.isWorthy = true;
                Globals.easyDifficulty = false;
                
                Globals.isMultiplayer = true;
                _graphics.PreferredBackBufferWidth = Globals.screenWidth * screenScale * 2;
                _graphics.ApplyChanges();
            } else if (input.IsKeyDown(Keys.Escape)) {
                saveData();
                Exit();
            }
        }

        // Scene Control: Main Game
        void MainGame() {
            if (Globals.gameLost) {
                Globals.gameLost = false;
                saveData();
                sceneManager.RemoveScene();
                gameState = GameStates.MainMenu;
            }

            if (input == previousInput) { return; }

            if (input.IsKeyDown(Keys.Escape)) {
                sceneManager.RemoveScene();
                if (Globals.isMultiplayer) {
                    sceneManager.RemoveScene();
                    _graphics.PreferredBackBufferWidth = Globals.screenWidth * screenScale;
                    _graphics.ApplyChanges();
                }

                LoadContent();
                gameState = GameStates.MainMenu;
            }

        }

        // How To / Settings menu controls
        void SettingsAndHowToMenu() {
            if (input == previousInput) { return; }

            if (input.IsKeyDown(Keys.Escape)) {
                sceneManager.RemoveScene();
                gameState = GameStates.MainMenu;
            }
        }

        protected override void Draw(GameTime gameTime) {
            // Set the Render Target to our renderTarget which is small.
            GraphicsDevice.SetRenderTarget(renderTarget);

            GraphicsDevice.Clear(Color.Black);

            _spriteBatch.Begin();
            
            sceneManager.currentScene().Draw(_spriteBatch);

            _spriteBatch.End();

            // Set Render Target back to main (what the user sees)
            // And draw renderTarget to the screen scaled up with "PointClamp" to keep the pixelated look.
            GraphicsDevice.SetRenderTarget(null);


            _spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            
            _spriteBatch.Draw(renderTarget,
                new Rectangle(0, 0,
                    Globals.screenWidth * screenScale,
                    Globals.screenHeight * screenScale),
                Color.White);
            sceneManager.currentScene().HighResDraw(_spriteBatch);
            
            _spriteBatch.End();

            //
            // If in multiplayer, draw 2nd screen
            //

            if (Globals.isMultiplayer) {

                GraphicsDevice.SetRenderTarget(renderTarget2);

                GraphicsDevice.Clear(Color.Black);

                _spriteBatch.Begin();

                sceneManager.peekDownTo(2).Draw(_spriteBatch);

                _spriteBatch.End();

                // Set Render Target back to main (what the user sees)
                // And draw renderTarget to the screen scaled up with "PointClamp" to keep the pixelated look.
                GraphicsDevice.SetRenderTarget(null);


                _spriteBatch.Begin(samplerState: SamplerState.PointClamp);

                _spriteBatch.Draw(renderTarget2,
                    new Rectangle(Globals.screenWidth * screenScale, 0,
                        Globals.screenWidth * screenScale,
                        Globals.screenHeight * screenScale),
                    Color.White);
                _spriteBatch.Draw(renderTarget,
                new Rectangle(0, 0,
                    Globals.screenWidth * screenScale,
                    Globals.screenHeight * screenScale),
                Color.White);
                sceneManager.currentScene().HighResDraw(_spriteBatch);
                sceneManager.peekDownTo(2).HighResDraw(_spriteBatch);

                _spriteBatch.End();
            }


            base.Draw(gameTime);
        }
    }
}

