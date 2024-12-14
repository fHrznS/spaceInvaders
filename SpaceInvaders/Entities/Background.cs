using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceInvaders.Utils;

namespace SpaceInvaders.Entities {
    internal class Background : BasicObject {
        int subPixel;
        int bgNum,
            currentBG;
        int transitionTimer = Time.ToFrames(15);
        int endFade = 60 * 30;
        Texture2D newSprite;

        public Background(int wave) {
            sprite = Sprites.backgrounds[whichBG(wave)];
            newSprite = sprite;
            position = new(0,-384);
        }

        public void Update(int wave) {
            subPixel++;

            if (subPixel == 2 + (bgNum == 4 ? 3 : 0)) {
                subPixel = 0;
                position.Y++;
            }

            bgNum = whichBG(wave);

            newSprite = Sprites.backgrounds[bgNum];

            if (position.Y == 0) {
                position.Y = -384;
            }

            if (endFade > 0 && wave >= 500) {
                endFade--;
            }

            transition();
        }

        int whichBG(int wave) {
            if (wave >= 450) {
                return 4;
            } else if (wave >= 300) {
                return 3;
            } else if (wave >= 150) {
                return 2;
            } else if (wave >= 50) {
                return 1;
            } else {
                return 0;
            }
        }

        void transition() {
            if (currentBG != bgNum) {
                transitionTimer--;

                if (transitionTimer == 0) {
                    transitionTimer = Time.ToFrames(15);
                    sprite = Sprites.backgrounds[bgNum];
                    currentBG = bgNum;  
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch) {
            if (endFade < 60 * 30) {
                spriteBatch.Draw(sprite, position, Color.White * (endFade / (float)Time.ToFrames(30)));
            } else {
                spriteBatch.Draw(newSprite, position, Color.White);
                spriteBatch.Draw(sprite, position, Color.White * (transitionTimer / (float)Time.ToFrames(15)));
            }
        }
    }
}
