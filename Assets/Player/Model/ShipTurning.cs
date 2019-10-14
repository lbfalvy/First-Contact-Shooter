using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ShipTurning : NetworkBehaviour
{
    public float Torque;
    
    Quaternion tgtRotation;
    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (tgtRotation == null) tgtRotation = transform.rotation;
    }
    
    void FixedUpdate() {
        rb.angularVelocity = Vector3.zero;
        float difference = Quaternion.Angle(transform.rotation, tgtRotation);
        if ( !Mathf.Approximately(difference, 0) ) {
            Quaternion step = Quaternion
                .RotateTowards(transform.rotation, tgtRotation, Torque)
                .Subtract(transform.rotation);
            rb.AddTorque(step.ToAngularVelocity());
        }
    }

    [HideInInspector]
    public Quaternion TargetRotation {
        get => tgtRotation;
        set => tgtRotation = value;
    }
    public Vector3 TargetDirection {
        get => (tgtRotation * Vector3.forward);
        set {
            Vector3 newFwd = value;
            Vector3 oldFwd = transform.forward;
            var rot = Quaternion.FromToRotation(oldFwd, newFwd);
            Vector3 newUp = rot * transform.up;
            tgtRotation = Quaternion.LookRotation(newFwd, newUp);
        }
    }
    public float TurnTime {
        get => Torque / rb.mass;
    }
}
