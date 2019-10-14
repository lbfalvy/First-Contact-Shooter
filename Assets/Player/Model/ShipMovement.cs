using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ShipMovement : NetworkBehaviour
{
    public float MaxThrottle;
    public float Throttle {
        get => throttle;
        set => CmdSetThrottle(value);
    }
    public float MaxAcceleration {
        get => MaxThrottle / rb.mass;
    }
    public float Acceleration {
        get => Throttle*MaxThrottle / rb.mass;
    }

    [SyncVar]
    float throttle = 0;

    [Command]
    void CmdSetThrottle(float value) {
        throttle = Mathf.Min(1, Mathf.Max(0, value));
    }

    Rigidbody rb;
    void Start() {
        rb = GetComponent<Rigidbody>();
    }
    void FixedUpdate() {
        float magnitude = MaxThrottle*Throttle*Time.fixedDeltaTime;
        rb.AddRelativeForce(0,0,magnitude);
    }
}
