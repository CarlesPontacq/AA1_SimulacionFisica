
public class VectorUtils2D
{
    public float x, y, z;
    private bool threeD;
    private float PI = 3.1416f;

    public VectorUtils2D(float x, float y)
    {
        this.x = x;
        this.y = y;

    }

    public static VectorUtils2D operator + (VectorUtils2D a , VectorUtils2D b){
        
        return new VectorUtils2D(a.x + b.x , a.y + b.y);
    }
    public static VectorUtils2D operator - (VectorUtils2D a , VectorUtils2D b){
        return new VectorUtils2D(a.x - b.x, a.y - b.y);

    }
    public static VectorUtils2D operator *(VectorUtils2D a, VectorUtils2D b)
    {
        return new VectorUtils2D(a.x * b.x, a.y * b.y);

    }


    public VectorUtils2D EscalarByProduct(float a)
    {        
        return new VectorUtils2D(this.x * a, this.y * a);

    }

    public float Magnitud()
    {
        return (float)System.MathF.Sqrt(x * x + y * y);
    }

    public VectorUtils2D Normalize()
    {
        float magnitude = Magnitud();
        if (magnitude == 0)
        {
            return new VectorUtils2D(0, 0);
        }

       
        return new VectorUtils2D(x / magnitude, y / magnitude);
    }

    public float DotProduct(VectorUtils2D a, VectorUtils2D b)
    {
        return a.x * b.x + a.y * b.y;
    }

    public float CrossProduct2D(VectorUtils2D a, VectorUtils2D b)
    {
        return a.x * b.y - a.y * b.x;
    }

    public float Angle(VectorUtils2D a, VectorUtils2D b)
    {
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

        return "(" + x + ", " + y + ")";
    }
}