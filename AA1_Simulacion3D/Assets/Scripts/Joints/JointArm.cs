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

    //Rotacion Inicial
    QuaternionUtils initialQuad = new QuaternionUtils();



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
        initialQuad.AssignFromUnityQuaternion(transform.rotation);

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
                
                deltaRot.Multiply(new QuaternionUtils().AngleToQuaternion(VectorUtils3D.right, angleSpeed));

                
                
            }
            else if (Input.GetKey(KeyCode.DownArrow))
            {
                
                deltaRot.Multiply(new QuaternionUtils().AngleToQuaternion(VectorUtils3D.right, -angleSpeed));
                
               
                
            }

            // Rotación sobre el eje Y LOCAL
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
                // Rotación sobre el eje Z LOCAL
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
            QuaternionUtils potentialQuad = new QuaternionUtils(ownQuad);
            potentialQuad.Multiply(deltaRot);
            potentialQuad.Normalize();

            //MEDIMOS LOS ANGULOS
            float potentialAngle = QuaternionUtils.AngleBetween(initialQuad, potentialQuad);

            float currentAngle = QuaternionUtils.AngleBetween(initialQuad, ownQuad);

            //Miramos si la rotacion esta dentro del limite

            if(potentialAngle <= limitAngleRad || potentialAngle < currentAngle)
            {
                ownQuad = potentialQuad;
            }

        }

       // Aplicar rotacion y actualizar el child

        // Apply the final rotation to the GameObject's Transform
        transform.rotation = ownQuad.ToUnityQuaternion();

        if (!isEndEffector && child != null)
        {
            //Calculate the joint's current local forward direction
            VectorUtils3D currentForwardDir = ownQuad.Rotate(VectorUtils3D.forward);

            // Update the child's position
            // New position = Joint Position + (Forward Direction * Length)
            child.position = transform.position + currentForwardDir.GetAsUnityVector() * distanceToChild;
            lr.SetPosition(0, transform.position);
            lr.SetPosition(1, child.position);
        }
    }
}
