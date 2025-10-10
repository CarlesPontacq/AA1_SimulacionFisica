
public class VectorUtils3D
{
    public float x, y, z;
    public float PI = 3.14159265359f;

    public VectorUtils3D(float x, float y, float z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    public VectorUtils3D()
    {
        this.x = 0;
        this.y = 0;
        this.z = 0;
    }

    public static VectorUtils3D operator + (VectorUtils3D a , VectorUtils3D b){
        
        return new VectorUtils3D(a.x + b.x , a.y + b.y, a.z + b.z);
    }
    public static VectorUtils3D operator - (VectorUtils3D a , VectorUtils3D b){
        return new VectorUtils3D(a.x - b.x, a.y - b.y, a.z - b.z);

    }

    public static VectorUtils3D operator *(VectorUtils3D a, VectorUtils3D b)
    {
        return new VectorUtils3D(a.x * b.x, a.y * b.y, a.z * b.z);

    }

    public VectorUtils3D EscalarByProduct(float a)
    {
       
       return new VectorUtils3D(this.x * a, this.y * a, this.z * a);
      
        
        

    }

    public float Magnitud()
    {
        return (float)System.MathF.Sqrt(x * x + y * y + z * z);
    }

    public VectorUtils3D Normalize()
    {
        float magnitude = Magnitud();
        if (magnitude == 0)
        {
            return new VectorUtils3D(0, 0, 0);
        }

        return new VectorUtils3D(x / magnitude, y / magnitude, z / magnitude);

    }

    public float DotProduct(VectorUtils3D a, VectorUtils3D b)
    {
        
        return a.x * b.x + a.y * b.y + a.z * b.z;
        
       
    }

    public VectorUtils3D CrossProduct3D(VectorUtils3D a, VectorUtils3D b)
    {
        float newX = a.y * b.z - a.z * b.y;
        float newY = a.z * b.x - a.x * b.z;
        float newZ = a.x * b.y - a.y * b.x;

        return new VectorUtils3D(newX, newY, newZ);
    }

    public float Angle(VectorUtils3D a, VectorUtils3D b)
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
        
            return "(" + x + ", " + y + ", " + z + ")";
        
        
    }


}