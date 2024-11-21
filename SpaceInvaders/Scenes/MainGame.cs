using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SpaceInvaders.Entities.Aliens;
using SpaceInvaders.Entities.Bosses;
using SpaceInvaders.Entities;
using SpaceInvaders.Utils;
using SpaceInvaders.Entities.Powerboxes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SpaceInvaders.Scenes {
    internal class MainGame : IScene {
        static internal KeyboardState input;
        static internal KeyboardState previousInput;
        static ContentManager Content;

        Background background;
        SpriteFont text;

        Player player = new();
        List<Alien> aliens = new();
        private BasicBoss currentBoss;
        private List<Powerbox> powerboxes = new();
        private int powerboxSummonTimer = 0;

        static List<Bullet> bullets = new();
        static List<Bullet> enemyBullets = new();
        static bool
            playerBulletDeleted = false,
            enemyBulletDeleted = false;

        private int finalWave = 20;
        private int highestAlienType = 2;
        private int freeEnemySpawnTimer, freeEnemySpawnTimerReset;

        string[] waves = new string[5];
        int[] waveHeight = new int[5];
        
        static private int id = 0;
        private int wave = 0;

        private int lostTimer = 30, lostTimerReset = 30;
        private bool won = false;

        Random rng = new();

        public MainGame(ContentManager contentManager) {
            Content = contentManager;
        }


        void IScene.LoadContent() {
            text = Content.Load<SpriteFont>("FontBig");
            
            // Set all variables to default
            player.sprite = Content.Load<Texture2D>("ShipSprites/normal");
            player.position = new Vector2(72, 160);
            player.hitbox = new(0, 0, 14, 14);

            // Define Background
            background = new(Content.Load<Texture2D>("Background"), new(-41, -48));
            
            Alien.count = 0;
            Bullet.bulletCount = 0;

            bullets.Clear(); enemyBullets.Clear();
            playerBulletDeleted = false; enemyBulletDeleted = false;
            id = 0;
            
            // Declare what each wave has for enemies
            // This is only used for the first 5 waves due to me not wanting to hardcode all the waves.
            waves[0] = "   11     1111   111111 ";
            waves[1] = "111   111  11  11 1  1 1 111111 ";
            waves[2] = "11  2 1111 2  1111  2 1111 2  11";
            waves[3] = "122  221  2112  12121212 2 2 2 2";
            waves[4] = "        21222212";

            waveHeight[0] = 3;
            waveHeight[1] = 4;
            waveHeight[2] = 4;
            waveHeight[3] = 4;
            waveHeight[4] = 2;

            if (waves.Contains(null)) {
                throw new IndexOutOfRangeException("Not enough waves are declared!");
            }

            newEnemyBatch();

            powerboxSummonTimer = rng.Next(1800, 3600*2); // 30 secs, 2 mins
            freeEnemySpawnTimerReset = 60 * 60;
            freeEnemySpawnTimer = freeEnemySpawnTimerReset;

            previousInput = Keyboard.GetState();
        }

        void IScene.Update(GameTime gameTime) {
            CheckGameCondition();
            // Has player died?
            if (player.health == 0) {
                LostUpdate();
                return;
            }

            input = Keyboard.GetState();

            UpdateEntities(gameTime);
            CheckCollision();

            CheckPowerbox();

            previousInput = input;

            playerBulletDeleted = false;
            enemyBulletDeleted = false;
        }

        void CheckGameCondition() {
            // Have all aliens been killed?
            if (Alien.count == 0) {
                if (wave == finalWave && currentBoss == null) {
                    won = true;
                } else {
                    wave++;
                    // Spawn a boss?
                    if (wave == 4) {
                        currentBoss = new Zhyron(Content.Load<Texture2D>("BossSprites/Zhyron"), wave);
                    }
                    if (wave == 1) {
                        currentBoss = new Seraphim(Content.Load<Texture2D>("BossSprites/Seraphim"), wave);
                    }

                    // Enemy spawning method
                    if (wave < 5) {
                        newEnemyBatch();
                    } else {
                        while (Alien.count == 0) {
                            newRandomEnemyBatch();
                        }
                    }
                }

            }

            if (wave == 8) {
                highestAlienType = 3;
            }

            if (wave >= 14) { // 15th wave start spawning "free" enemies.
                freeEnemySpawnTimer--;

                if (freeEnemySpawnTimer == 0) {
                    int type = rng.Next(1, highestAlienType + 1);
                    int xPos = rng.Next(0, 7);

                    createEnemy(xPos, 4, type, free: true);

                    freeEnemySpawnTimer = freeEnemySpawnTimerReset;
                }
            }
        }
        
        void UpdateEntities(GameTime gameTime) {
            // Run Update code of every entity
            // Player
            player.Update();

            //Alien
            foreach (Alien alien in aliens) {
                alien.Update(gameTime);
                if (alien.position.Y >= 160) {
                    player.health = 0;
                }
            }

            // Player Bullets
            foreach (Bullet bullet in bullets) {
                bullet.Update();
                if (playerBulletDeleted) { break; }
            }

            // Enemy Bullets
            foreach (Bullet bullet in enemyBullets) {
                bullet.Update();
                if (enemyBulletDeleted) { break; }
            }
            // Boss
            if (currentBoss != null) {
                currentBoss.Update();
                if (currentBoss.health == 0) {
                    currentBoss = null;
                }
            }

            // Powerbox
            foreach (Powerbox powerbox in powerboxes) {
                powerbox.Update();
            }

            CheckPowerbox();
        }
        
        void CheckCollision() {
            // Is the bullet colliding with an Alien?
            foreach (Bullet bullet in bullets) {
                bool removeBullet = false;
                foreach (Alien alien in aliens) {
                    if (bullet.hitbox.Intersects(alien.hitbox) && !alien.isImmune) {
                        aliens.Remove(aliens.Find(x => x.id == alien.id)); // Little black magic here
                        Alien.count--;
                        removeBullet = true;
                        break;
                    }
                }
                // If not colliding with Alien, boss collision?
                if (removeBullet == false && currentBoss != null) {
                    if (bullet.hitbox.Intersects(currentBoss.hitbox)) {
                        currentBoss.health--;
                        removeBullet = true;
                    }
                }

                // If colliding delete bullet.
                if (removeBullet) {
                    bullets.Remove(bullets.Find(x => x.id == bullet.id));
                    Bullet.bulletCount--;
                    break;
                }
            }

            // Is enemy bullet colliding with player?
            foreach (Bullet bullet in enemyBullets) {
                if (bullet.hitbox.Intersects(player.hitbox) && !won) {
                    player.registerDamage(bullet.damage);
                    enemyBullets.Remove(enemyBullets.Find(x => x.id == bullet.id));
                    // Bullet.bulletCount--;
                    break;
                }
            }

            // Is a powerbox colliding with player?
            foreach (Powerbox powerbox in powerboxes) {
                if (powerbox.hitbox.Intersects(player.hitbox)) {
                    if (powerbox.GetType() == typeof(HealBox)) {
                        if (player.health != 3) {
                            player.health++;
                            player.updateSprite();
                        }
                    } else if (powerbox.GetType() == typeof(BulletBox)) {
                        player.maxBullets++;
                    } else if (powerbox.GetType() == typeof(SplitBulletBox)) {
                        player.splitBullet = true;
                    } else if (powerbox.GetType() == typeof(BulletSpeedBox)) {
                        player.bulletSpeed -= 2;
                    }

                    powerboxes.Remove(powerboxes.Find(x => x.id == powerbox.id));
                    break;
                }
            }
        }

        void LostUpdate() {
            // Little "animation" that removes every enemy 1 by 1
            if (lostTimer == 0) {
                if (lostTimerReset == 120) {
                    Globals.gameLost = true;
                }

                if (aliens.Count != 0) {
                    aliens.Remove(aliens.Last());
                }
                if (bullets.Count != 0) {
                    bullets.Remove(bullets.Last());
                }
                if (enemyBullets.Count != 0) {
                    enemyBullets.Remove(enemyBullets.Last());
                }

                lostTimer = lostTimerReset;
            }

            // When all entities are gone, wait extra then boot user to menu.
            if (aliens.Count == 0 && bullets.Count == 0 && enemyBullets.Count == 0 && lostTimerReset != 120) {
                lostTimerReset = 120;
                lostTimer = lostTimerReset;
            }

            lostTimer--;
        }


        // Makes a new wave
        void newEnemyBatch() {
            int heightOffset = wave == 0 ? 5 : -64;

            for (int y = 0; y < waveHeight[wave]; y++) {
                for (int x = 0; x < 8; x++) {
                    int gridSpot = y * 8 + x;
                    if (waves[wave][gridSpot] == ' ') { continue; }

                    aliens.Add(new BasicAlien(int.Parse(waves[wave][gridSpot].ToString()), // Due to the first 5 waves only having 2 types, we can ignore the special types.
                        new Vector2(16+16*x, heightOffset+16*y),
                        new Rectangle(2,2,11,11),
                        id));
                    
                    aliens.Last().sprite = Content.Load<Texture2D>(aliens.Last().getTextureName());
                    Alien.count++;
                    id++;
                }
            }
        }

        void newRandomEnemyBatch() {
            Random rng = new();
            for (int y = 0; y < 4; y++) {
                for (int x = 0; x < 8; x++) {
                    int chanceOfEmpty = 2 + wave/finalWave;
                    int type = rng.Next(1, highestAlienType+1);

                    if (rng.Next(0,chanceOfEmpty) == 1) { continue; }

                    createEnemy(x,y,type);
                }
            }
        }

        void createEnemy(int xPos, int yPos, int type, bool free = false) {
            switch (type) {
                case 1:
                case 2:
                    aliens.Add(new BasicAlien(type,
                        new Vector2(16 + 16 * xPos, -64 + 16 * yPos),
                        new Rectangle(2, 2, 11, 11),
                        id));
                    break;
                case 3:
                    aliens.Add(new BoltAlien(type,
                    new Vector2(16 + 16 * xPos, -64 + 16 * yPos),
                    new Rectangle(2, 2, 11, 11),
                    id));
                    break;
            }

            aliens.Last().sprite = Content.Load<Texture2D>(aliens.Last().getTextureName());
            Alien.count++;
            id++;
        }

        // Powerbox code
        void CheckPowerbox() {
            powerboxSummonTimer--;

            if (powerboxSummonTimer == 0) {
                powerboxSummonTimer = rng.Next(5400, 10800); // 1.5 min, 3 min
                int minUpgrade = 1;
                if (player.health == 3) {
                    minUpgrade += 1;
                }

                int boxToSummon = rng.Next(minUpgrade,5);
                int boxXPos = rng.Next(3, 141);

                if (boxToSummon == 1) {
                    powerboxes.Add(new HealBox(
                        new(boxXPos, -16),
                        Content.Load<Texture2D>("Powerbox/Heal"),
                        60,
                        id));
                    id++;
                } else if (boxToSummon == 2 && player.maxBullets < wave / 5) {
                    powerboxes.Add(new BulletBox(
                        new(boxXPos, -16),
                        Content.Load<Texture2D>("Powerbox/Bullet"),
                        20,
                        id));
                    id++;
                } else if (boxToSummon == 3 && player.maxBullets > 2) {
                    powerboxes.Add(new SplitBulletBox(
                        new(boxXPos, -16),
                        Content.Load<Texture2D>("Powerbox/BulletSplit"),
                        30,
                        id));
                    id++;
                } else {
                    powerboxes.Add(new BulletSpeedBox(
                        new(boxXPos, -16),
                        Content.Load<Texture2D>("Powerbox/BulletSpeed"),
                        30,
                        id));
                    id++;
                }
            } 
            
            foreach (Powerbox powerbox in powerboxes) {
                if (powerbox.position.Y > 192) {
                    powerboxes.Remove(powerboxes.Find(x => x.id == powerbox.id));
                    break;
                }
            }
        }


        // Code to spawn new bullet
        static internal void newPlayerBullet(int xPosition, Vector2 direction) {
            bullets.Add(new(position: new(xPosition, 160), new(2, 8), direction: direction, Content.Load<Texture2D>("Bullet"), 1, id));
            id++;
        }
        static internal void newEnemyBullet(Vector2 position, Vector2 direction, string type, bool bossBullet = false, int damage = 1) {
            if (!bossBullet) {
                enemyBullets.Add(new(
                    position: position,
                    hitboxSize: new(6, 6),
                    direction: direction,
                    Content.Load<Texture2D>("EnemyBulletSprites/EnemyBullet"+type),
                    id,
                    damage,
                    evil:true));
            } else {
                enemyBullets.Add(new(
                    position: position, 
                    hitboxSize: new (6, 6),
                    direction: direction,
                    Content.Load<Texture2D>("BossBullets/BulletType"+type),
                    id,
                    damage,
                    evil:true));
            }
            id++;
        }

        // Code to delete bullet
        static internal void deleteBullet(int id, bool evil) {
            if (evil) {
                enemyBullets.Remove(enemyBullets.Find(x => x.id == id));
                enemyBulletDeleted = true;
            } else {
                bullets.Remove(bullets.Find(x => x.id == id));
                Bullet.bulletCount--;
                playerBulletDeleted = true;
            }
        }

        void IScene.Draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(background.sprite, background.position, Color.White);

            foreach (Alien alien in aliens) {
                alien.Draw(spriteBatch);
            }
            
            foreach (Bullet bullet in bullets) {
                bullet.Draw(spriteBatch);
            }
            foreach (Bullet bullet in enemyBullets) {
                bullet.Draw(spriteBatch);
            }

            foreach (Powerbox powerbox in powerboxes) {
                powerbox.Draw(spriteBatch);
            }

            if (currentBoss != null) {
                currentBoss.Draw(spriteBatch);
            }

            player.Draw(spriteBatch);

            if (won) {
                spriteBatch.DrawString(text, "YOU WON", new(0,0), Color.White);
            }
        }
        void IScene.HighResDraw(SpriteBatch spriteBatch) {
            spriteBatch.DrawString(text, player.health.ToString(), new(0, 0), Color.White);
        }
    }
}
