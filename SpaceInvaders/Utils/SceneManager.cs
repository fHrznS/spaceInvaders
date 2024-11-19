using System.Collections.Generic;

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
    }
}
