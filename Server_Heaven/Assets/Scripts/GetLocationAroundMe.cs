using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetLocationAroundMe : MonoBehaviour
{

    public Vector2      CurrentPosition;
    public Transform    SataliteManager;
    public bool         Working;
    public GameObject   Plane;

    private const float kRadiusOfEarth = 50;
    private Vector3     pos;
    private GameObject  tmp_plane;
    private GameObject  tempTrs;

    GameObject ourPossition;

    void Start()
    {
        ourPossition = new GameObject("OurSpotOnTheMap");
        tempTrs = new GameObject();
        pos.x = (kRadiusOfEarth) * Mathf.Cos(CurrentPosition.x) * Mathf.Cos(CurrentPosition.y);
        pos.z = (kRadiusOfEarth) * Mathf.Cos(CurrentPosition.x) * Mathf.Sin(CurrentPosition.y);
        pos.y = (kRadiusOfEarth) * Mathf.Sin(CurrentPosition.x);
        ourPossition.transform.position = pos;
    }

    private void Update()
    {
        if (StateOfMachine.Instance.SetSate == true | Working == true)
        {
            FindObjectOfType<PathFinding>().SetPoints = GetRayHit();
            Working = false;
            StateOfMachine.Instance.SetSate = false;
            SataliteManager.gameObject.SetActive(false);
        }
    }

    List<Vector3> GetRayHit()
    {
        Destroy(tmp_plane);
        ourPossition.transform.LookAt(SataliteManager, Vector3.up);

        //tmp_plane.transform.LookAt(SataliteManager, Vector3.up);
        //tmp_plane.transform.Rotate(tmp_plane.transform.rotation.x - 90, tmp_plane.transform.rotation.y, tmp_plane.transform.rotation.z);

        tmp_plane = Instantiate(Plane, ourPossition.transform.position, ourPossition.transform.rotation);
        tmp_plane.name = "Plane";
        tmp_plane.transform.parent = this.transform;

        List<Vector3> sat_around = new List<Vector3>();
        Ray landingRay;
        RaycastHit hit;
        RaycastHit hit2;
        Vector3 right = Vector3.zero, left = Vector3.zero, bottom = Vector3.zero, up = Vector3.zero;
        Vector3 up_left = Vector3.zero, up_right = Vector3.zero, bottom_left = Vector3.zero, bottom_right = Vector3.zero;
        foreach (Transform sat in SataliteManager)
        {
            Vector3 Direction = sat.transform.position;
            landingRay = new Ray(Vector3.zero, Direction);
            if (Physics.Raycast(landingRay, out hit, Mathf.Infinity))
            {
                if (hit.collider.tag == "Plane")
                {
                    Debug.DrawLine(Vector3.zero, hit.point, Color.red);
                    Vector3 Direction_2 = hit.collider.transform.position;
                    landingRay = new Ray(hit.point, -Direction_2);
                    if (Physics.Raycast(landingRay, out hit2, Mathf.Infinity))
                    {
                        if (hit2.collider.tag == "ZeroPlane")
                        {
                            if (sat_around.Count< 1)
                            {
                                FindObjectOfType<GameServer>().Send_Center(hit2.collider.bounds.center);
                            }
                            Debug.DrawLine(hit.point, hit2.point, Color.blue);
                            sat_around.Add(hit2.point);
                        }
                    }
                }
            }
        }
        return sat_around;
    }
}
