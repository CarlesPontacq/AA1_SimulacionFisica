using UnityEngine;
using QuaternionUtility;
using System.Numerics;
using System;

public class JointArm : MonoBehaviour
{
    [SerializeField] bool isEndEffector;
    [SerializeField] public bool isSelected;
    [SerializeField] Transform child;


    QuaternionUtils ownQuad = new QuaternionUtils();
    VectorUtils3D direction = new VectorUtils3D();
    VectorUtils3D ownTrans = new VectorUtils3D();
    VectorUtils3D childTrans = new VectorUtils3D();
    VectorUtils3D initialDir = new VectorUtils3D();

    float distanceToChild;

    void Start()
    {
        // Guardar posición y rotación inicial
        ownQuad.ToQuaternionUtils(transform.rotation);
        ownTrans = VectorUtils3D.ToVectorUtils3D(transform.position);

        if (!isEndEffector && child != null)
        {
            childTrans = VectorUtils3D.ToVectorUtils3D(child.position);
            distanceToChild = VectorUtils3D.Distance(ownTrans, childTrans);
            initialDir = (childTrans - ownTrans).Normalize();
        }

    }

    void Update()
    {

        // Initialize a temporary rotation change variable
        QuaternionUtils deltaRot = null;

        if (isSelected)
        {
            float angle = 1f * Time.deltaTime; // Rotation speed
            VectorUtils3D axis = VectorUtils3D.zero;

            // --- Rotation around LOCAL X-axis (Pitch / Up-Down) ---
            if (Input.GetKey(KeyCode.W))
            {
                // Rotate around the joint's current LOCAL X-axis (transform.right)
                // VectorUtils3D.right is the world-space right vector (1, 0, 0)
                // To get the local X-axis, we need to rotate VectorUtils3D.right by ownQuad
                axis = ownQuad.Rotate(VectorUtils3D.right);
                deltaRot = new QuaternionUtils().AngleToQuaternion(axis, angle);
            }
            else if (Input.GetKey(KeyCode.S))
            {
                axis = ownQuad.Rotate(VectorUtils3D.right);
                deltaRot = new QuaternionUtils().AngleToQuaternion(axis, -angle);
            }

            // --- Rotation around LOCAL Y-axis (Yaw / Left-Right) ---
            if (Input.GetKey(KeyCode.A))
            {
                // Rotate around the joint's current LOCAL Y-axis (transform.up)
                axis = ownQuad.Rotate(VectorUtils3D.up);
                // Note: Use 'up' for yaw rotation unless your arm is built differently
                deltaRot = new QuaternionUtils().AngleToQuaternion(axis, angle);
            }
            else if (Input.GetKey(KeyCode.D))
            {
                axis = ownQuad.Rotate(VectorUtils3D.up);
                deltaRot = new QuaternionUtils().AngleToQuaternion(axis, -angle);
            }

            // Apply the accumulated rotation change if any
            if (deltaRot != null)
            {
                // Apply rotation accumulatedly: new_rotation = delta_rotation * current_rotation
                // This is typically the order for local rotations
                ownQuad.Multiply(deltaRot);
            }
        }

        // --- Update Transform and Child Position ---

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
        }
    }
}
