using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class CameraController : MonoBehaviour
{
    [Tooltip("The camera should look just above the top of the ship")]
    public float VerticalOffset;
    public float Speed = 1;
    public KeyCode FreePointer;
    bool IsPointerFree;

    [Space(10)]

    [Header("Zooming")]
    public float ZoomSpeed;
    public float MaxZoomOut;
    public float Zoom;

    Camera cam;
    GameObject target;

    public GameObject Target { 
        get => target; 
        set {
            target = value;
            transform.rotation = Target.transform.rotation;
        } 
    }

    void Start()
    {
        cam = GetComponent<Camera>();
        if (!Input.GetKey(FreePointer))
            DoCapturePtr();
    }
    void OnDestroy()
    {
        DoFreePtr();
    }

    void Update()
    {
        if (Target != null) {
            if (!IsPointerFree) {
                MoveByInput();
                HandleZoom();
            }
            FollowTarget();
        }
        UpdatePointerState();
    }

    void MoveByInput() {
        float h = Speed * Input.GetAxis("Mouse X");
        float v = Speed * Input.GetAxis("Mouse Y");
        float r = Speed * -Input.GetAxis("Horizontal");
        transform.RotateAround(Target.transform.position, transform.up, h);
        transform.RotateAround(Target.transform.position, transform.right, v);
        transform.RotateAround(Target.transform.position, transform.forward, r);
    }
    void HandleZoom() {
        Zoom += ZoomSpeed * Input.GetAxis("Mouse ScrollWheel");
        Zoom = Mathf.Max(0.1f, Mathf.Min(MaxZoomOut, Zoom));
    }
    void FollowTarget() {
        Vector3 worldRelPos = transform.localToWorldMatrix * localPos;
        transform.position = Target.transform.position + worldRelPos;
    }
    Vector3 localPos { get => new Vector3(0, VerticalOffset, -Zoom); }
    void UpdatePointerState() {
        if (Input.GetKeyDown(FreePointer)) DoFreePtr();
        if (Input.GetKeyUp(FreePointer)) DoCapturePtr();
    }
    void DoFreePtr() {
        Cursor.lockState = CursorLockMode.None;
        IsPointerFree = true;
    }
    void DoCapturePtr() {
        Cursor.lockState = CursorLockMode.Locked;
        IsPointerFree = false;
    }
}
