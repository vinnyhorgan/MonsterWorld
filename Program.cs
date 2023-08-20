using System;
using System.Numerics;
using Raylib_cs;

namespace MonsterWorld
{
    class Program
    {
        public static int GameWidth { get; private set; } = 480;
        public static int GameHeight { get; private set; } = 270;
        public static Vector2 Mouse { get; private set; }

        static void Main(string[] args)
        {
            Logger.Init(true);

            Raylib.SetConfigFlags(ConfigFlags.FLAG_WINDOW_RESIZABLE);
            Raylib.InitWindow(GameWidth * 3, GameHeight * 3, "Monster World");
            Raylib.SetWindowMinSize(GameWidth, GameHeight);
            Raylib.SetExitKey(KeyboardKey.KEY_NULL);
            Raylib.SetTargetFPS(Raylib.GetMonitorRefreshRate(Raylib.GetCurrentMonitor()));

            Raylib.InitAudioDevice();

            RenderTexture2D target = Raylib.LoadRenderTexture(GameWidth, GameHeight);
            Raylib.SetTextureFilter(target.texture, TextureFilter.TEXTURE_FILTER_POINT);

            Raylib.SetWindowIcon(Raylib.LoadImageFromTexture(AssetManager.Instance.GetTexture("m.png")));

            SceneManager.Instance.LoadScreen(new Scenes.Menu());

            while (!Raylib.WindowShouldClose())
            {
                var scale = MathF.Min(
                    (float)Raylib.GetScreenWidth() / GameWidth,
                    (float)Raylib.GetScreenHeight() / GameHeight
                );

                var mouse = Raylib.GetMousePosition();
                var virtualMouse = Vector2.Zero;
                virtualMouse.X = (mouse.X - (Raylib.GetScreenWidth() - (GameWidth * scale)) * 0.5f) / scale;
                virtualMouse.Y = (mouse.Y - (Raylib.GetScreenHeight() - (GameHeight * scale)) * 0.5f) / scale;

                var max = new Vector2(GameWidth, GameHeight);
                virtualMouse = Vector2.Clamp(virtualMouse, Vector2.Zero, max);

                Mouse = virtualMouse;

                var dt = Raylib.GetFrameTime();

                SceneManager.Instance.Update(dt);

                Raylib.BeginDrawing();

                Raylib.ClearBackground(Color.BLACK);

                Raylib.BeginTextureMode(target);

                Raylib.ClearBackground(Color.BLACK);

                SceneManager.Instance.Draw();

                Raylib.EndTextureMode();

                var sourceRec = new Rectangle(
                    0.0f,
                    0.0f,
                    target.texture.width,
                    -target.texture.height
                );

                var destRec = new Rectangle(
                    (Raylib.GetScreenWidth() - (GameWidth * scale)) * 0.5f,
                    (Raylib.GetScreenHeight() - (GameHeight * scale)) * 0.5f,
                    GameWidth * scale,
                    GameHeight * scale
                );

                Raylib.DrawTexturePro(target.texture, sourceRec, destRec, new Vector2(0, 0), 0.0f, Color.WHITE);

                Raylib.EndDrawing();
            }

            Raylib.UnloadRenderTexture(target);

            Raylib.CloseAudioDevice();

            Raylib.CloseWindow();
        }
    }
}
