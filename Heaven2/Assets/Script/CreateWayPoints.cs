using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateWayPoints : MonoBehaviour
{
    public int maxPoints, maxX, maxY;
    public int _case;
    MeshFilter mF;
    public GameObject pen;
    // Start is called before the first frame update
    void Start()
    {
        GameObject  plane = GameObject.CreatePrimitive(PrimitiveType.Plane);
        plane.GetComponent<MeshRenderer>().enabled = false;
         mF = plane.GetComponent<MeshFilter>();
        mF.mesh = UpdateMesh(mF.mesh, (maxX/5/2), (maxY/5/2 ));
        plane.transform.position = new Vector3(0, 0, 2);

    }

    // Update is called once per frame

    public  List<Vector3>   PointsInstance(int aCase)
    {
    List < Vector3 > _wayPoints = new List<Vector3>();

        _case = aCase;
        if (_case == 0)
        {

            Vector3 temp = mF.mesh.GetRandomPointOnSurface();
            while (Vector3.Distance(temp, pen.transform.position) > 50)
            {
                temp = mF.mesh.GetRandomPointOnSurface();
            }
            _wayPoints.Add(temp);

        }
        if (_case == 1)
        {
            for (int i = 0; i < maxPoints; i++)
            {

                Vector3 temp = RandomPoint();
                if(i>1)
                while (Vector3.Distance(temp, _wayPoints[i-1]) < 2 | Vector3.Distance(temp, _wayPoints[i - 1]) > 10)
                {
                    temp = RandomPoint();
                }
                _wayPoints.Add(temp);
            }
        }
        return _wayPoints;

    }
    Vector3 RandomPoint()
    {
        //yPos += Random.Range(1f, 5);
        //if (yPos >= maxY| yPos < -maxY)
        //{
        //    yPos = -maxY;
        //}
        //float xPos = Random.Range(-maxX/2, maxX/2);

        ////return  new Vector3(yPos, xPos,0 );
        ///
        //return new Vector3(Random.Range(-maxX/2, maxX/2), Random.Range(maxY/2, maxY/2));
        ////Random.InitState(Random.Range(12, 21200));
        Vector3 random = Random.insideUnitCircle * maxX ;
        random = new Vector3(random.x + 70, random.y, random.z);
        //random = new Vector3(random.x - 190, random.y-130, random.z);
        return random;
    }
    Mesh UpdateMesh(Mesh aMesh, float scaleX, float scaleY)
    {
        Vector3[] baseVertices =  aMesh.vertices;
        Vector3[] vertices = new Vector3[baseVertices.Length];
        Vector3 center = Vector3.zero;//any V3 you want as the pivot point.
        Quaternion newRotation = new Quaternion();
        newRotation.eulerAngles = new Vector3(0, -90, 90);//the degrees the vertices are to be rotated, for example (0,90,0) ;
        for (int i = 0; i < vertices.Length; i++)
        {
            var vertex = baseVertices[i];
            vertex.x = vertex.x * scaleX;
            vertex.z = vertex.z * scaleY;
            vertices[i] = vertex;
            vertices[i] = newRotation * (vertices[i] - center) + center;

        }
        aMesh.vertices = vertices;
        aMesh.RecalculateNormals();
        aMesh.RecalculateBounds();

        return aMesh;
    }
}
