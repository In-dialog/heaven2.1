using System.Collections.Generic;
using UnityEngine;

public class ControlSystem : MonoBehaviour
{
    public List<Vector3> _wayPoints = new List<Vector3>();
    public List<LineProperties> lineProperties = new List<LineProperties>();
    bool needPoints= true;
    public bool randomMode;
    public Vector3 center;
     public  bool noWStart;
    public void ActivateObject(bool active)
    {
        //needPoints = active;
        noWStart = !noWStart;
    }
        public void Random(bool active)
    {
        //needPoints = active;
        randomMode = !randomMode;

    }
    void Update()
    {

        if (lineProperties.Count > 100)
        {
            lineProperties.Clear();
            lineProperties = new List<LineProperties>();
        }

        if (!noWStart)
        {
            _wayPoints.Clear();
            return;
        }

        if (_wayPoints.Count == 4)
        {
            needPoints = true;
        }
        if (_wayPoints.Count < 1 & needPoints)
        {
            if (randomMode)
                _wayPoints = GetComponent<CreateWayPoints>().PointsInstance(1);
            else
                FindObjectOfType<GameClient>().start = true;
            needPoints = false;
        }



    }

    public LineProperties SetLine
    {
        set
        {
            _ = new LineProperties();
            LineProperties temp = value;
            //Debug.Log(temp.type);
            lineProperties.Add(temp);
            FindObjectOfType<GraphicElements>().InstanceObject(temp);
            FindObjectOfType<GraphicElements>().CreatePoints(temp);

            string comand = CreateComands(temp);
            if (comand != "null")
            {
                FindObjectOfType<SendToArduino>()._positionsToSend[0].Add(comand);
                //FindObjectOfType<SendToArduino>()._positionsToSend[1].Add(comand);
                //FindObjectOfType<SendToArduino>()._positionsToSend[1].Add(comand);
            }
            FindObjectOfType<SendToArduino>()._positionsToSend[1].Add(ComandWall(temp, 0.6f));

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
        //Vector2 inVector = new Vector2(inLine.endPosition.y -130,inLine.endPosition.x-190-70);
        //if (inVector.x < -260 & inVector.y < -350)
        //    return null;


        Vector2 inVector = new Vector2(inLine.endPosition.y, inLine.endPosition.x);
        //if (inVector.x < -260 & inVector.y < -350)
        //    return null;

        int scale = 1;
        string lineType = inLine.type;
        string comand = "null";
        if (lineType == "Arc")
        {
            if (lineProperties.Count > 1) {
                Vector3 pastVector = lineProperties[lineProperties.IndexOf(inLine) - 1].endPosition;
                comand = "G01X" + pastVector.y * scale + "Y" + pastVector.x * scale;
            }
            if (inLine.LR == 1)
                comand = "G03X" + inVector.x * scale + "Y" + inVector.y * scale + "R" + inLine.radious * scale;
            if (inLine.LR == -1)
                comand = "G02X" + inVector.x * scale + "Y" + inVector.y * scale + "R" + inLine.radious * scale;
        }
        if (lineType == "Line")
        {
            comand = "G01X" + inVector.x * scale + "Y" + inVector.y * scale;
        }
        return comand;
    }
    string ComandWall(LineProperties inLine, float scale)
    {
        Vector3 inVector = inLine.endPosition;

        string lineType = inLine.type;
        string comand = "null";

        if (lineType == "Arc")
        {
            if (lineProperties.Count > 1)
                comand = "G01X" + -lineProperties[lineProperties.IndexOf(inLine) - 1].endPosition.y * scale + "Y" + -lineProperties[lineProperties.IndexOf(inLine) - 1].endPosition.x * scale;

            if (inLine.LR == 1)
                comand = "G03X" + -inVector.y * scale + "Y" + -inVector.x * scale + "R" + inLine.radious * scale;
            if (inLine.LR == -1)
                comand = "G02X" + -inVector.y * scale + "Y" + -inVector.x * scale + "R" + inLine.radious * scale;
        }
        if (lineType == "Line")
        {
            comand = "G01X" + -inVector.y * scale + "Y" + -inVector.x * scale;
        }
        return comand;
    }

    Vector2 Converted_Value(Vector3 Points, float distance_from_center, float distance_between_motors)
    {
        Vector2 result = new Vector2();
        Vector2 Pos_Mx = new Vector2();
        Vector2 Pos_My = new Vector2();

        Pos_Mx.x = center.x - distance_between_motors / 2;
        Pos_Mx.y = center.y + distance_from_center;
        Pos_My.x = center.x + distance_between_motors / 2;
        Pos_My.y = center.y + distance_from_center;

        result.x = Vector2.Distance(Points, Pos_Mx)*-1;
        result.y = Vector2.Distance(Points, Pos_My)*-1;
        Debug.Log(result + "dsdfsfadfsdfsdfadsfds");


        return result;
    }
    //string CreateComands(LineProperties inLine)
    //{
    //    Vector3 inVector = inLine.endPosition;
    //    if (lineProperties.Count > 1)
    //    {
    //        Vector3 pastVector = lineProperties[lineProperties.IndexOf(inLine) - 1].endPosition;
    //    }

    //    int scale = 1;
    //    string lineType = inLine.type;
    //    string comand = "null";
    //    if (lineType == "Arc")
    //    {
    //        if (lineProperties.Count > 1)
    //            comand = "G01X" + lineProperties[lineProperties.IndexOf(inLine) - 1].endPosition.y * scale + "Y" + lineProperties[lineProperties.IndexOf(inLine) - 1].endPosition.x * scale;
    //        if (inLine.LR == 1)
    //            comand = "G03X" + (inLine.endPosition.y * scale + "Y" + inLine.endPosition.x * scale + "R" + inLine.radious * scale;
    //        if (inLine.LR == -1)
    //            comand = "G02X" + inLine.endPosition.y * scale + "Y" + inLine.endPosition.x * scale + "R" + inLine.radious * scale;
    //    }
    //    if (lineType == "Line")
    //    {
    //        comand = "G01X" + inLine.endPosition.y * scale + "Y" + inLine.endPosition.x * scale;
    //    }
    //    return comand;
    //}
}

