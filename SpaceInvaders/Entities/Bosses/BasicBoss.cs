using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace SpaceInvaders.Entities.Bosses {
    internal abstract class BasicBoss {
        internal int
            health, maxHealth,
            attackTimer, attackTimerReset;
        internal Vector2 position;
        internal Rectangle hitbox;
        internal Point hitboxOffset;
        internal Texture2D sprite;
        internal bool disableEnemySpawning;
        internal Random rng = new();

        abstract internal void Update();
        abstract internal void Draw(SpriteBatch spriteBatch);
    }
}

// List of possible planned Bosses
/*
 Zhyron [DONE]

 Seraphim - One shot bullets, slow bullets.
 Gabriel - More enemies, low health, decent fire rate.
 Lilith - Bullet Hell
 Adam & Eve - 2 Bosses as 1
 The Mothership - Tons of HP, summons previous bosses.
 Calamitas - Survival boss

 Secret: Judgement - 1 shot bullet hell, tons of HP, summons old bosses.
*/