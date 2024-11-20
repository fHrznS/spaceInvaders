using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceInvaders.Entities.Bosses {
    internal abstract class BasicBoss {
        internal int
            health, maxHealth,
            attackTimer, attackTimerReset;
        internal Vector2 position;
        internal Rectangle hitbox;
        internal Texture2D sprite;
        internal bool disableEnemySpawning;

        abstract internal void Update();
        abstract internal void Draw(SpriteBatch spriteBatch);
    }
}

// List of possible planned Bosses
/*
 Zhyron [DONE]

 Gabriel - More enemies, low health, decent fire rate.
 Seraphim - One shot bullets, slow bullets.
 Lilith - Bullet Hell
 Adam & Eve - 2 Bosses as 1
 The Mothership - Tons of HP, summons previous bosses.
 Calamitas - Survival boss

 Secret: Judgement - 1 shot bullet hell, tons of HP, summons old bosses.
*/