using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zeptomoby.OrbitTools;
using System.Runtime.Serialization.Formatters.Binary;

//[System.Serializable]
public class Satalite_cord : MonoBehaviour
{
    public string       line1;
    public string       line2;
    public string       str1 = "SGP4";

    public double       PosX;
    public double       PosY;
    public double       PosZ;

    public double       VelX;
    public double       VelY;
    public double       VelZ;

    public void CreatePosVel(Tle tle)
    {
        Satellite sat = new Satellite(tle);

        Eci eci = sat.PositionEci(360);
        PosX = eci.Position.X;
        PosY = eci.Position.Y;
        PosZ = eci.Position.Z;

        VelX = eci.Velocity.X;
        VelY = eci.Velocity.Y;
        VelZ = eci.Velocity.Z;
    }
}
