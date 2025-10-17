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

    VectorUtils3D currentPosition = new VectorUtils3D();
    QuaternionUtils currentRotation = new QuaternionUtils();
    VectorUtils3D currentVelocity = new VectorUtils3D();
    
    float angularVelocity = 0;

    // Variables received from input
    VectorUtils3D movementDirection = new VectorUtils3D();
    bool movingForward = false;
    bool movingBackwards = false;

    bool tryingToRotate = false;
    float angleToRotate = 0;

    // Update is called once per frame
    void Update()
    {
        UpdatePhysicsVariables();
        RegisterInputs();
    }

    private void FixedUpdate()
    {
        MoveRobotBody();
    }

    void RegisterInputs()
    {
        movementDirection = new VectorUtils3D();
        movingForward = false;
        movingBackwards = false;

        if (Input.GetKey(KeyCode.W))
        {
            movementDirection.AssignFromUnityVector(robotRB.transform.right);
            movingForward = true;
        }
        if (Input.GetKey(KeyCode.S))
        {
            movementDirection.AssignFromUnityVector(-robotRB.transform.right);
            movingBackwards = true;
        }

        tryingToRotate = false;
        angleToRotate = angularVelocity;
        if (Input.GetKey(KeyCode.A) && (movingForward || movingBackwards))
        {
            tryingToRotate = true;
            if (movingForward)
                angleToRotate = -mainAngularAcceleration;
            else
                angleToRotate = mainAngularAcceleration;
        }
        else if (Input.GetKey(KeyCode.D) && (movingForward || movingBackwards))
        {
            tryingToRotate = true;
            if (movingForward)
                angleToRotate = mainAngularAcceleration;
            else
                angleToRotate = -mainAngularAcceleration;
        }
    }

    void UpdatePhysicsVariables()
    {
        currentPosition.AssignFromUnityVector(transform.position);
        currentRotation.AssignFromUnityQuaternion(transform.rotation);
        currentVelocity.AssignFromUnityVector(robotRB.linearVelocity);
    }

    void MoveRobotBody()
    {
        VectorUtils3D rotationToMake = new VectorUtils3D(0, angleToRotate, 0);
        if (angleToRotate > 0)
        {
            Debug.Log("deberia rotar");
        }
        Debug.Log(rotationToMake.ToString());
        currentRotation.QuaternionRotate(rotationToMake);

        if (tryingToRotate)
        {
            currentVelocity = currentRotation.Quaternion_toEulerZYX(currentRotation) * currentVelocity;
            angularVelocity += mainAngularAcceleration;
            if (angularVelocity > maxMainAngularVelocity)
                angularVelocity = maxMainAngularVelocity;
        }
        else
            angularVelocity = 0;
        

        if (movementDirection.Magnitude() > 0.01f)
        {
            Debug.Log("deberia moverse");
        }

        VectorUtils3D resultingForce = new VectorUtils3D();
        resultingForce = movementDirection * mainAcceleration;
        robotRB.AddForce(resultingForce.GetAsUnityVector());

        
        currentVelocity.AssignFromUnityVector(robotRB.linearVelocity);
        if (currentVelocity.Magnitude() > maxMainVelocity)
        {
            VectorUtils3D maxVelocity = movementDirection * maxMainVelocity;
            currentVelocity = maxVelocity;
        }
        robotRB.linearVelocity = currentVelocity.GetAsUnityVector();
        
    }

    void MoveJoint(int jointIndex, RotationType rotationType, float angle)
    {

    }
}
