using UnityEngine;

public class MyRobotController : MonoBehaviour
{
    [SerializeField] Transform[] joints;
    [SerializeField] Rigidbody robotRB;
    enum RotationType { SWING, TWIST };

    [SerializeField] float mainAcceleration = 1.5f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            robotRB.AddForce(mainAcceleration, 0,0);
        }
        if (Input.GetKey(KeyCode.S))
        {
            robotRB.AddForce(-mainAcceleration, 0, 0);
        }
    }

    void MoveJoint(int jointIndex, RotationType rotationType, float angle)
    {

    }
}
