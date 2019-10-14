using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MathAdditions
{
    public static Vector3 ToAngularVelocity(this Quaternion q) {
        float magnitude;
        Vector3 axis;
        q.ToAngleAxis(out magnitude, out axis);
        return axis.normalized*magnitude;
    }
    public static Quaternion Subtract(this Quaternion a, Quaternion b) {
        return a*Quaternion.Inverse(b);
    }
}
