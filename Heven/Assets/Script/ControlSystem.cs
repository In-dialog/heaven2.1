using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlSystem : MonoBehaviour
{
   public List<Vector3> _wayPoints = new List<Vector3>();
    public List<Vector3> pointsRecived = new List<Vector3>();

    public List<LineProperties> lineProperties = new List<LineProperties>();
    public bool needPoints;
    public bool randomMode;

    void Update()
    {
        if (randomMode)
        {
            if (_wayPoints.Count < 1)
            {
                _wayPoints = GetComponent<CreateWayPoints>().PointsInstance(0);

            }
        }
        else
        {
            if (needPoints)
            {
                FindObjectOfType<GameClient>().start = true;
                needPoints = false;
            }

            if (_wayPoints.Count < 1)
            {
                _wayPoints = new List<Vector3>(pointsRecived);
                pointsRecived.Clear();
                print("No Oints");

            }
            else
                print("I Have Oints");

            if (lineProperties.Count > 100)
            {
                lineProperties.Clear();
                lineProperties = new List<LineProperties>();
            }
        }
    }
 

    public LineProperties SetLine
    {
        set
        {
            _ = new LineProperties();
            LineProperties temp = value;
            Debug.Log(temp.type);
            lineProperties.Add(temp);
            FindObjectOfType<GraphicElements>().InstanceObject(temp);
            FindObjectOfType<GraphicElements>().CreatePoints(temp);

            string comand = CreateComands(temp);
            if (comand != "null")
            {
                FindObjectOfType<SendToArduino>()._positionsToSend[0].Add(comand);
                FindObjectOfType<SendToArduino>()._positionsToSend[1].Add(comand);
            }
        }

    }

    public Vector3 GetTarget
    {
        get
        {
            if (_wayPoints.Count>0)
            {
                Vector3 temp = _wayPoints[0];
                _wayPoints.RemoveAt(0);
                return temp;
            }
            else
            {
                return Vector3.zero;
            }
        }
    }

    string CreateComands(LineProperties inLine)
    {
        int scale = 1;
        string lineType = inLine.type;
        string comand = "null";
        if (lineType == "Arc")
        {   if(lineProperties.Count>1)
            comand = "G01X" + -lineProperties[lineProperties.IndexOf(inLine)-1].endPosition.y * scale + "Y" + -lineProperties[lineProperties.IndexOf(inLine) - 1].endPosition.x * scale;
            if (inLine.LR == 1)
                comand = "G03X" + -inLine.endPosition.y * scale + "Y" + -inLine.endPosition.x * scale + "R" + inLine.radious * scale;
            if (inLine.LR == -1)
                comand = "G02X" + -inLine.endPosition.y * scale + "Y" + -inLine.endPosition.x * scale + "R" + inLine.radious * scale;
        }
        if (lineType == "Line")
        {
            comand = "G01X" + -inLine.endPosition.y * scale + "Y"+ -inLine.endPosition.x * scale;
        }
        return comand;
    }


}

