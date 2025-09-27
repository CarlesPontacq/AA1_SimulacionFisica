using UnityEngine;

public class VectorUtils : MonoBehaviour
{
    public float x, y, z;

    public VectorUtils(float x, float y)
    {
        this.x = x;
        this.y = y;
        this.z = 0;
    }

    public VectorUtils(float x, float y, float z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    public VectorUtils EscalarByProduct(float a)
    {
        return new VectorUtils(this.x * a, this.y * a, this.z * a);
    }

    public float Magnitud()
    {
        return (float) Mathf.Sqrt(x * x + y * y + z * z);
    }

    public VectorUtils Normalize()
    {
        float magnitude = Magnitud();
        if (magnitude == 0)
        {
            return new VectorUtils(0, 0, 0);
        }
        
        return new VectorUtils(x / magnitude, y / magnitude, z / magnitude);
    }

    public float DotProduct(VectorUtils a, VectorUtils b)
    {
        return a.x * b.x + a.y * b.y + a.z * b.z;
    }

    public VectorUtils CrossProduct3D(VectorUtils a, VectorUtils b)
    {
        float newX = a.y * b.z - a.z * b.y;
        float newY = a.z * b.x - a.x * b.z;
        float newZ = a.x * b.y - a.y * b.x;
        
        return new VectorUtils(newX, newY, newZ);
    }

    public float CrossProduct2D(VectorUtils a, VectorUtils b)
    {
        return a.x * b.y - a.y * b.x;
    }

    public float Angle(VectorUtils a, VectorUtils b)
    {
        float dot = DotProduct(a, b);
        float magnitudA = a.Magnitud();
        float magnitudB = b.Magnitud();

        if(magnitudA == 0 || magnitudB == 0)
        {
            return 0;
        }

        return (float)Mathf.Acos(dot / (magnitudA * magnitudB)) * (180f / (float)Mathf.PI);
    }

    public override string ToString()
    {
        return "(" + x + ", " + y + ", " + z + ")";
    }
}