using System.Numerics;
using Raylib_cs;

class Program
{
    public static void Main()
    {
        Vector2 resolution = new(1280, 1280);

        Raylib.InitWindow((int)resolution.X, (int)resolution.Y, "Damped springs");

        Vector2 position = new (resolution.X / 2, resolution.Y / 2);
        Vector2 velocity = Vector2.Zero;

        Vector2 restPosition = position;

        float frequency = 20;
        float damping = 0.9f;

        while (!Raylib.WindowShouldClose())
        {
            float delta = Raylib.GetFrameTime();

            if (Raylib.IsMouseButtonDown(MouseButton.Left))
            {
                restPosition = Raylib.GetMousePosition();
            }

            if (Raylib.IsKeyDown(KeyboardKey.Q))
                frequency += 3 * delta;
            if (Raylib.IsKeyDown(KeyboardKey.A))
                frequency -= 3 * delta;
            if (Raylib.IsKeyDown(KeyboardKey.W))
                damping += 0.1f * delta;
            if (Raylib.IsKeyDown(KeyboardKey.S))
                damping -= 0.1f * delta;

            DampedSpring.CalculateDampedString
            (
                ref position,
                ref velocity,
                restPosition,
                delta,
                frequency,
                damping
            );

            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.White);

            Raylib.DrawCircle((int)position.X, (int)position.Y, 80, Color.Red);
            Raylib.DrawText($"frequency {frequency:F2}\ndamping {damping:F2}", 50, 50, 32, Color.Black);
            Raylib.DrawText($"use [Q, A] to change the frequency\nuse [W, S] to change the damping", 50, 130, 28, Color.Gray);

            Raylib.EndDrawing();
        }

        Raylib.CloseWindow();
    }
}