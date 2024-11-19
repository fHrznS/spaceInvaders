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
