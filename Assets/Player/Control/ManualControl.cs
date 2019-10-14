using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ManualControl
{
    [Header("Camera control (aka mouse-aim)")]
    public bool FollowCamera = true;
    [ConditionalField("FollowCamera")] public KeyCode FreeCamera;
    
    [Space(10)]

    [Header("Thrust control")]
    public bool ControlThrottle = true;
    [ConditionalField("ControlThrottle")] public float ThrottleStep = 0.01f;
    [ConditionalField("ControlThrottle")] public KeyCode IncreaseThrottle;
    [ConditionalField("ControlThrottle")] public KeyCode DecreaseThrottle;
    [ConditionalField("ControlThrottle")] public KeyCode MaxThrottle;
    [ConditionalField("ControlThrottle")] public KeyCode MinThrottle;

    [Space(10)]

    [Header("Guns")]
    public KeyCode Fire;

    ShipControl sc;
    ShipMovement sm;
    ShipTurning st;
    Gun gun;
    Transform ControllingCamera;

    public void Start( ShipControl shipControl, Gun gun, ShipTurning shipTurning, 
        ShipMovement shipMovement, Transform cam)
    {
        sc = shipControl;
        this.gun = gun;
        st = shipTurning;
        sm = shipMovement;
        ControllingCamera = cam;
    }
    public void Update() {
        if (Input.GetKeyDown(FreeCamera))
            FollowCamera = !FollowCamera;
        if (FollowCamera) HandleCameraControl();
        if (ControlThrottle) HandleThrottleInput();
    }
    void HandleThrottleInput() {
        if (Input.GetKey(IncreaseThrottle))
            sm.Throttle += ThrottleStep * Time.deltaTime;
        if (Input.GetKey(DecreaseThrottle))
            sm.Throttle -= ThrottleStep * Time.deltaTime;
        if (Input.GetKeyDown(MaxThrottle)) sm.Throttle = 1;
        if (Input.GetKeyDown(MinThrottle)) sm.Throttle = 0;
        if (Input.GetKeyDown(Fire)) gun.Shoot();
    }
    void HandleCameraControl() {
        if (ControllingCamera != null)
            st.TargetRotation = ControllingCamera.rotation;
    }
}
