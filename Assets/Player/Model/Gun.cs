using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public float Force;
    public Vector3 nozzle;
    public GameObject Projectile;

    Rigidbody rigidbody;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    public void Shoot() {
        Vector3 worldOffset = transform.localToWorldMatrix * nozzle;
        Vector3 pos = transform.position + worldOffset;
        var instance = GameObject.Instantiate(Projectile, pos, transform.rotation);
        var irb = instance.GetComponent<Rigidbody>();
        irb.velocity = rigidbody.velocity; // TODO not accurate, add torque
        irb.AddRelativeForce(Vector3.forward*Force);
    }
}
