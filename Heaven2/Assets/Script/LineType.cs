using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;


public class LineProperties
{
    public string type;
    public Vector3 starPosition, endPosition;
    public int LR;
    public Transform center;
    public float radious;
    public float angle;


    public LineProperties()
    {
    }

    public LineProperties(Vector3 starPosition, Vector3 endPosition, string type)
    {
        this.starPosition = starPosition;
        this.endPosition = endPosition;
        this.type = type;
    }

    public LineProperties(Vector3 starPosition, Vector3 endPosition, string type, float radious, int lR)
    {
        this.starPosition = starPosition;
        this.endPosition = endPosition;
        this.type = type;
        this.radious = radious;
        LR = lR;
    }

    public LineProperties(Vector3 sp, Vector3 ep, Transform c, string t, float r, int lr)
    {
        starPosition = sp;
        endPosition = ep;
        center = c;
        radious = r;
        type = t;
        LR = lr;
    }


}

public class ArduinoConmander
{

    public int  stepsX, stepsY;
    static  string _lock = "$X";
    static string _pause = "M0";
    static string _home = "$H";
    static string _reset;

    public string port;
    public SerialController sr;
    public int nrOfMachine;
    public bool connectedOn;
    public string machineType;

    string _origin = "G0X????Y?????";

    public ArduinoConmander()
    {
    }

    public string SetMachineType 
    {
        set
        {
            machineType = value;
        }
    }
    public SerialController SetSr
    {
        set
        {
            value = sr;
        }
        get
        {
            return sr;
        }
    }
    //public void MoveTo(Vector3 pos)
    //{
    //    sr.SendSerialMessage("G0X" + (pos.y -130) + "Y" + (pos.x-190));
    //}
    public void MoveTo(Vector3 pos)
    {
        sr.SendSerialMessage("G0X" + (pos.y +70) + "Y" + (pos.x +70));
    }

    public void MoveForword (int step)
    {
        stepsX -= step;
        sr.SendSerialMessage("G0X"+stepsX+"Y"+stepsY);
    }

    public void MoveBack( int step)
    {
        stepsX +=step;
        sr.SendSerialMessage("G0X" + stepsX + "Y" + stepsY);

    }
    public void MoveLeft( int step)
    {
        stepsY -= step;
        sr.SendSerialMessage("G0X" + stepsX + "Y" + stepsY);
    }
    public void MoveRight( int step)
    {
        stepsY += step;
        sr.SendSerialMessage("G0X" + stepsX + "Y" + stepsY);
    }

    public int SetNumber
    {
        set
        {
            nrOfMachine = value;
        }
        get
        {
            return nrOfMachine;
        }
    }

    public string SetPot
    {
        set
        {
            port = value;
        }
        get
        {
            return port;
        }
    }

    public void SendComand( string _case)
    {
        switch (_case)
        {

            case "Reset" :
                _reset = Encoding.ASCII.GetString(new byte[] { 24 });
                sr.SendSerialMessage(_reset);
                break;
            case "Pause":
                sr.SendSerialMessage(_pause);
                break;
            case "Home":
                sr.SendSerialMessage(_home);
                break;
            case "StartPosition":
                string _startPosition = "G0X????Y?????";
                sr.SendSerialMessage(_startPosition);
                break;
        }
    }
}
