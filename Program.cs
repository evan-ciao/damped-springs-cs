using System.Numerics;
using Raylib_cs;

internal class Program
{
    public static void Main()
    {
        Vector2 resolution = new(800, 800);

        Raylib.InitWindow((int)resolution.X, (int)resolution.Y, "Damped springs");

        // 2D - circle
        Vector2 position = new (resolution.X / 2, resolution.Y / 2);
        Vector2 velocity = Vector2.Zero;

        Vector2 restPosition = position;

        // 3D - cube
        SimpleCube simpleCube = new(DrawVertex);
        float angle = 0;
        Quaternion restRotation = Quaternion.Identity;

        // camera
        Camera3D camera = new(
            new Vector3(0, 10, 10),
            new Vector3(0, 0, 0),
            new Vector3(0, 1, 0),
            50,
            CameraProjection.Perspective
        );

        // spring params
        float frequency = 40;
        float damping = 1.2f;

        while (!Raylib.WindowShouldClose())
        {
            float delta = Raylib.GetFrameTime();

            // input
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

            if (Raylib.IsKeyDown(KeyboardKey.Up))
                angle += 3.14f * delta;
            if (Raylib.IsKeyDown(KeyboardKey.Down))
                angle -= 3.14f * delta;
            
            if (MathF.Abs(angle) > 6.28f)
                angle = 0;
            restRotation = Quaternion.CreateFromAxisAngle(Vector3.UnitY, angle);
            //

            // damp circle position
            DampedSpring.CalculateDampedString
            (
                ref position,
                ref velocity,
                restPosition,
                delta,
                frequency,
                damping
            );

            // damp spring cube rotation
            DampedSpring.CalculateDampedString
            (
                ref simpleCube.rotation,
                ref simpleCube.angularVelocity,
                restRotation,
                delta,
                frequency,
                damping
            );
            
            // update simple cube
            simpleCube.UpdateRotation(delta);

            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.White);

            //Raylib.DrawCircle((int)position.X, (int)position.Y, 80, Color.Red);
            Raylib.DrawText($"frequency {frequency:F2}\ndamping {damping:F2}", 50, 50, 32, Color.Black);
            Raylib.DrawText($"use [Q, A] to change the frequency\nuse [W, S] to change the damping", 50, 130, 28, Color.Gray);

            Raylib.BeginMode3D(camera);

            simpleCube.Draw();

            Raylib.EndMode3D();

            Raylib.EndDrawing();
        }

        Raylib.CloseWindow();
    }

    private static void DrawVertex(Vector3 position)
    {
        Raylib.DrawSphere(position, 0.1f, Color.Red);
    }
}