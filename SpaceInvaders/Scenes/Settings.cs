using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SpaceInvaders.Utils;

namespace SpaceInvaders.Scenes {
    internal class Settings : IScene {
        ContentManager Content;
        SpriteFont text;

        private enum Setting {
            None,
            Controls,
            Audio
        }
        private enum ControlSetting {
            shoot,
            left,
            right,
            pause
        }

        Setting settingChosen = Setting.None;
        Setting settingHovered = Setting.Controls;
        ControlSetting controlSetting = ControlSetting.shoot;
        KeyboardState input = Keyboard.GetState();
        KeyboardState previousInput;
        bool changeControl = false;

        public Settings(ContentManager contentManager) {
            Content = contentManager;
        }

        public void LoadContent() {
            text = Content.Load<SpriteFont>("Font");
            previousInput = input;
        }

        public void Update(GameTime gameTime) {
            input = Keyboard.GetState();

            if (input == previousInput) { return; }
            if (changeControl) {
                if (input.GetPressedKeyCount() != 0) {
                    if (controlSetting == ControlSetting.shoot) {
                        Utils.Controls.shoot = input.GetPressedKeys()[0];
                    }
                    if (controlSetting == ControlSetting.pause) {
                        Utils.Controls.pause = input.GetPressedKeys()[0];
                    }
                    if (controlSetting == ControlSetting.left) {
                        Utils.Controls.moveLeft = input.GetPressedKeys()[0];
                    }
                    if (controlSetting == ControlSetting.right) {
                        Utils.Controls.moveRight = input.GetPressedKeys()[0];
                    }

                    changeControl = false;
                }
                return;
            }

            if (input.IsKeyDown(Utils.Controls.shoot) && input != previousInput) {
                previousInput = input;
                settingChosen = settingHovered;
            }

            if (settingChosen == Setting.None) {
                Main();
            } else if (settingChosen == Setting.Controls) {
                Controls();
            }

            previousInput = input;
        }


        void Main() {
            if (input.IsKeyDown(Keys.Down) && settingHovered == Setting.Controls) {
                settingHovered = Setting.Audio;
            }

            if (input.IsKeyDown(Keys.Up) && settingHovered == Setting.Audio) {
                settingHovered = Setting.Controls;
            }
        }
        void Controls() {
            if (input == previousInput) { return; }

            if (input.IsKeyDown(Keys.Down) && controlSetting == ControlSetting.shoot) {
                controlSetting = ControlSetting.pause;
            } else if (input.IsKeyDown(Keys.Down) && controlSetting == ControlSetting.pause) {
                controlSetting = ControlSetting.left;
            } else if (input.IsKeyDown(Keys.Down) && controlSetting == ControlSetting.left) {
                controlSetting = ControlSetting.right;
            }

            if (input.IsKeyDown(Keys.Up) && controlSetting == ControlSetting.pause) {
                controlSetting = ControlSetting.shoot;
            } else if (input.IsKeyDown(Keys.Up) && controlSetting == ControlSetting.left) {
                controlSetting = ControlSetting.pause;
            } else if (input.IsKeyDown(Keys.Up) && controlSetting == ControlSetting.right) {
                controlSetting = ControlSetting.left;
            }

            if (input.IsKeyDown(Utils.Controls.shoot)) {
                changeControl = true;
            }
        }

        public void Draw(SpriteBatch spriteBatch) {
            if (settingChosen == Setting.None) {
                spriteBatch.DrawString(text, settingHovered == Setting.Controls ? "* Controls" : "Controls", new(5,0), Color.White);
                spriteBatch.DrawString(text, settingHovered == Setting.Audio ? "* Audio" : "Audio", new(5,26), Color.White);
            } else if (settingChosen == Setting.Controls) {
                spriteBatch.DrawString(text, 
                    controlSetting == ControlSetting.shoot ? "* Shoot: " + Utils.Controls.shoot.ToString() : "Shoot: " + Utils.Controls.shoot.ToString(),
                    new(5, 0), Color.White);

                spriteBatch.DrawString(text,
                    controlSetting == ControlSetting.pause ? "* Pause: " + Utils.Controls.pause.ToString() : "Pause: " + Utils.Controls.pause.ToString(),
                    new(5, 26), Color.White);

                spriteBatch.DrawString(text,
                    controlSetting == ControlSetting.left ? "* Left: " + Utils.Controls.moveLeft.ToString() : "Left: " + Utils.Controls.moveLeft.ToString(),
                    new(5, 26*2), Color.White);

                spriteBatch.DrawString(text,
                    controlSetting == ControlSetting.right ? "* Right: " + Utils.Controls.moveRight.ToString() : "Right: " + Utils.Controls.moveRight.ToString(),
                    new(5, 26 * 3), Color.White);
            }
        }

        public void HighResDraw(SpriteBatch spriteBatch) {}
    }
}
