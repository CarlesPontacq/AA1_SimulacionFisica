
using UnityEngine.UIElements;

public class QuaternionUtils 
{
    float w;
    float i;
    float j;
    float k;

    QuaternionUtils(float w ,float i, float j, float k)
    {
        this.w = w;
        this.i = i;
        this.j = j;
        this.k = k;
    }

    QuaternionUtils()
    {
        this.w = 0;
        this.i = 0;
        this.j = 0;
        this.k = 0;
    }

    /// <summary>
    /// THIS PASSES A CUATERNION AND A VECTOR AND RETURNS A QUATERNION ALREADY ANGLED
    /// </summary>
    /// <param name="v"></param>
    /// <param name="angle"></param>
    /// <returns></returns>

    QuaternionUtils AngleToQuaternion(VectorUtils3D v,float angle)
    {
       QuaternionUtils a =  new QuaternionUtils(0f,0f,0f,0f);
        

        a.w = System.MathF.Cos(angle/2);
        float c = System.MathF.Sin(angle/2);

        a.i = c * v.x;
        a.j = c * v.y;
        a.k = c * v.z;


        return a;
    }

    float QuaternionToAngle(QuaternionUtils a)
    {
        VectorUtils3D v = new VectorUtils3D(0f,0f,0f);
        float angle = 2.0f * System.MathF.Acos(a.w);
        float divider = System.MathF.Sqrt(1.0f - a.w * a.w);

        if (divider != 0.0)
        {
            // Calculate the axis
            v.x = a.i / divider;
            v.y = a.j / divider;
            v.z = a.k / divider;
        }
        else
        {
            // Arbitrary normalized axis
            v.x = 1;
            v.y = 0;
            v.z = 0;
        }

        return angle;

    }

    void QuaternionPrint()
    {
        UnityEngine.Debug.Log("w : "+ w + ", i : " + i + ", j : "+j + " , k : " + k);
            
    }

}
