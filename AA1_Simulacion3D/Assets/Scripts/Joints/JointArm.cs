using UnityEngine;
using QuaternionUtility;

public class JointArm : MonoBehaviour
{
    [SerializeField] bool isEndEffector;
    [SerializeField] Transform child;


    QuaternionUtils ownQuad = new QuaternionUtils();
    VectorUtils3D direction = new VectorUtils3D(); 

    float distanceToChild;
    
    void Start()
    {
        direction.x = transform.position.x;
        direction.y = transform.position.y;
        direction.z = transform.position.z;
        ownQuad.ToQuaternionUtils(transform.rotation);
    }

    void Update()
    {
        if(Input.GetKey(KeyCode.W))
        {
            // Quaternion incremental: 1° sobre Z local
            VectorUtils3D localZ = ownQuad.Rotate(VectorUtils3D.right);
            QuaternionUtils deltaRot = new QuaternionUtils().AngleToQuaternion(localZ, 1f * Time.deltaTime );

            // Aplicar rotación acumulativa
            ownQuad.Multiply(deltaRot);

            // Actualizar vector forward
            direction = ownQuad.Rotate(VectorUtils3D.forward);

            // Aplicar al transform
            transform.rotation = ownQuad.ToUnityQuaternion();

        }
        else if (Input.GetKey(KeyCode.S))
        {
            // Quaternion incremental: 1° sobre Z local
            VectorUtils3D localZ = ownQuad.Rotate(VectorUtils3D.right);
            QuaternionUtils deltaRot = new QuaternionUtils().AngleToQuaternion(localZ, -1f * Time.deltaTime);

            // Aplicar rotación acumulativa
            ownQuad.Multiply(deltaRot);

            // Actualizar vector forward
            direction = ownQuad.Rotate(VectorUtils3D.forward);

            // Aplicar al transform
            transform.rotation = ownQuad.ToUnityQuaternion();

        }
    }
}
