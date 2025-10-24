using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class ChooseJoints : MonoBehaviour
{
    [SerializeField] public List<JointArm> joints;

    JointArm currentJoint;
    
    void Start()
    {
        if (joints != null)
        {
            currentJoint = joints[0];
        }
    }

    void Update()
    {
        foreach (JointArm joint in joints)
        {
            if(joint != currentJoint)
            {
                joint.isSelected = false;
            }
            else
            {
                joint.isSelected = true;
            }
        }

        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            currentJoint.isSelected = false;
            currentJoint = joints[0];
            currentJoint.isSelected = true;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            currentJoint.isSelected = false;
            currentJoint = joints[1];
            currentJoint.isSelected = true;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            currentJoint.isSelected = false;
            currentJoint = joints[2];
            currentJoint.isSelected = true;
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            currentJoint.isSelected = false;
            currentJoint = joints[3];
            currentJoint.isSelected = true;
        }

    }
}
