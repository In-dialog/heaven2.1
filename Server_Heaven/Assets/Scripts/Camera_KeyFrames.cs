using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

[System.Serializable]
public class Camera_KeyFrames
{
    public float Pos_X;
    public float Pos_Y;
    public float Pos_Z;

    public float Rot_X;
    public float Rot_Y;
    public float Rot_Z;

    public void SetValues(Vector3 Position, Vector3 Rotation)
    {
        Pos_X = Position.x;
        Pos_Y = Position.y;
        Pos_Z = Position.z;

        Rot_X = Rotation.x;
        Rot_Y = Rotation.y;
        Rot_Z = Rotation.z;
    }
}
