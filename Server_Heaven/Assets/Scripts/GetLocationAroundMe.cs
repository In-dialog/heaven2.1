using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetLocationAroundMe : MonoBehaviour
{

    public Read_data        data;
    public Vector2          CurrentPosition;
    public int              Count;
    public Transform        SataliteManager;

    public bool Working;
    public List<Vector3>    Sat_Visible;
    public GameObject       Plane;
    public GameObject       Cube;

    public bool Finished_Courotine;

    private const float         kRadiusOfEarth = 50;
    private Vector3             pos = new Vector3();
    private GameObject          tmp_plane;
    private List<Satalite_cord> satlist;

    IEnumerator Start()
    {
        yield return new WaitForEndOfFrame();
        satlist = data.GetList();
        pos.x = (kRadiusOfEarth) * Mathf.Cos(CurrentPosition.x) * Mathf.Cos(CurrentPosition.y);
        pos.z = (kRadiusOfEarth) * Mathf.Cos(CurrentPosition.x) * Mathf.Sin(CurrentPosition.y);
        pos.y = (kRadiusOfEarth) * Mathf.Sin(CurrentPosition.x);
    }

    private void Update()
    {
        if (Working == true)
        {
            PathFinding.Instance.SetPoints = XY_sat();
            Working = false;
        }
    }


    List<Vector3> XY_sat()
    {
        List<Vector3> sat_around = new List<Vector3>();
        float distance;
        Vector3 OA;
        Vector3 result;

        tmp_plane = Instantiate(Plane, pos, Quaternion.identity);
        tmp_plane.name = "Plane";
        tmp_plane.transform.parent = this.transform;
        tmp_plane.transform.LookAt(SataliteManager, Vector3.up);
        tmp_plane.transform.Rotate(tmp_plane.transform.rotation.x - 90, tmp_plane.transform.rotation.y, tmp_plane.transform.rotation.z);
        GetRayHit(SataliteManager, tmp_plane.transform, Cube);
        tmp_plane.transform.position = Vector3.zero;
        tmp_plane.transform.rotation = new Quaternion(0, 0, 0, 0);
        foreach (Transform pos in tmp_plane.transform)
        {
            OA = (pos.position - tmp_plane.transform.position);
            distance = (OA.x * tmp_plane.transform.up.x) + (OA.y * tmp_plane.transform.up.y) + (OA.z * tmp_plane.transform.up.z);
            result = pos.position - tmp_plane.transform.up * distance;
            sat_around.Add(result);
        }
        GameObject.Destroy(tmp_plane);
        return (sat_around);
    }

    void GetRayHit(Transform Parent, Transform destination, GameObject tmp_cube)
    {
        Vector3 Origin;
        Vector3 Direction;
        Vector3 Target = new Vector3(0, 0, 0);
        Ray landingRay;
        RaycastHit hit;

        foreach (Transform sat in Parent)
        {
            Origin = Target;
            Direction = sat.transform.position;
            landingRay = new Ray(Origin, Direction);
            if (Physics.Raycast(landingRay, out hit, Mathf.Infinity))
            {
                if (hit.collider.tag == "Plane")
                {
                    GameObject cube = Instantiate(tmp_cube, hit.point, Quaternion.identity);
                    cube.name = "Cube";
                    cube.transform.parent = destination;
                }
            }
        }
    }
}
