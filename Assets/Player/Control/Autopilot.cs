using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Autopilot
{
    public enum Mode { Disabled, LookAt, Brake }

    public KeyCode Disable;
    public KeyCode LookAt;
    public KeyCode Brake;
    public Mode mode;

    ShipControl sc;
    ShipTurning st;
    ShipMovement sm;
    Selector sel;

    Transform tf;
    Rigidbody rb;

    #region Wiring, input
    public void Start(ShipControl shipControl, Transform transform, 
        Rigidbody rigidbody, ShipTurning shipTurning, ShipMovement shipMovement,
        Selector selector)
    {
        sc = shipControl;
        tf = transform;
        rb = rigidbody;
        st = shipTurning;
        sm = shipMovement;
        sel = selector;
    }
    public void Update()
    {
        if (Input.GetKeyDown(Disable)) mode = Mode.Disabled;
        if (Input.GetKeyDown(LookAt)) mode = Mode.LookAt;
        if (Input.GetKeyDown(Brake)) mode = Mode.Brake;
        switch(mode) {
            case Mode.Disabled: return;
            case Mode.Brake:
                BrakeByTarget();
                break;
            case Mode.LookAt:
                lookAtTarget();
                break;
            default: break;
        }
    }
    #endregion
    
    void lookAtTarget() {
        st.TargetDirection = sel.target.transform.position - tf.position;
    }
    void BrakeByTarget() {
        if (sel.target == null) return;
        var v = getVelocity(sel.target);
        var targetVelocity = v - rb.velocity;
        st.TargetDirection = targetVelocity;
        sm.Throttle = decideAutoThrottle(targetVelocity);
    }
    Vector3 getVelocity(Component target) {
        Rigidbody rb;
        bool has = target.TryGetComponent(out rb);
        return has ? rb.velocity : Vector3.zero;
    }
    float decideAutoThrottle(Vector3 target) {
        float angle = Vector3.Angle( tf.forward, target );
        Vector3 difference = target - rb.velocity;
        float maxDeltaV = Time.fixedDeltaTime * sm.MaxAcceleration;
        if ( angle > 45 ) return 0;
        else if (difference.magnitude < maxDeltaV)
            return difference.magnitude/maxDeltaV;
        else return 1;
    }
}
