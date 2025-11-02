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
        /*
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
        */

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            currentJoint.Unselect();
            currentJoint = joints[0];
            currentJoint.Select();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            currentJoint.Unselect();
            currentJoint = joints[1];
            currentJoint.Select();
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            currentJoint.Unselect();
            currentJoint = joints[2];
            currentJoint.Select();
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            currentJoint.Unselect();
            currentJoint = joints[3];
            currentJoint.Select();
        }

    }
}
