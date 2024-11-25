using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceInvaders.Entities;
using System;
using System.Collections.Generic;

namespace SpaceInvaders.Utils {
    public class ParticleObject {
        int particleId = 0;
        List<Particle> particles = new();
        List<int> particlesToDelete = new();
        Random rng = new();

        public ParticleObject(int count, int speed, int speedDeviation, int lifespan, int lifespanDeviation, Vector2 position, Vector2 gravity, Texture2D sprite) {
            int minSpeed = speed - speedDeviation * 2;
            int maxSpeed = speed + speedDeviation;

            int minLife = lifespan - lifespanDeviation;
            int maxLife = lifespan + lifespanDeviation;

            for (int i = 0; i < count; i++) {
                particles.Add(
                    new Particle(
                        rng.Next(minLife, maxLife+1),
                        startPos: position,
                        velocity: new Vector2(
                            (float)(rng.Next(minSpeed, maxSpeed) * rng.NextDouble()),
                            (float)(rng.Next(minSpeed, maxSpeed) * rng.NextDouble())),
                        gravity,
                        particleId,
                        sprite
                    )
                );
                particleId++;
            }
        }

        public void Update() {
            // Update each particle
            foreach (Particle particle in particles) {
                particle.Update();
                if (particle.lifespan == 0) {
                    particlesToDelete.Add(particle.id);
                }
            }

            // Remove any with a lifespan of 0
            if (particlesToDelete.Count != 0) {
                for (int i = 0; i < particlesToDelete.Count; i++) {
                    particles.Remove(particles.Find(x => x.id == particlesToDelete[i]));
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch) {
            foreach (Particle particle in particles) {
                particle.Draw(spriteBatch);
            }
        }
    }
    
    internal class Particle : BasicObject {
        internal int lifespan;
        private int maxLifespan;
        Vector2 velocity, gravity;

        public Particle(int life, Vector2 startPos, Vector2 velocity, Vector2 gravityDirection, int id, Texture2D texture) {
            this.id = id;
            lifespan = life < 0 ? life * -1 : life; // Ensure positive lifespan
            maxLifespan = lifespan;
            position = startPos;
            this.velocity = velocity;
            gravity = gravityDirection;
            sprite = texture;
        }

        internal void Update() {
            lifespan--;

            if (lifespan == 0) {
            }

            position += velocity;
            velocity -= gravity;
        }

        internal void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(sprite, position, Color.White * ((float)lifespan / (float)maxLifespan));
        }
    }
}
