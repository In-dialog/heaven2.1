using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetLocationAroundMe : MonoBehaviour
{

    public Vector2 CurrentPosition;
    public Transform SataliteManager;

    public bool Working;
    public GameObject Plane;


    private const float kRadiusOfEarth = 50;
    private Vector3 pos;
    private GameObject tmp_plane;

    GameObject tempTrs;

    void Start()
    {
        tempTrs  = new GameObject();
        pos.x = (kRadiusOfEarth) * Mathf.Cos(CurrentPosition.x) * Mathf.Cos(CurrentPosition.y);
        pos.z = (kRadiusOfEarth) * Mathf.Cos(CurrentPosition.x) * Mathf.Sin(CurrentPosition.y);
        pos.y = (kRadiusOfEarth) * Mathf.Sin(CurrentPosition.x);
    }

    private void Update()
    {
        //SataliteManager.gameObject.SetActive(true);
        if (StateOfMachine.Instance.SetSate == true)
        {
            FindObjectOfType<PathFinding>().SetPoints = GetRayHit();
            Debug.Log("I am getting the points for pathfinding");
            StateOfMachine.Instance.SetSate = false;
            SataliteManager.gameObject.SetActive(false);

        }
    }



    List<Vector3> GetRayHit()
    {
        Destroy(tmp_plane);
        tmp_plane = Instantiate(Plane, pos, Quaternion.identity);
        tmp_plane.name = "Plane";
        tmp_plane.transform.parent = this.transform;
        tmp_plane.transform.LookAt(SataliteManager, Vector3.up);
        tmp_plane.transform.Rotate(tmp_plane.transform.rotation.x - 90, tmp_plane.transform.rotation.y, tmp_plane.transform.rotation.z);
        List<Vector3> sat_around = new List<Vector3>();
        Ray landingRay;
        RaycastHit hit;
        foreach (Transform sat in SataliteManager)
        {
            Vector3 Direction = sat.transform.position;
            landingRay = new Ray(Vector3.zero, Direction);
            if (Physics.Raycast(landingRay, out hit, Mathf.Infinity))
            {
                if (hit.collider.tag == "Plane")
                {
                   Vector3 newPos = new Vector3(hit.point.x, hit.point.y, hit.point.z);
                   Vector3 trip = Vector3.ProjectOnPlane(hit.point, tempTrs.transform.up);
                    sat_around.Add(trip);
                }
            }
        }
        return sat_around;
    }
}
