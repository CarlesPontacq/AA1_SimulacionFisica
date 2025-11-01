using UnityEngine;
using System.Collections.Generic;
using QuaternionUtility;

public class WheelsController : MonoBehaviour
{
    [SerializeField] List<Wheel> wheels;
    [SerializeField] float wheelRotation = 20f;
    [SerializeField] float wheelRadius = 0.0365f;

    [System.Serializable]
    class Wheel
    {
        public Transform transform;
        public WheelType type;

        [HideInInspector] public Direction currentDirection;
        [HideInInspector] public float steeringAngle = 0f;
        [HideInInspector] public float rollAngle = 0f;
        [HideInInspector] public QuaternionUtils baseRotation;
        [HideInInspector] public VectorUtils3D position;
    }

    public enum Direction { LEFT, RIGHT, STRAIGHT };
    public enum WheelType { FRONT, BACK };

    float angularVelocity = 0f;

    private void Start()
    {
        foreach (var wheel in wheels)
        {
            wheel.currentDirection = Direction.STRAIGHT;
            wheel.steeringAngle = 0f;
            wheel.rollAngle = 0f;

            // Guardar la rotación y posición base de cada rueda

            wheel.baseRotation = new QuaternionUtils();
            wheel.baseRotation.AssignFromUnityQuaternion(wheel.transform.localRotation);

            Vector3 unityPos = wheel.transform.localPosition;
            wheel.position = new VectorUtils3D(unityPos.x, unityPos.y, unityPos.z);
        }
    }

    private void FixedUpdate()
    {
        foreach (Wheel wheel in wheels)
        {
            // Calcular la rotación que hay que hacer este frame
            wheel.rollAngle += angularVelocity * Mathf.Rad2Deg * Time.fixedDeltaTime;

            // Preparar los quaternions de las rotaciones
            QuaternionUtils steerRot = new QuaternionUtils();
            steerRot.FromYRotation(wheel.steeringAngle * Mathf.Deg2Rad);

            QuaternionUtils rollRot = new QuaternionUtils();
            rollRot.FromZRotation(wheel.rollAngle * Mathf.Deg2Rad);

            // Combinar rotaciones
            QuaternionUtils finalRot = new QuaternionUtils();
            finalRot.AssignFromUnityQuaternion(wheel.baseRotation.GetAsUnityQuaternion());
            finalRot.Multiply(steerRot);
            finalRot.Multiply(rollRot);

            // Aplicar resultado a la rueda
            wheel.transform.localRotation = finalRot.GetAsUnityQuaternion();    
        }
    }

    public void TurnWheels(Direction direction)
    {
        foreach (Wheel wheel in wheels)
        {
            // Se ignoran las ruedas de atrás (porque no tienen este tipo de giro) y las ruedas que ya están bien giradas
            if (wheel.type == WheelType.BACK || wheel.currentDirection == direction)
                continue;

            // Calcular el ángulo objetivo al que se quiere llegar en base a la dirección de giro
            float targetAngle = 0f;
            switch (direction)
            {
                case Direction.LEFT: targetAngle = -wheelRotation; break;
                case Direction.RIGHT: targetAngle = wheelRotation; break;
                case Direction.STRAIGHT: targetAngle = 0f; break;
            }

            // Guardar el ángulo y la dirección en las variables de la propia rueda
            wheel.steeringAngle = targetAngle;
            wheel.currentDirection = direction;
        }
    }

    // Actualizar la velocidad angular de las ruedas en base a la velocidad lineal a la que va el robot (y pasarlo a m/s)
    public void UpdateAngularVelocity(float linearVelocity)
    {
        angularVelocity = -linearVelocity / wheelRadius;
        Debug.Log("Velocity updated: " + angularVelocity);
    }
}
