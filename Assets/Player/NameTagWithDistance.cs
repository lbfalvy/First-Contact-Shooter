using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NameTagWithDistance : NameTag
{
    public Transform distanceOrigin;
    public string Name;

    protected override void Start() {
        base.Start();
        if (distanceOrigin == null) distanceOrigin = Camera.main.transform;
    }

    protected override void updateText() {
        float distance = (transform.position - distanceOrigin.position).magnitude;
        SetText(string.Format("{0}\n{1:0.0} km", Name, distance/1000));
    }
}
