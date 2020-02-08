using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class PathFinding : MonoBehaviour
{
    public List<Vector3>        satelite = new List<Vector3>();
    public List<Vector3>        pointFound = new List<Vector3>();
    public static Vector3[]     bestObstions;
    public List<Vector3>        originalSatelite = new List<Vector3>();
    public Transform            Satalite_manager;
    public int                  nr_of_paths;

    public static PathFinding   Instance;
    public LineRenderer         lr;
    public int                  count, count2, nr_of_points_send;
    static float                wight;
    public float                pastWaight = 0;
    private Vector3             Vector;


    void Start()
    {
        Instance = this.GetComponent<PathFinding>();
        bestObstions = new Vector3[1];
        pointFound.Add(this.transform.position);
        wight = -originalSatelite.Count;
        nr_of_paths = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (originalSatelite.Count < 1) return;
        if (count2 >= originalSatelite.Count | (((100 - ((pastWaight * -1) / originalSatelite.Count) * 100) < 10) && count2 >= 20))
        {
            StateOfMachine.Instance.SetSate = true;
            count2 = 0;
        }
        
        if (satelite.Count < 1)
        {
            Debug.Log("I am in path finding");
            //if (pastWaight <  wight)
            if (wight < pastWaight)
            {
                Debug.Log(wight + "<--------------- My current weight" + pastWaight + "<--------------- My past weight");
                bestObstions = new Vector3[lr.positionCount];
                lr.GetPositions(bestObstions);
                Debug.Log("Sending an option with this length ----------------->" + bestObstions.Length);
                nr_of_points_send += bestObstions.Length;
                nr_of_paths += 1;
                FindObjectOfType<GameServer>().SendPoint(bestObstions);
                pastWaight = wight;
                Satalite_manager.gameObject.SetActive(true);
            }
            pointFound.Clear();
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
