using System.Collections;
using System.Collections.Generic;
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
    public  string port;
    public SerialController sr;
    public int nrOfMachine;
    public bool connectedOn;

    string _origin = "G0X????Y?????";

    public ArduinoConmander()
    {
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

    public void MoveForword (int step)
    {
        stepsX += step;
        sr.SendSerialMessage("G0X"+stepsX+"Y"+stepsY);
    }

    public void MoveBack( int step)
    {
        stepsX  -=step;
        sr.SendSerialMessage("G0X" + stepsX + "Y" + stepsY);

    }
    public void MoveLeft( int step)
    {
        stepsY += step;
        sr.SendSerialMessage("G0X" + stepsX + "Y" + stepsY);
    }
    public void MoveRight( int step)
    {
        stepsY -= step;
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

    public void SendComand(SerialController sc, string _case)
    {
        switch (_case)
        {
            case "Lock" :
                sc.SendSerialMessage(_lock);
                break;
            case "Pause":
                sc.SendSerialMessage(_pause);
                break;
            case "Home":
                sc.SendSerialMessage(_home);
                break;
            case "StartPosition":
                string _startPosition = "G0X????Y?????";
                sc.SendSerialMessage(_startPosition);
                break;
        }
    }
}
