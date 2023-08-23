using System.Numerics;
using Raylib_cs;

namespace MonsterWorld
{
    class Scene
    {
        public int GameWidth
        {
            get { return Program.GameWidth; }
        }

        public int GameHeight
        {
            get { return Program.GameHeight; }
        }

        public Vector2 Mouse
        {
            get { return Program.Mouse; }
        }

        public virtual void Load() {}

        public virtual void Update(float dt) {}

        public virtual void Draw() {}

        public virtual void Unload() {}
    }
}
