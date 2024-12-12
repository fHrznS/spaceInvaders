using System.Collections.Generic;
using System.Linq;

namespace SpaceInvaders.Utils {
    internal class SceneManager {
        Stack<IScene> scenes = new();

        public void AddScene(IScene scene) {
            scenes.Push(scene);
        }

        public bool RemoveScene() {
            scenes.Pop();

            if (scenes.Count == 0) {
                return false;
            }
            return true;
        }

        public IScene currentScene() {
            return scenes.Peek();
        }

        public IScene peekDownTo(int depth) {
            return scenes.Take(depth).Last();
        }
    }
}
