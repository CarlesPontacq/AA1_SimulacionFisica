using UnityEngine;

public class LibraryTest : MonoBehaviour
{
    VectorUtils test = new VectorUtils(2, 2, 2);
    VectorUtils test2 = new VectorUtils(1, 5, 12);

    void Start()
    {
        Debug.Log("Producto por escalar: " + test.EscalarByProduct(5).ToString());
        Debug.Log("Magnitud: " + test.Magnitud());
        Debug.Log("Producto escalar: " + test.DotProduct(test, test2));
        Debug.Log("Producto vectorial: " + test.CrossProduct3D(test, test2).ToString());
        Debug.Log("Angulo: " + test.Angle(test, test2));
    }

    void Update()
    {

    }
}
