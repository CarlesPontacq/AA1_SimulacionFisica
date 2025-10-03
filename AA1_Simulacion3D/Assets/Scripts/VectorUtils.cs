
public class VectorUtils
{
    public float x, y, z;
    private bool threeD;
    private float PI = 3.1416f;

    public VectorUtils(float x, float y)
    {
        this.x = x;
        this.y = y;
        threeD = false;
    }

    public VectorUtils(float x, float y, float z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
        threeD = true;
    }

    public static VectorUtils operator + (VectorUtils a , VectorUtils b){
        
        return new VectorUtils(a.x + b.x , a.y + b.y);
    }
    public static VectorUtils operator - (VectorUtils a , VectorUtils b){
        return new VectorUtils(a.x - b.x, a.y - b.y);

    }

    public VectorUtils VectorAdditionSubtractioin(VectorUtils a, VectorUtils b, bool add)
    {
        if (threeD)
        {
            if (add)
            {
                return new VectorUtils(a.x + b.x, a.y + b.y, a.z + b.z);
            }
            
            return new VectorUtils(a.x - b.x, a.y - b.y, a.z - b.z);
            
        }

        if (add)
        {
            return new VectorUtils(a.x + b.x, a.y + b.y);
        }

        return new VectorUtils(a.x - b.x, a.y - b.y);
    }

    public VectorUtils EscalarByProduct(float a)
    {
        if (threeD) { 
            return new VectorUtils(this.x * a, this.y * a, this.z * a);
        }
        
        return new VectorUtils(this.x * a, this.y * a);

    }

    public float Magnitud()
    {
        if (threeD)
        {
            return (float)System.MathF.Sqrt(x * x + y * y + z * z);
        }
        return (float)System.MathF.Sqrt(x * x + y * y);
    }

    public VectorUtils Normalize()
    {
        float magnitude = Magnitud();
        if (magnitude == 0)
        {
            if(magnitude == 0 && !threeD)
            {
                return new VectorUtils(0, 0);
            }

            return new VectorUtils(0, 0, 0);
        }

        if (threeD)
        {
            return new VectorUtils(x / magnitude, y / magnitude, z / magnitude);
        }
        return new VectorUtils(x / magnitude, y / magnitude);
    }

    public float DotProduct(VectorUtils a, VectorUtils b)
    {
        if (!CheckDimensions(a, b))
        {
            return 0;
        }

        if (threeD)
        {
            return a.x * b.x + a.y * b.y + a.z * b.z;
        }
        return a.x * b.x + a.y * b.y;
    }

    public VectorUtils CrossProduct3D(VectorUtils a, VectorUtils b)
    {
        if (!CheckDimensions(a, b))
        {
            return new VectorUtils(0, 0, 0);
        }

        float newX = a.y * b.z - a.z * b.y;
        float newY = a.z * b.x - a.x * b.z;
        float newZ = a.x * b.y - a.y * b.x;

        return new VectorUtils(newX, newY, newZ);
    }

    public float CrossProduct2D(VectorUtils a, VectorUtils b)
    {
        if (!CheckDimensions(a, b))
        {
            return 0;
        }

        return a.x * b.y - a.y * b.x;
    }

    public float Angle(VectorUtils a, VectorUtils b)
    {
        if(!CheckDimensions(a, b))
        {
            return 0;
        }

        float dot = DotProduct(a, b);
        float magnitudA = a.Magnitud();
        float magnitudB = b.Magnitud();

        if (magnitudA == 0 || magnitudB == 0)
        {
            return 0;
        }

        return (float)System.MathF.Acos(dot / (magnitudA * magnitudB)) * (180f / PI);
    }

    public string ToString()
    {
        if (threeD)
        {
            return "(" + x + ", " + y + ", " + z + ")";
        }
        return "(" + x + ", " + y + ")";
    }

    private bool CheckDimensions(VectorUtils a, VectorUtils b)
    {
        if (a.threeD != b.threeD)
        {
            UnityEngine.Debug.Log("No tienen la misma dimension por lo que no puede hacer el calculo");
        }
        
        return a.threeD && b.threeD;
    }
}