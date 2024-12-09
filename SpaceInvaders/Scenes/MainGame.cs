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
using Microsoft.Xna.Framework.Audio;

namespace SpaceInvaders.Scenes {
    enum GameState {
        Running,
        Paused
    }

    internal class MainGame : IScene {
        GameState gameState = GameState.Running;

        static internal KeyboardState input;
        static internal KeyboardState previousInput;
        static ContentManager Content;

        Background background;
        public static SpriteFont text;

        Player player = new();
        List<Alien> aliens = new();
        private List<ParticleObject> particleObjects = new();
        private List<BasicBoss> currentBoss = new();
        private List<Powerbox> powerboxes = new();
        private int powerboxSummonTimer = 0;

        static List<Bullet> bullets = new();
        static List<Bullet> enemyBullets = new();
        static bool
            playerBulletDeleted = false,
            enemyBulletDeleted = false;

        private int finalWave = 349;
        private int highestAlienType = 2;
        private int freeEnemySpawnTimer, freeEnemySpawnTimerReset;

        string[] waves = new string[5];
        int[] waveHeight = new int[5];

        static private int id = 0;
        private int wave = 0;
        private int previousWave = 0; // Used for the secret boss as to not reset player stats every frame.

        private int lostTimer = 30, lostTimerReset = 30;
        private bool won = false;

        private int maxWaveHeight = 4;
        private int minimumEnemy = 1;
        private int baseEmpty = 2;

        private bool finishedLoading = false;

        private int worthyText = 0;
        private string[] worthyMessages = { "Well done", "You're interesting", "It deems you worthy", "It wants to see you", "Edge of universes"};
        private int worthyMessageNumber = 0;

        Random rng = new();

        public MainGame(ContentManager contentManager) {
            Content = contentManager;
        }


        void IScene.LoadContent() {
            text = Content.Load<SpriteFont>("FontBig");
            
            // Set all variables to default
            player.sprite = Content.Load<Texture2D>("ShipSprites/normal");
            player.position = new Vector2(72, 160);
            player.hitbox = new(1, 4, 14, 10);

            // Define Background
            background = new(Content.Load<Texture2D>("Background"));
            
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


            powerboxSummonTimer = rng.Next(1800, 3600*2); // 30 secs, 2 mins
            freeEnemySpawnTimerReset = 60 * 60;
            freeEnemySpawnTimer = freeEnemySpawnTimerReset;

            Globals.disableEnemyShooting = false;
            Globals.invasionMode = false;
            Globals.stopSpawn = false;
            
            // Load all content
            if (Sprites.bullets.Count == 0) {
                // TEXTURES
                // Load every bullet sprites
                Sprites.bullets.Add(Content.Load<Texture2D>("Bullet"));
                Sprites.bullets.Add(Content.Load<Texture2D>("EnemyBulletSprites/EnemyBullet1"));
                Sprites.bullets.Add(Content.Load<Texture2D>("EnemyBulletSprites/EnemyBullet2"));
                Sprites.bullets.Add(null);
                Sprites.bullets.Add(Content.Load<Texture2D>("EnemyBulletSprites/EnemyBullet3"));

                Sprites.bossBullets.Add(Content.Load<Texture2D>("BossBullets/BulletType1"));
                Sprites.bossBullets.Add(Content.Load<Texture2D>("BossBullets/BulletType2"));
                Sprites.bossBullets.Add(Content.Load<Texture2D>("BossBullets/BulletType3"));
                Sprites.bossBullets.Add(Content.Load<Texture2D>("BossBullets/BulletType4"));
                Sprites.bossBullets.Add(Content.Load<Texture2D>("BossBullets/BulletType5"));
                Sprites.bossBullets.Add(Content.Load<Texture2D>("BossBullets/BulletType6"));
                Sprites.bossBullets.Add(Content.Load<Texture2D>("BossBullets/BulletType7"));

                // Load every boss sprite
                Sprites.bosses.Add(Content.Load<Texture2D>("BossSprites/Zhyron"));
                Sprites.bosses.Add(Content.Load<Texture2D>("BossSprites/Seraphim"));
                Sprites.bosses.Add(Content.Load<Texture2D>("BossSprites/Gabriel"));
                Sprites.bosses.Add(Content.Load<Texture2D>("BossSprites/Lilith"));
                Sprites.bosses.Add(Content.Load<Texture2D>("BossSprites/AdamAndEve"));
                Sprites.bosses.Add(Content.Load<Texture2D>("BossSprites/TheMothership"));
                Sprites.bosses.Add(Content.Load<Texture2D>("BossSprites/Saigai"));
                // Load every enemy sprite
                Sprites.enemies.Add(Content.Load<Texture2D>("AlienSprites/Type1"));
                Sprites.enemies.Add(Content.Load<Texture2D>("AlienSprites/Type2"));
                Sprites.enemies.Add(Content.Load<Texture2D>("AlienSprites/Type3"));
                Sprites.enemies.Add(Content.Load<Texture2D>("AlienSprites/Type4"));

                // Load every particle sprite
                Sprites.particles.Add(Content.Load<Texture2D>("Particles/TestParticle"));

                // Load every Powerbox sprite
                Sprites.powerboxes.Add(Content.Load<Texture2D>("Powerbox/Heal"));
                Sprites.powerboxes.Add(Content.Load<Texture2D>("Powerbox/Bullet"));
                Sprites.powerboxes.Add(Content.Load<Texture2D>("Powerbox/BulletSpeed"));
                Sprites.powerboxes.Add(Content.Load<Texture2D>("Powerbox/BulletSplit"));
                Sprites.powerboxes.Add(Content.Load<Texture2D>("Powerbox/Resistance"));
                Sprites.powerboxes.Add(Content.Load<Texture2D>("Powerbox/SheildBreaker"));

                // SFXs
                // Die
                SFX.deathSounds.Add(Content.Load<SoundEffect>("Sounds/SFX/Death"));
                
                // Shoot
                SFX.shootSounds.Add(Content.Load<SoundEffect>("Sounds/SFX/Shoot"));
                
                // Hit
                SFX.hitSounds.Add(Content.Load<SoundEffect>("Sounds/SFX/Hit"));
            }

            if (Globals.SaveData.Count == 0) {
                Globals.SaveData.Add("Wave", 0);
                Globals.SaveData.Add("BulletCount", 1);
                Globals.SaveData.Add("BulletSpeed", -5);
                Globals.SaveData.Add("BulletDamage", 0);
                Globals.SaveData.Add("Armour", 0);
                Globals.SaveData.Add("SplitBullet", 0);
            }

            if (Globals.easyDifficulty && Globals.SaveData.Count != 0) {
                wave = Globals.SaveData["Wave"];
                player.bulletArmour = Globals.SaveData["BulletDamage"];
                player.bulletSpeed = Globals.SaveData["BulletSpeed"];
                player.maxBullets = Globals.SaveData["BulletCount"];
                player.splitBullet = Globals.SaveData["SplitBullet"] == 1 ? true : false;
                player.sheild = Globals.SaveData["Armour"];
            }

            finishedLoading = true;

            if (Globals.debug) {
                wave = Globals.dbWave;
                CheckGameCondition();
            } else if (Globals.easyDifficulty) {
                wave--;
                CheckGameCondition();
            } else {
                newEnemyBatch();
            }
            previousInput = Keyboard.GetState();
        }

        void IScene.Update(GameTime gameTime) {
            input = Keyboard.GetState();

            worthyText--;

            if (!finishedLoading) {
                return;
            }

            if (gameState == GameState.Paused) {
                if (input.IsKeyDown(Controls.pause) && previousInput != input) {
                    gameState = GameState.Running;
                    previousInput = input;
                }
                previousInput = input;
                return;
            } else if (gameState == GameState.Running) {
                if (input.IsKeyDown(Controls.pause) && previousInput != input) {
                    gameState = GameState.Paused;
                    previousInput = input;
                }
            }

            CheckGameCondition();
            background.Update();

            // Has player died?
            if (player.health == 0) {
                LostUpdate();
                return;
            }

            UpdateEntities(gameTime);
            CheckCollision();

            CheckPowerbox();

            foreach (ParticleObject particleObject in particleObjects) {
                particleObject.Update();
            }

            previousInput = input;

            playerBulletDeleted = false;
            enemyBulletDeleted = false;
        }

        void CheckGameCondition() {
            if (Globals.summonSecondBoss) {
                aliens.Clear(); Alien.count = 0;
            }

            if (!Globals.easyDifficulty && Globals.isWorthy && previousWave != wave) {
                if (wave == 24) {
                    player.health = 3;
                    player.bulletSpeed = -9;
                    player.maxBullets = 1;
                    worthyText = 150;
                    worthyMessageNumber = 0;
                }
                if (wave == 49) {
                    player.health = 3;
                    player.bulletSpeed = -13;
                    player.maxBullets = 3;
                    worthyText = 150;
                    worthyMessageNumber = 1;
                }
                if (wave == 99) {
                    player.health = 3;
                    player.sheild = 1;
                    player.bulletSpeed = -13;
                    player.maxBullets = 3;
                    player.bulletArmour = 1;
                    worthyText = 150;
                    worthyMessageNumber = 2;
                }
                if (wave == 149) {
                    player.health = 3;
                    player.sheild = 1;
                    player.bulletSpeed = -17;
                    player.maxBullets = 5;
                    player.bulletArmour = 2;
                    worthyText = 150;
                    worthyMessageNumber = 3;
                }
                if (wave == 199) {
                    player.health = 3;
                    player.sheild = 2;
                    player.bulletArmour = 3;
                    worthyText = 150;
                    worthyMessageNumber = 4;
                }
                if (wave == 299) {
                    player.health = 3;
                    player.sheild = 5;
                    player.maxBullets = 5;
                    player.bulletArmour = 5;
                    worthyText = 150;
                }
                player.updateSprite();
            }
            previousWave = wave;

            if (Globals.easyDifficulty && (wave + 1) % 25 == 0) {
                Globals.SaveData["Wave"] = wave;
                Globals.SaveData["BulletCount"] = player.maxBullets;
                Globals.SaveData["BulletSpeed"] = player.bulletSpeed;
                Globals.SaveData["BulletDamage"] = player.bulletArmour;
                Globals.SaveData["Armour"] = player.sheild;
                Globals.SaveData["SplitBullet"] = player.splitBullet ? 1 : 0;
            }

            // Have all aliens been killed?
            if (Alien.count == 0 && !Globals.stopSpawn) {
                if (wave >= finalWave + (Globals.isWorthy ? 100 : 0) && currentBoss.Count == 0) {
                    won = true;
                } else {
                    wave++;
                    // Spawn a boss?

                    /*if (wave == 5) {
                        currentBoss = new Lilith(Sprites.bosses[3], wave);
                    }*/

                    if (wave == 4) {
                        currentBoss.Add(new Zhyron(Sprites.bosses[0], wave));
                    } else if (wave == 19) {
                        currentBoss.Add(new Seraphim(Sprites.bosses[1], wave));
                    } else if (wave == 39) {
                        currentBoss.Add(new Gabriel(Sprites.bosses[2], wave));
                    } else if (wave == 64) {
                        currentBoss.Add(new Lilith(Sprites.bosses[3], wave));
                    } else if (wave == 99) {
                        currentBoss.Add(new AdamAndEve(Sprites.bosses[4], wave));
                    } else if (wave == 119) {
                        int boss = rng.Next(0,4);
                        if (boss == 0) {
                            currentBoss.Add(new Zhyron(Sprites.bosses[0], wave));
                        } else if (boss == 1) {
                            currentBoss.Add(new Seraphim(Sprites.bosses[1], wave));
                        } else if (boss == 2) {
                            currentBoss.Add(new Gabriel(Sprites.bosses[2], wave));
                        } else if (boss == 3) {
                            currentBoss.Add(new Lilith(Sprites.bosses[3], wave));
                        }
                    } else if (wave == 159) {
                        int boss = rng.Next(0, 3);
                        if (boss == 0) {
                            currentBoss.Add(new Gabriel(Sprites.bosses[2], wave));
                        } else if (boss == 1) {
                            currentBoss.Add(new Lilith(Sprites.bosses[3], wave));
                        } else if (boss == 2) {
                            currentBoss.Add(new AdamAndEve(Sprites.bosses[4], wave));
                        }
                    } else if (wave == 199) {
                        currentBoss.Add(new TheMothership(Sprites.bosses[5], wave));
                    } else if (wave == 299) {
                        currentBoss.Add(new Saigai(Sprites.bosses[6], wave));
                    } else if (wave == 399) {
                        currentBoss.Add(new Zhyron(Sprites.bosses[5], wave + 1000));
                    }

                    // Most difficult enemy to spawn?
                    if (wave >= 9) {
                        highestAlienType = 3;
                    }
                    if (wave > 19) {
                        highestAlienType = 4;
                    }

                    if (Globals.summonSecondBoss) {
                        int boss = rng.Next(0, 5);
                        if (boss == 0) {
                            currentBoss.Add(new Zhyron(Sprites.bosses[0], wave));
                        } else if (boss == 1) {
                            currentBoss.Add(new Seraphim(Sprites.bosses[1], wave));
                        } else if (boss == 2) {
                            currentBoss.Add(new Gabriel(Sprites.bosses[2], wave));
                        } else if (boss == 3) {
                            currentBoss.Add(new Lilith(Sprites.bosses[3], wave));
                        } else if (boss == 4) {
                            currentBoss.Add(new AdamAndEve(Sprites.bosses[4], wave));
                        }
                        Globals.summonSecondBoss = false;
                    }

                    // Enemy spawning method
                    if (wave < 5) {
                        newEnemyBatch();
                    } else {
                        while (Alien.count == 0 && !Globals.stopSpawn) {
                            newRandomEnemyBatch(maxWaveHeight);
                        }
                    }
                }

            }


            if (wave >= 14) { // 15th wave start spawning "free" enemies.
                freeEnemySpawnTimer--;

                if (wave > 25) {
                    freeEnemySpawnTimer = 60 * 40;
                } else if (wave > 40) {
                    freeEnemySpawnTimer = 60 * 25;
                } else if (wave > 60) {
                    freeEnemySpawnTimer = 60 * 15;
                } else if (wave > 100) {
                    freeEnemySpawnTimer = 60 * 10;
                }

                if (freeEnemySpawnTimer == 0) {
                    int type = rng.Next(1, highestAlienType + 1);
                    int xPos = rng.Next(0, 7);

                    createEnemy(xPos, 0, type, free: true);

                    freeEnemySpawnTimer = 60 * 40;
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
            if (currentBoss.Count != 0) {
                currentBoss.Last().Update(player.position);
                if (currentBoss.Last().health <= 0) {
                    if (currentBoss.GetType() == typeof(Gabriel)) {
                        maxWaveHeight += 1;
                        minimumEnemy += 1;
                        baseEmpty += 2;
                    }

                    currentBoss.RemoveAt(currentBoss.Count - 1);

                    Globals.disableEnemyShooting = false;
                    Globals.invasionMode = false;
                    Globals.stopSpawn = false;
                }
            }

            // Powerbox
            foreach (Powerbox powerbox in powerboxes) {
                powerbox.Update();
            }
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
                        particleObjects.Add(new(15, 1, 2, 20, 4, bullet.position, new(0, 0), Sprites.particles[0]));
                        break;
                    }
                }

                // If not colliding with Alien, boss collision?
                if (removeBullet == false && currentBoss.Count != 0) {
                    if (currentBoss.Last().GetType() == typeof(AdamAndEve)) {
                        if (bullet.hitbox.Intersects(currentBoss.Last().adamHitbox)) {
                            currentBoss.Last().adamHealth -= bullet.damage;
                            particleObjects.Add(new(30, 2, 2, 40, 10, bullet.position, new(0, 0), Sprites.particles[0]));
                            removeBullet = true;
                        } else if (bullet.hitbox.Intersects(currentBoss.Last().eveHitbox)) {
                            currentBoss.Last().eveHealth -= bullet.damage;
                            particleObjects.Add(new(30, 2, 2, 40, 10, bullet.position, new(0, 0), Sprites.particles[0]));
                            removeBullet = true;
                        }
                    }
                    else {
                        if (bullet.hitbox.Intersects(currentBoss.Last().hitbox)) {
                            currentBoss.Last().health -= bullet.damage;
                            particleObjects.Add(new(30, 2, 2, 40, 10, bullet.position, new(0, 0), Sprites.particles[0]));
                            removeBullet = true;
                        }
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
                    } else if (powerbox.GetType() == typeof(ResistanceBox)) {
                        player.sheild += 1;
                        player.updateSprite();
                    } else if (powerbox.GetType() == typeof(SheildBreakerBox)) {
                        player.bulletArmour += 1;
                    }

                    powerboxes.Remove(powerboxes.Find(x => x.id == powerbox.id));
                    Globals.isWorthy = false;
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
                    
                    aliens.Last().sprite = Sprites.enemies[aliens.Last().type-1];
                    Alien.count++;
                    id++;
                }
            }
        }

        void newRandomEnemyBatch(int height = 4) {
            Random rng = new();
            for (int y = 0; y < height * (Globals.invasionMode ? 2 : 1); y++) {
                for (int x = 0; x < 8; x++) {
                    int chanceOfEmpty = baseEmpty + wave/finalWave + 20 * (Globals.invasionMode ? 1 : 0);
                    int type = rng.Next(minimumEnemy, highestAlienType+1 - (Globals.invasionMode ? 1 : 0) );

                    if (rng.Next(0,chanceOfEmpty) == 1) { continue; }

                    createEnemy(x,y,type);
                }
            }
        }

        void createEnemy(int xPos, int yPos, int type, bool free = false) {
            Vector2 spawnPos = new(16 + 16 * xPos, -16 - 16 * yPos);
            switch (type) {
                case 1:
                case 2:
                    aliens.Add(new BasicAlien(type,
                        spawnPos,//-64 + 16 * yPos),
                        new Rectangle(2, 2, 11, 11),
                        id));
                    break;
                case 3:
                    aliens.Add(new BoltAlien(type,
                    spawnPos, // new Vector2(16 + 16 * xPos, -64 + 16 * yPos),
                    new Rectangle(2, 2, 11, 11),
                    id));
                    break;
                case 4:
                    aliens.Add(new ShellShock(type,
                    spawnPos,
                    new Rectangle(2, 2, 11, 11),
                    id));
                    break;
            }

            aliens.Last().sprite = Sprites.enemies[aliens.Last().type - 1];
            Alien.count++;
            id++;
        }

        // Powerbox code
        void CheckPowerbox() {
            powerboxSummonTimer--;

            if (powerboxSummonTimer == 0) {
                powerboxSummonTimer = rng.Next(
                    Time.ToFrames(
                        seconds: 0,
                        minutes: 1),
                    Time.ToFrames(
                        seconds: 30,
                        minutes: 2)
                );

                int minUpgrade = 1;
                if (player.health == 3) {
                    minUpgrade += 1;
                } if (player.maxBullets > wave / 6) {
                    minUpgrade += 1;
                }

                int boxToSummon = rng.Next(minUpgrade,6);
                int boxXPos = rng.Next(3, 141);

                if (boxToSummon == 1) {
                    powerboxes.Add(new HealBox(
                        new(boxXPos, -16),
                        Sprites.powerboxes[0],
                        60,
                        id));
                } else if (boxToSummon == 2 && player.maxBullets < wave / 5) {
                    powerboxes.Add(new BulletBox(
                        new(boxXPos, -16),
                        Sprites.powerboxes[1],
                        20,
                        id));
                } else if (boxToSummon == 3 && player.maxBullets > 2 && !player.splitBullet) {
                    powerboxes.Add(new SplitBulletBox(
                        new(boxXPos, -16),
                        Sprites.powerboxes[3],
                        30,
                        id));
                } else if (boxToSummon == 4 && player.health > 1) {
                    powerboxes.Add(new ResistanceBox(
                        new(boxXPos, -16),
                        Sprites.powerboxes[4],
                        45,
                        id));
                } else if (boxToSummon == 5 && player.bulletArmour < wave / 50) {
                    powerboxes.Add(new SheildBreakerBox(
                        new(boxXPos, -24),
                        Sprites.powerboxes[5],
                        45,
                        id));
                } else if (player.bulletSpeed > - 5 - (wave / 4) && player.bulletSpeed > -16 ) {
                    powerboxes.Add(new BulletSpeedBox(
                        new(boxXPos, -16),
                        Sprites.powerboxes[2],
                        30,
                        id));
                }
                id++;
            } 
            
            foreach (Powerbox powerbox in powerboxes) {
                if (powerbox.position.Y > 192) {
                    powerboxes.Remove(powerboxes.Find(x => x.id == powerbox.id));
                    break;
                }
            }
        }


        // Code to spawn new bullet
        static internal void newPlayerBullet(int xPosition, Vector2 direction, int damage) {
            bullets.Add(new(position: new(xPosition, 160), new(2, 8), direction: direction, Sprites.bullets[0], id, damage));
            id++;
        }
        static internal void newEnemyBullet(Vector2 position, Vector2 direction, int type, bool bossBullet = false, int damage = 1) {
            if (!bossBullet) {
                enemyBullets.Add(new(
                    position: position,
                    hitboxSize: new(6, 6),
                    direction: direction,
                    Sprites.bullets[type],
                    id,
                    damage,
                    evil:true));
            } else {
                enemyBullets.Add(new(
                    position: position, 
                    hitboxSize: new (6, 6),
                    direction: direction,
                    Sprites.bossBullets[type], // Black magic here
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
            if (gameState == GameState.Paused) { return; }
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

            if (currentBoss.Count != 0) {
                currentBoss.Last().Draw(spriteBatch);
            }

            player.Draw(spriteBatch);

            if (won) {
                spriteBatch.DrawString(text, "YOU WON", new(0,0), Color.White);
            }
            foreach (ParticleObject particleObject in particleObjects) {
                particleObject.Draw(spriteBatch);
            }
        }
        void IScene.HighResDraw(SpriteBatch spriteBatch) {
            if (gameState == GameState.Paused) {
                spriteBatch.DrawString(text, "PAUSED", new(3,3), Color.White);
                return;
            }
            if (!won) {
                spriteBatch.DrawString(text, (player.health + player.sheild).ToString(), new(0, 0), Color.White);
                spriteBatch.DrawString(text, "Wave: " + (wave+1).ToString(), new(0, 38), Color.White);
                spriteBatch.DrawString(text, worthyMessages[worthyMessageNumber], new(5, 192*4-51), Color.White * worthyText);
            }

        }
    }
}
