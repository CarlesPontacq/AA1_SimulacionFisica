using QuaternionUtility;
using UnityEngine;

public class MagnetPull : MonoBehaviour
{
    public JointArm endEffector;
    bool isMagneticPullOn = false;
    bool pendingAttach = false;

    public GameObject attachedGameObject;   // Objeto actualmente pegado
    VectorUtils3D localOffset;      // Offset de posición relativa
    QuaternionUtils localRotation;  // Rotación relativa

    void Start()
    {
        
    }

    void Update()
    {
        //Convertir la posición y rotación del EndEffector a tus tipos personalizados
        VectorUtils3D endEffectorPos = VectorUtils3D.ToVectorUtils3D(endEffector.transform.position);
        QuaternionUtils endEffectorRot = new QuaternionUtils();
        endEffectorRot.AssignFromUnityQuaternion(endEffector.transform.rotation);

        //Añadimos un offset hacia arriba para que no este pegado dentro del endEffector
        VectorUtils3D offset = new VectorUtils3D(0, endEffector.GetComponent<SphereCollider>().radius * 2, 0);
        VectorUtils3D rotatedOffset = endEffectorRot.Rotate(offset);
        VectorUtils3D magnetPos = endEffectorPos + rotatedOffset;

        //Aplicamos posición y rotación al objeto con MagnetPull
        transform.position = magnetPos.GetAsUnityVector();
        transform.rotation = endEffectorRot.GetAsUnityQuaternion();

        if(attachedGameObject != null)
        {
            // Posición relativa
            VectorUtils3D worldOffset = endEffectorRot.Rotate(localOffset);
            VectorUtils3D objectPos = magnetPos + worldOffset;

            QuaternionUtils worldRot = new QuaternionUtils();
            worldRot.AssignFromUnityQuaternion(transform.rotation);
            worldRot.Multiply(localRotation);

            attachedGameObject.transform.position = objectPos.GetAsUnityVector();
            attachedGameObject.transform.rotation = worldRot.ToUnityQuaternion();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        AttachObject(collision);
    }

    private void OnCollisionStay(Collision collision)
    {
        if (pendingAttach)
        {
            AttachObject(collision);
        }   
    }

    void AttachObject(Collision collision)
    {
        if (attachedGameObject == null && collision.gameObject.CompareTag("Objective") && isMagneticPullOn)
        {
            attachedGameObject = collision.gameObject;
            Transform targetTransform = attachedGameObject.transform;

            //Guardamos la posición y rotación relativas al imán
            VectorUtils3D magnetPos = VectorUtils3D.ToVectorUtils3D(transform.position);
            VectorUtils3D targetPos = VectorUtils3D.ToVectorUtils3D(targetTransform.position);
            localOffset = targetPos - magnetPos;

            localRotation = new QuaternionUtils();
            QuaternionUtils magnetRot = new QuaternionUtils();
            magnetRot.AssignFromUnityQuaternion(transform.rotation);

            QuaternionUtils targetRot = new QuaternionUtils();
            targetRot.AssignFromUnityQuaternion(targetTransform.rotation);

            //Guardamos la rotación relativa = rot_target * inverse(rot_imán)
            localRotation = targetRot.MultiplyWithInverse(magnetRot);

            //Desactivamos las físicas del objeto para que no interfiera
            Rigidbody rb = attachedGameObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = true;
                rb.useGravity = false;
            }
        }
    }
   
    private void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //Activamos o desactivamos el pulso magnetico
            isMagneticPullOn = !isMagneticPullOn;

            //Si esta activo el objeto puede pegarse
            if (isMagneticPullOn)
            {
                pendingAttach = true;
            }
            //Si no lo esta y el attachedGameObject es nulo se despega del objeto y restaura sus físicas
            else if (attachedGameObject != null)
            { 
                pendingAttach= false;
                Rigidbody rb = attachedGameObject.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.isKinematic = false;
                    rb.useGravity = true;
                }

                attachedGameObject = null;            
            }
        }
    }
}
