using UnityEngine;
using QuaternionUtility;
using Unity.VisualScripting.FullSerializer.Internal;

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
        if (tryingToRotate)
        {
            VectorUtils3D localY = currentRotation.Rotate(VectorUtils3D.up);
            QuaternionUtils deltaRot = new QuaternionUtils().AngleToQuaternion(localY, angleToRotate * Time.fixedDeltaTime);
            currentRotation.Multiply(deltaRot);
            currentRotation.Normalize();
            transform.rotation = currentRotation.ToUnityQuaternion();
        }

        VectorUtils3D currentVel = VectorUtils3D.ToVectorUtils3D(robotRB.linearVelocity);
        float speed = currentVel.Magnitude();

        if (speed > 0.001f)
        {
            VectorUtils3D forwardDir = currentRotation.Rotate(VectorUtils3D.right);

            float dirSign = Mathf.Sign(forwardDir.DotProduct(currentVel));

            VectorUtils3D redirectedVelocity = forwardDir.Normalize() * speed * dirSign;
            robotRB.linearVelocity = redirectedVelocity.GetAsUnityVector();
        }

        if (movingForward || movingBackwards)
        {
            VectorUtils3D forwardDir = currentRotation.Rotate(VectorUtils3D.right);
            float dirMultiplier = movingForward ? 1f : -1f;

            VectorUtils3D resultingForce = forwardDir * mainAcceleration * dirMultiplier;
            robotRB.AddForce(resultingForce.GetAsUnityVector(), ForceMode.Acceleration);

            currentVelocity.AssignFromUnityVector(robotRB.linearVelocity);
            if (currentVelocity.Magnitude() > maxMainVelocity)
            {
                VectorUtils3D clampedVelocity = forwardDir * maxMainVelocity * dirMultiplier;
                robotRB.linearVelocity = clampedVelocity.GetAsUnityVector();
            }
        }
    }



    void MoveJoint(int jointIndex, RotationType rotationType, float angle)
    {

    }
}
