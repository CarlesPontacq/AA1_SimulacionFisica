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



    [SerializeField] float angleLimit = 50f; 
    float distanceToChild;

    // Guardamos el estado de rotación actual de cada eje en radianes
    float currentPitch = 0f;
    float currentYaw = 0f;
    float currentRoll = 0f;
    float limitAngleRad; // Límite de ángulo en radianes


    void Start()
    {
        
        // Guardar posici�n y rotaci�n inicial
        ownQuad.AssignFromUnityQuaternion(transform.rotation);
        ownTrans = VectorUtils3D.ToVectorUtils3D(transform.position);

        limitAngleRad = angleLimit * QuaternionUtils.Degree2Rad; 

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

     
        QuaternionUtils deltaRot = new QuaternionUtils(); // Quaternión de identidad

        if (isSelected)
        {
            // Velocidad de rotación en radianes
            float angleSpeed = 30f * Time.deltaTime * QuaternionUtils.Degree2Rad;


            // Rotación sobre el eje X LOCAL
            if (Input.GetKey(KeyCode.UpArrow))
            {
                if(currentPitch + angleSpeed <= limitAngleRad)
                {
                    deltaRot.Multiply(new QuaternionUtils().AngleToQuaternion(VectorUtils3D.right, angleSpeed));
                    currentPitch += angleSpeed;
                }
                
            }
            else if (Input.GetKey(KeyCode.DownArrow))
            {
                if (currentPitch - angleSpeed >= -limitAngleRad)
                {
                    deltaRot.Multiply(new QuaternionUtils().AngleToQuaternion(VectorUtils3D.right, -angleSpeed));
                    currentPitch -= angleSpeed;
                }
                
            }

            // Rotación sobre el eje Y LOCAL
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                if (currentPitch + angleSpeed <= limitAngleRad)
                {
                    deltaRot.Multiply(new QuaternionUtils().AngleToQuaternion(VectorUtils3D.up, angleSpeed));
                    currentPitch += angleSpeed;
                }
                
            }
            else if (Input.GetKey(KeyCode.RightArrow))
            {
                if (currentPitch - angleSpeed >= -limitAngleRad)
                {
                    deltaRot.Multiply(new QuaternionUtils().AngleToQuaternion(VectorUtils3D.up, -angleSpeed));
                    currentPitch -= angleSpeed;
                }
                
            }



            if(isEndEffector)
            {
                // Rotación sobre el eje Z LOCAL
                if (Input.GetKey(KeyCode.Q))
                {
                    if (currentPitch + angleSpeed <= limitAngleRad)
                    {
                        deltaRot.Multiply(new QuaternionUtils().AngleToQuaternion(VectorUtils3D.forward, angleSpeed));
                        currentPitch += angleSpeed;
                    }
                    
                }
                else if (Input.GetKey(KeyCode.E))
                {
                    if (currentPitch - angleSpeed >= -limitAngleRad)
                    {
                        deltaRot.Multiply(new QuaternionUtils().AngleToQuaternion(VectorUtils3D.forward, -angleSpeed));
                        currentPitch -= angleSpeed;
                    }
                    
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
            VectorUtils3D currentForwardDir = ownQuad.Rotate(VectorUtils3D.forward);

            // 3. Update the child's position
            // New position = Joint Position + (Forward Direction * Length)
            child.position = transform.position + currentForwardDir.GetAsUnityVector() * distanceToChild;
            lr.SetPosition(0, transform.position);
            lr.SetPosition(1, child.position);
        }
    }
}
