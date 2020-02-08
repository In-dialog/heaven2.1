using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour
{
    LineProperties lineProperties = new LineProperties();
    public Vector3 targe;
    public float rottationSpeed;
    bool ontarget,firstTime;
    bool restart = true;
    GameObject center;
    void Start()
    {
        center = new GameObject("Center");
    }
 
    void Update()
    {
        if (targe != Vector3.zero)
        {
            Transform myTransform = this.transform;
            if (restart)
            {
                lineProperties = new LineProperties();
                lineProperties.LR = OrientationOfTarget(myTransform.forward, targe, myTransform.up);
                lineProperties.center = CalculateCenter(targe, lineProperties.LR);
                restart = false;
            }
            int angle = (int)FindAngle(lineProperties.center.position, myTransform.position, targe);
            Debug.Log(angle +" Center  "+ lineProperties.center.position+"  ourPosition  "+ myTransform.position +"  the target "+ targe);
          
            //////////////////////////////////---------------------------------->>>>>>> If object is not in front rotate around a point towords the trget
            if (Mathf.Abs(angle - 90) > Mathf.Epsilon && !ontarget)
            {
                if (lineProperties.LR == 1 || lineProperties.LR == -1) 
                {
                    if (firstTime)
                    {
                        lineProperties.type = "Arc";
                        lineProperties.angle = angle;
                        lineProperties.starPosition = transform.position;
                        firstTime = false;
                    }
                    transform.RotateAround(lineProperties.center.position, Vector3.back, lineProperties.LR * rottationSpeed * Time.deltaTime);

                }
            }
            else
            {
            //////////////////////////////////---------------------------------->>>>>>> Else if object is in front go towords object
                ontarget = true;
                if (myTransform.position != targe)
                {
                    if (!firstTime)
                    {
                        lineProperties.endPosition = transform.position;
                        FindObjectOfType<ControlSystem>().SetLine = lineProperties;
                        lineProperties.type = "Line";
                        lineProperties.starPosition = transform.position;
                        firstTime = true;
                    }

                    float step = rottationSpeed/2 * Time.deltaTime;
                    transform.position = Vector3.MoveTowards(transform.position, targe, step);
                }
                else
                {
                    lineProperties.endPosition = transform.position;
                    FindObjectOfType<ControlSystem>().SetLine = lineProperties;
                    targe = FindObjectOfType<ControlSystem>().GetTarget;
                    ontarget = false;
                    restart = true;
            
                }
            }
            if (angle < -180)
            {
                restart = true;
                targe = Vector3.zero;
                return;
            }
        }
        else targe = FindObjectOfType<ControlSystem>().GetTarget;
    }

    Transform CalculateCenter(Vector3 _target, int orientation)
    {
        center.transform.position = Vector3.zero  ;
        float curentDist = Vector3.Distance(this.gameObject.transform.position, _target);
        Vector3 position = (transform.position + (orientation * transform.right) * curentDist / 8);
        lineProperties.radious = Vector3.Distance(position, transform.position);
        center.transform.position = position;
        return center.transform;
    }

    int OrientationOfTarget(Vector3 fwd, Vector3 targetDir, Vector3 up)
    {
        Vector3 heading = targetDir - transform.position;

        Vector3 perp = Vector3.Cross(fwd, heading);
        float dir = Vector3.Dot(perp, up);

        if (dir > 0f)
        {
            return 1;
        }
        else if (dir < 0f)
        {
            return -1;
        }
        else
        {
            return 0;
        }
    } 
    public double FindAngle(Vector3 p0, Vector3 p1, Vector3 p2)
    {
        float a = Mathf.Pow(p1.x - p0.x, 2) + Mathf.Pow(p1.y - p0.y, 2),
            b = Mathf.Pow(p1.x - p2.x, 2) + Mathf.Pow(p1.y - p2.y, 2),
            c = Mathf.Pow(p2.x - p0.x, 2) + Mathf.Pow(p2.y - p0.y, 2);
        double ang = Mathf.Acos((a + b - c) / Mathf.Sqrt(4 * a * b));
        ang *= 360.0 / (2 * Mathf.PI);
        return ang;
    }
}
