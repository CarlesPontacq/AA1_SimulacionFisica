using UnityEngine;
using System.Collections.Generic;

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
        public Direction currentDirection;
    }
    public enum Direction { LEFT, RIGHT, STRAIGHT };
    public enum WheelType { FRONT, BACK };

    float angularVelocity = 0f;

    private void Start()
    {
        for (int i=0; i<wheels.Count;i++)
        {
            wheels[i].currentDirection = Direction.STRAIGHT;
        }
    }

    private void FixedUpdate()
    {
        foreach (Wheel wheel in wheels)
        {
            //wheel.transform.Rotate(new Vector3(0, 0, angularVelocity*50));
        }
    }

    public void TurnWheels(Direction direction)
    {
        foreach (Wheel wheel in wheels)
        {
            if (wheel.type == WheelType.BACK || wheel.currentDirection == direction) continue;

            Vector3 original = wheel.transform.localEulerAngles;

            float rotation = 0f;
            switch (direction)
            {
                case Direction.LEFT:
                    if (wheel.currentDirection == Direction.RIGHT)
                        rotation = -wheelRotation * 2;
                    else
                        rotation = -wheelRotation;
                    break;
                case Direction.RIGHT:
                    if (wheel.currentDirection == Direction.RIGHT)
                        rotation = wheelRotation * 2;
                    else
                        rotation = wheelRotation;
                    break;
                case Direction.STRAIGHT:
                    if (wheel.currentDirection == Direction.LEFT)
                        rotation = wheelRotation;
                    else
                        rotation = -wheelRotation;
                    break;
            }

            wheel.transform.Rotate(new Vector3(0, rotation, 0));
            wheel.currentDirection = direction;
        }
    }

    public void UpdateAngularVelocity(float linearVelocity)
    {
        float newAngularVelocity = linearVelocity / wheelRadius;
        angularVelocity = newAngularVelocity;
        Debug.Log("Velocity updated: " + angularVelocity);
    }
}
    