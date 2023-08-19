using Raylib_cs;

namespace MonsterWorld.Transitions
{
    class FadeTransition : Transition
    {
        private Color _color;

        public FadeTransition(Color color, float duration = 1.0f)
            : base(duration)
        {
            _color = color;
        }

        public override void Draw()
        {
            Raylib.DrawRectangle(
                0,
                0,
                Raylib.GetScreenWidth(),
                Raylib.GetScreenHeight(),
                new Color(_color.r, _color.g, _color.b, (byte)(255 * Value))
            );
        }
    }
}
