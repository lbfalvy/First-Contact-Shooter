using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

public class ShipControl : NetworkBehaviour
{
    public ManualControl manual;
    public Selector selector;
    public Autopilot autopilot;
    public Gun gun;

    void Start()
    {
        var mainCamera = Camera.main;
        if (isLocalPlayer) {
            ConnectElements(mainCamera);
            reverseLinkCamera(mainCamera);
            reverseLinkCore();
        }
        else {
            selector = null;
            manual = null;
            autopilot = null;
            gun = null;
        }
        HandleOwnNametag(mainCamera);
    }
    void reverseLinkCamera(Camera cam) {
        var camController = cam.GetComponent<CameraController>();
        camController.Target = this.gameObject;
    }
    void reverseLinkCore() {
        var coreController = FindObjectOfType<CoreController>();
        coreController.addPlayer(this.GetComponent<NetworkPlayer>());
    }
    void HandleOwnNametag(Camera cam) {
        var nameTag = GetComponent<NameTagWithDistance>();
        if (isLocalPlayer) nameTag.enabled = false;
        else nameTag.mainCamera = Camera.main;
    }
    void ConnectElements(Camera cam) {
        // Fetch stuff
        var st = GetComponent<ShipTurning>();
        var sm = GetComponent<ShipMovement>();
        var rb = GetComponent<Rigidbody>();
        var gun = GetComponent<Gun>();
        // Connect it all around
        selector.Start(this, cam);
        manual.Start(this, gun, st, sm, cam.transform);
        autopilot.Start(this, transform, rb, st, sm, selector);
    }
    void Update()
    {
        if (isLocalPlayer) {
            selector?.Update();
            manual?.Update();
            autopilot?.Update();
        } 
    }
}
