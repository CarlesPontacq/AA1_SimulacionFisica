using UnityEngine;
using QuaternionUtility;
using System.Numerics;
using System;

public class JointArm : MonoBehaviour
{
    [SerializeField] bool isEndEffector;
    [SerializeField] public bool isSelected;
    [SerializeField] Transform child;
    [SerializeField] LineRenderer lr;


    QuaternionUtils ownQuad = new QuaternionUtils();
    VectorUtils3D direction = new VectorUtils3D();
    VectorUtils3D ownTrans = new VectorUtils3D();
    VectorUtils3D childTrans = new VectorUtils3D();
    VectorUtils3D initialDir = new VectorUtils3D();

    float distanceToChild;

    void Start()
    {
        
        // Guardar posici�n y rotaci�n inicial
        ownQuad.AssignFromUnityQuaternion(transform.rotation);
        ownTrans = VectorUtils3D.ToVectorUtils3D(transform.position);

        if (!isEndEffector && child != null)
        {
            childTrans = VectorUtils3D.ToVectorUtils3D(child.position);
            distanceToChild = VectorUtils3D.Distance(ownTrans, childTrans);
            initialDir = (childTrans - ownTrans).Normalize();
            lr = GetComponent<LineRenderer>();
            lr.positionCount = 2;
        }

    }

    void Update()
    {

        // Initialize a temporary rotation change variable
        QuaternionUtils deltaRot = new QuaternionUtils(); // Quaternión identidad (w=1) para este frame

        if (isSelected)
        {
            // Velocidad de rotación en radianes
            float angleSpeed = 30f * Time.deltaTime * QuaternionUtils.Degree2Rad;


            // Rotación Pitch (W/S) sobre el eje X LOCAL
            if (Input.GetKey(KeyCode.UpArrow))
            {
                deltaRot.Multiply(new QuaternionUtils().AngleToQuaternion(VectorUtils3D.right, angleSpeed));
            }
            else if (Input.GetKey(KeyCode.DownArrow))
            {
                deltaRot.Multiply(new QuaternionUtils().AngleToQuaternion(VectorUtils3D.right, -angleSpeed));
            }

            // Rotación Yaw (A/D) sobre el eje Y LOCAL
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                deltaRot.Multiply(new QuaternionUtils().AngleToQuaternion(VectorUtils3D.up, angleSpeed));
            }
            else if (Input.GetKey(KeyCode.RightArrow))
            {
                deltaRot.Multiply(new QuaternionUtils().AngleToQuaternion(VectorUtils3D.up, -angleSpeed));
            }



            if(isEndEffector)
            {
                // Rotación Yaw (E/Q) sobre el eje Z LOCAL
                if (Input.GetKey(KeyCode.Q))
                {
                    deltaRot.Multiply(new QuaternionUtils().AngleToQuaternion(VectorUtils3D.forward, angleSpeed));
                }
                else if (Input.GetKey(KeyCode.E))
                {
                    deltaRot.Multiply(new QuaternionUtils().AngleToQuaternion(VectorUtils3D.forward, -angleSpeed));
                }
            }
           

            // Aplicamos la rotación LOCAL al cuaternión principal.
            // La nueva orientación será la actual multiplicada por la rotación local de este frame.
            ownQuad.Multiply(deltaRot);
            ownQuad.Normalize();
        }

       // Update Transform and Child Position

        // 1. Apply the final rotation to the GameObject's Transform
        transform.rotation = ownQuad.ToUnityQuaternion();

        if (!isEndEffector && child != null)
        {
            // 2. Calculate the joint's current local forward direction
            // The transform.forward is calculated automatically by Unity based on transform.rotation
            // Here, we calculate it using our QuaternionUtils (which should match transform.rotation now)
            VectorUtils3D currentForwardDir = ownQuad.Rotate(VectorUtils3D.forward);

            // 3. Update the child's position
            // New position = Joint Position + (Forward Direction * Length)
            child.position = transform.position + currentForwardDir.GetAsUnityVector() * distanceToChild;
            lr.SetPosition(0, transform.position);
            lr.SetPosition(1, child.position);
        }
    }
}
