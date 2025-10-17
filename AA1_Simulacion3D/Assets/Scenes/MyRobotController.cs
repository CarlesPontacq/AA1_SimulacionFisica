using UnityEngine;

public class MyRobotController : MonoBehaviour
{
    [SerializeField] Transform[] joints;
    [SerializeField] Rigidbody robotRB;
    enum RotationType { SWING, TWIST };

    [SerializeField] float mainAcceleration = 1.5f;
    [SerializeField] float maxMainVelocity = 5f;
    [SerializeField] float mainAngularAcceleration = 0.1f;
    [SerializeField] float maxMainAngularVelocity = 0.5f;


    float angularVelocity = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 movementDirection = Vector3.zero;
        bool movingForward = false;
        bool movingBackward = false;

        if (Input.GetKey(KeyCode.W))
        {
            movementDirection = robotRB.transform.right;
            movingForward = true;
        }
        if (Input.GetKey(KeyCode.S))
        {
            movementDirection = -robotRB.transform.right;
            movingBackward = true;
        }

        bool tryingToRotate = false;
        float angleToRotate = angularVelocity;
        if (Input.GetKey(KeyCode.A) && (movingForward || movingBackward))
        {
            tryingToRotate = true;
            if (movingForward)
                angleToRotate = -angularVelocity;
            else
                angleToRotate = angularVelocity;
        }
        else if (Input.GetKey(KeyCode.D) && (movingForward || movingBackward))
        {
            tryingToRotate = true;
            if (movingForward)
                angleToRotate = angularVelocity;
            else
                angleToRotate = -angularVelocity;
        }

        robotRB.transform.Rotate(0, angleToRotate, 0);

        if (tryingToRotate)
        {
            Vector3 currentVelocity = robotRB.linearVelocity;
            robotRB.linearVelocity = Quaternion.Euler(0, angleToRotate, 0) * currentVelocity;

            angularVelocity += mainAngularAcceleration;
            if (angularVelocity > maxMainAngularVelocity)
                angularVelocity = maxMainAngularVelocity;
        }
        else
            angularVelocity = 0;

        robotRB.AddForce(movementDirection * mainAcceleration);
        if (robotRB.linearVelocity.magnitude > maxMainVelocity)
        {
            robotRB.linearVelocity = movementDirection * maxMainVelocity;
        }
    }

    void MoveJoint(int jointIndex, RotationType rotationType, float angle)
    {

    }
}
