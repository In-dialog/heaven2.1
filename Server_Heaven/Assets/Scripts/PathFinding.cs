using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class PathFinding : MonoBehaviour
{
    public List<Vector3> satelite = new List<Vector3>();
    public List<Vector3> pointFound = new List<Vector3>();
    public static Vector3[] bestObstions;
    public List<Vector3> originalSatelite = new List<Vector3>();

    public static PathFinding Instance;
    public LineRenderer lr;
   public  int count, count2,count3;
    public float wight = 0;
    public  float pastWaight = 0;
    Vector3 Vector;


    void Start()
    {
        Instance = this.GetComponent<PathFinding>();
        bestObstions = new Vector3[1];
        pointFound.Add(this.transform.position);
        wight = -originalSatelite.Count;
    }

    // Update is called once per frame
    void Update()
    {
        if (originalSatelite.Count < 1) return;
        if (count2 >= originalSatelite.Count)
        {
            StateOfMachine.Instance.SetSate = true;
            //originalSatelite.Clear();
            count2 = 0;
        }
        //if (count3 >= 5)
        //{
        //    StateOfMachine.Instance.SetSate = true;
        //    originalSatelite.Clear();
        //    count3 = 0;
        //    return;
        //}
        
       
        if (satelite.Count < 1)
        {
            //If weight does not change stop geving data and desplay satelites
            Debug.Log("I am in finding a path");
            //if (pastWaight <  wight)
                if (wight < pastWaight)
                {
                Debug.Log(wight + "<--------------- My current weight" + pastWaight + "<--------------- My past weight");
                bestObstions = new Vector3[lr.positionCount];
                lr.GetPositions(bestObstions);
                Debug.Log("Send second best options and its length" + bestObstions.Length);
                FindObjectOfType<GameServer>().SendPoint(bestObstions);
                pastWaight = wight;
                count3++;
            }
            pointFound.Clear();
            //if (count2 == originalSatelite.Count - 1)
            //{
            //    Debug.Log("Send best options and itts length" + bestObstions.Length);
            //    FindObjectOfType<GameServer>().SendPoint(bestObstions);
            //    lr.positionCount = bestObstions.Length - 1;
            //    lr.SetPositions(bestObstions);
            //    wight = 0;
            //}
            pointFound.Add(originalSatelite[count2]);
            count = 0;
            wight = -originalSatelite.Count-1;
            satelite = new List<Vector3>(originalSatelite);
            count2++;
        }
        else
        {
            Vector3 temp = CheckDistance(satelite, pointFound[pointFound.Count - 1]);
            pointFound.Add(temp);
            satelite.Remove(temp);
            for (int i = 0; i < pointFound.Count - 3; i += 2)
            {
                if (Math2d.LineSegmentsIntersection(pointFound[pointFound.Count - 2], pointFound[pointFound.Count - 1], pointFound[i], pointFound[i + 1], out Vector))
                {
                   wight += 1f;
                }

            }
            lr.positionCount = count + 1;
            lr.SetPosition(count, temp);

            count++;
        }
    }
    Vector3 CheckDistance(List<Vector3> allPoints, Vector3 thePoint)
    {
        List<float> distanceList = new List<float>();
        foreach (var item in allPoints)
        {
            distanceList.Add(Vector3.Distance(thePoint, item));

        }
        float minVal = distanceList.Min();
        return allPoints[distanceList.IndexOf(minVal)];
    }

    public List<Vector3> SetPoints
    {
        set
        {
            satelite = new List<Vector3>(value);
            originalSatelite = new List<Vector3>(value);
            pastWaight =0;


            wight = -originalSatelite.Count;
        }
    }
    public Vector3[] GetBestPoints()
    {
      
      return (bestObstions);
      
    }

    public void Reset_array()
    {
        Array.Clear(bestObstions, 0, bestObstions.Length);
        bestObstions = new Vector3[1];
    }
}
