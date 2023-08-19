using MonsterWorld.Transitions;

namespace MonsterWorld
{
    class SceneManager
    {
        private static SceneManager _instance;

        private Scene _currentScreen;
        private Transition _currentTransition;

        private SceneManager()
        {

        }

        public static SceneManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new SceneManager();
                }

                return _instance;
            }
        }

        public void LoadScreen(Scene scene, Transition transition)
        {
            if (_currentTransition != null)
            {
                return;
            }

            _currentTransition = transition;
            _currentTransition.StateChanged += (sender, args) => LoadScreen(scene);
            _currentTransition.Completed += (sender, args) => _currentTransition = null;
        }

        public void LoadScreen(Scene scene)
        {
            _currentScreen?.Unload();

            scene.Load();

            _currentScreen = scene;
        }

        public void Update(float dt)
        {
            _currentScreen?.Update(dt);

            _currentTransition?.Update(dt);
        }

        public void Draw()
        {
            _currentScreen?.Draw();

            _currentTransition?.Draw();
        }
    }
}
