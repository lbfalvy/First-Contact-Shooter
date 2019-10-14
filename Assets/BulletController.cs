using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class BulletController : NetworkBehaviour
{
    void OnCollisionEnter(Collision col) {
        if (!isClient) CollideOnServer(col);
        GameObject.Destroy(gameObject);
    }

    void CollideOnServer(Collision col) {
        Transform otherTf = col.transform;
        ShipControl shipControl = otherTf.GetComponentInParent<ShipControl>();
        if (shipControl != null)
            shipControl.GetComponent<NetworkPlayer>().takeDamage(100);
    }
}
