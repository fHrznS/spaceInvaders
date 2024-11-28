using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace SpaceInvaders.Entities.Bosses {
    /// <summary>
    /// This makes the grounds for every boss.
    /// It has all boss-unique variables, such as health and enemy spawning disabling if the attack calls for it.
    /// </summary>
   
    internal abstract class BasicBoss : BasicObject {
        internal int
            health, maxHealth,
            attackTimer, attackTimerReset;
        internal Point hitboxOffset;
        internal bool disableEnemySpawning;
        internal Random rng = new();
        internal Rectangle sourceRect = new(0, 0, 80, 32);
        internal int nextFrameTimer = 60;
        internal int onFrame = 0;

        abstract internal void Update();
        abstract internal void Draw(SpriteBatch spriteBatch);
    }
}

// List of planned Bosses
/*
 Zhyron [DONE]

 Seraphim - One shot bullets, slow bullets. [DONE]
 Gabriel - More enemies, low health, decent fire rate. [Untested]
 Lilith - Bullet Hell
 Adam & Eve - 2 Bosses as 1
 The Mothership - Tons of HP, summons previous bosses.
 Calamitas - Survival boss

 Secret: Judgement - 1 shot bullet hell, tons of HP, summons old bosses.
*/