using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SpaceInvaders.Utils;
using System;

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

        private enum AudioSetting {
            music,
            sfx
        }

        Setting settingChosen = Setting.None;
        Setting settingHovered = Setting.Controls;
        ControlSetting controlSetting = ControlSetting.shoot;
        AudioSetting audioSetting = AudioSetting.sfx;

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
                        Utils.Controls.P1shoot = input.GetPressedKeys()[0];
                    }
                    if (controlSetting == ControlSetting.pause) {
                        Utils.Controls.P1pause = input.GetPressedKeys()[0];
                    }
                    if (controlSetting == ControlSetting.left) {
                        Utils.Controls.P1moveLeft = input.GetPressedKeys()[0];
                    }
                    if (controlSetting == ControlSetting.right) {
                        Utils.Controls.P1moveRight = input.GetPressedKeys()[0];
                    }

                    changeControl = false;
                    previousInput = input;
                }
                return;
            }


            if (settingChosen == Setting.None) {
                if (input.IsKeyDown(Utils.Controls.P1shoot) && input != previousInput) {
                    previousInput = input;
                    settingChosen = settingHovered;
                }
                Main();
            } else if (settingChosen == Setting.Controls) {
                Controls();
            } else if (settingChosen == Setting.Audio) {
                Audio();
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

            if (input.IsKeyDown(Utils.Controls.P1shoot)) {
                changeControl = true;
            }
        }

        void Audio() {
            if (input == previousInput) { return; }

            if (input.IsKeyDown(Keys.Down) && audioSetting == AudioSetting.sfx) {
                audioSetting = AudioSetting.music;
            }

            if (input.IsKeyDown(Keys.Up) && audioSetting == AudioSetting.music) {
                audioSetting = AudioSetting.sfx;
            }

            if (input.IsKeyDown(Keys.Left)) {
                if (audioSetting == AudioSetting.sfx && Globals.sfxVolume != 0) {
                    Globals.sfxVolume = Math.Round(Globals.sfxVolume - 0.1, 1);
                } else if (audioSetting == AudioSetting.music && Globals.musicVolume != 0) {
                    Globals.musicVolume = Math.Round(Globals.musicVolume - 0.1, 1);
                }
            }
            if (input.IsKeyDown(Keys.Right)) {
                if (audioSetting == AudioSetting.sfx && Globals.sfxVolume != 1) {
                    Globals.sfxVolume = Math.Round(Globals.sfxVolume + 0.1, 1);
                } else if (audioSetting == AudioSetting.music && Globals.musicVolume != 1) {
                    Globals.musicVolume = Math.Round(Globals.musicVolume + 0.1, 1);
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch) {
            if (settingChosen == Setting.None) {
                spriteBatch.DrawString(text, settingHovered == Setting.Controls ? "* Controls" : "Controls", new(5,0), Color.White);
                spriteBatch.DrawString(text, settingHovered == Setting.Audio ? "* Audio" : "Audio", new(5,26), Color.White);
            } else if (settingChosen == Setting.Controls) {
                spriteBatch.DrawString(text, 
                    controlSetting == ControlSetting.shoot ? "* Shoot: " + Utils.Controls.P1shoot.ToString() : "Shoot: " + Utils.Controls.P1shoot.ToString(),
                    new(5, 0), Color.White);

                spriteBatch.DrawString(text,
                    controlSetting == ControlSetting.pause ? "* Pause: " + Utils.Controls.P1pause.ToString() : "Pause: " + Utils.Controls.P1pause.ToString(),
                    new(5, 26), Color.White);

                spriteBatch.DrawString(text,
                    controlSetting == ControlSetting.left ? "* Left: " + Utils.Controls.P1moveLeft.ToString() : "Left: " + Utils.Controls.P1moveLeft.ToString(),
                    new(5, 26*2), Color.White);

                spriteBatch.DrawString(text,
                    controlSetting == ControlSetting.right ? "* Right: " + Utils.Controls.P1moveRight.ToString() : "Right: " + Utils.Controls.P1moveRight.ToString(),
                    new(5, 26 * 3), Color.White);
            } else if (settingChosen == Setting.Audio) {
                spriteBatch.DrawString(text,
                    audioSetting == AudioSetting.sfx ? "* Sound: " + Globals.sfxVolume * 10 : "Sound: " + Globals.sfxVolume * 10,
                    new(5,0), Color.White);

                spriteBatch.DrawString(text,
                    audioSetting == AudioSetting.music ? "* Music: " + Globals.musicVolume * 10 : "Music: " + Globals.musicVolume * 10,
                    new(5, 26), Color.White);
            }
        }

        public void HighResDraw(SpriteBatch spriteBatch) {}
    }
}
