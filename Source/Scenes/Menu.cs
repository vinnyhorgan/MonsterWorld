using System.Numerics;
using MonoGame.Extended.Tweening;
using MonsterWorld.Transitions;
using Raylib_cs;

namespace MonsterWorld.Scenes
{
    class MonsterSprite
    {
        public Rectangle Frame;
        public Vector2 Position;
    }

    class Menu : Scene
    {
        private Texture2D _characters;
        private MonsterSprite _monster;
        private Tweener _tweenIn = new();
        private Tweener _tweenOut = new();

        public override void Load()
        {
            _characters = AssetManager.Instance.GetTexture("characters.png");

            _monster = new MonsterSprite
            {
                Frame = new Rectangle(0, 16, 32, 32),
                Position = new Vector2(GameWidth + 100, 120)
            };

            _tweenIn.TweenTo(_monster, _monster => _monster.Position, new Vector2(GameWidth / 2 - 16, 120), 1, 0).RepeatForever(5);
            _tweenOut.TweenTo(_monster, _monster => _monster.Position, new Vector2(-100, 120), 1, 4)
                .RepeatForever(5)
                .OnEnd((tween) => {
                    _monster.Position = new Vector2(GameWidth + 100, 120);
                    _monster.Frame.x += 32;

                    if (_monster.Frame.x == 32 * 4)
                    {
                        _monster.Frame.x = 0;
                        _monster.Frame.y += 32;
                    }

                    if (_monster.Frame.y == 16 + 32 * 2)
                    {
                        _monster.Frame.y = 16;
                    }
                });
        }

        public override void Update(float dt)
        {
            _tweenIn.Update(dt);
            _tweenOut.Update(dt);

            if (Raylib.IsKeyPressed(KeyboardKey.KEY_SPACE))
            {
                SceneManager.Instance.LoadScreen(new World(), new FadeTransition(Color.BLACK));
            }
        }

        public override void Draw()
        {
            Raylib.ClearBackground(Color.LIGHTGRAY);

            Raylib.DrawText("Monster World", GameWidth / 2 - Raylib.MeasureText("Monster World", 20) / 2, 25, 20, Color.BLACK);

            Raylib.DrawEllipse(GameWidth / 2, GameHeight / 2 + 20, 50, 25, Color.LIME);

            Raylib.DrawText("Press SPACE to start", GameWidth / 2 - Raylib.MeasureText("Press SPACE to start", 5) / 2, GameHeight / 2 + 80, 5, Color.BLACK);

            Raylib.DrawTextureRec(_characters, _monster.Frame, _monster.Position, Color.WHITE);
        }
    }
}
