using System.Numerics;

class SimpleCube
{
    public Vector3[] vertices = new Vector3[8];
    private DrawVertexDelegate _drawVertexCallback;

    public Vector3 position = Vector3.Zero;

    public Quaternion rotation = Quaternion.Identity;
    public Vector3 angularVelocity = Vector3.One;

    public SimpleCube(DrawVertexDelegate drawVertexCallback)
    {
        vertices[0] = new Vector3 { X = -1, Y = -1, Z = -1 };
        vertices[1] = new Vector3 { X = 1, Y = -1, Z = -1 };
        vertices[2] = new Vector3 { X = -1, Y = 1, Z = -1 };
        vertices[3] = new Vector3 { X = 1, Y = 1, Z = -1 };

        vertices[4] = new Vector3 { X = -1, Y = -1, Z = 1 };
        vertices[5] = new Vector3 { X = 1, Y = -1, Z = 1 };
        vertices[6] = new Vector3 { X = -1, Y = 1, Z = 1 };
        vertices[7] = new Vector3 { X = 1, Y = 1, Z = 1 };

        _drawVertexCallback = drawVertexCallback;
    }

    public void UpdateRotation(float delta)
    {
        float angularSpeed = angularVelocity.Length();
        Vector3 angularRotationAxis = angularVelocity / angularSpeed;

        float angle = angularSpeed * delta;

        rotation = Quaternion.Multiply(
            rotation,
            Quaternion.CreateFromAxisAngle(
                angularRotationAxis, angle
            )
        );
    }

    public delegate void DrawVertexDelegate(Vector3 position);
    public void Draw()
    {
        foreach (var vertex in vertices)
        {   
            _drawVertexCallback(
                TransformVertex(vertex, position, rotation)
            );
        }
    }

    public Vector3 TransformVertex(Vector3 vertex, Vector3 position, Quaternion rotation)
    {
        Quaternion quaternionVertex = new Quaternion { W = 0, X = vertex.X, Y = vertex.Y, Z = vertex.Z };
        var result = Quaternion.Multiply(Quaternion.Multiply(rotation, quaternionVertex), Quaternion.Inverse(rotation));

        return new Vector3 { X = result.X + position.X, Y = result.Y + position.Y, Z = result.Z + position.Z };
    }
}