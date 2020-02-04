using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleKeplerOrbits;

#pragma warning disable CS3009 // Base type is not CLS-compliant
public class Satalite_manager : MonoBehaviour
#pragma warning restore CS3009 // Base type is not CLS-compliant
{
    public GameObject Satelatite;
    public GameObject Atractor;
    public Read_data data;
    
    public float attractorMass;
    public int count;
    public List<Satalite_cord> satlist;

    private int i;
    private SimpleKeplerOrbits.KeplerOrbitMover body;

    // Start is called before the first frame update
    void Start()
    {
        i = 0;
        satlist = data.GetList();
        Debug.Log("The length of satlist in manager" + satlist.Count);
        foreach (Satalite_cord sat in satlist)
        {
            if (i < count)
            {
                GameObject satilite = Instantiate(Satelatite, new Vector3((float)sat.PosX / 100, (float)sat.PosY / 100, (float)sat.PosZ / 100), Quaternion.identity);
                body = satilite.GetComponent<SimpleKeplerOrbits.KeplerOrbitMover>();
                body.AttractorSettings.AttractorObject = Atractor.transform;
                body.AttractorSettings.AttractorMass = attractorMass;
                body.CreateNewOrbitFromPositionAndVelocity(satilite.GetComponent<Transform>().transform.position, new Vector3((float)sat.VelX, (float)sat.VelY, (float)sat.VelZ));
                body.SetAutoCircleOrbit();
                body.LockOrbitEditing = true;
                //body.AttractorSettings.GravityConstant = GConstant;
                //body.OrbitData = new KeplerOrbitData(
                //                    position: new Vector3d((float)sat.PosX / 100, (float)sat.PosY / 100, (float)sat.PosZ / 100),
                //                    velocity: new Vector3d((float)sat.VelX * 100, (float)sat.VelY * 100, (float)sat.VelZ * 100 ),
                //                    attractorMass: attractorMass,
                //                    gConst: GConstant);
                //body.ForceUpdateViewFromInternalState();
                //Debug.Log(sat.mean_anomaly);
                //GetPosition(satilite, sat, body);
                //body.enabled = false;
                satilite.transform.parent = this.transform;
                i++;
            }
        }
        StateOfMachine.Instance.SetSate = false;
    }

    private void Update()
    {
        
    }

    //private void GetPosition(GameObject sat_obj, Satalite_cord sat_n, SimpleKeplerOrbits.KeplerOrbitMover orbit)
    //{
    //    List<Vector3> tmp = new List<Vector3>();
    //    double starting_anomaly;
    //    int i;
    //    int count;
    //    int increment;
    //    bool full_path;

    //    starting_anomaly = orbit.OrbitData.MeanAnomaly;
    //    i = 0;
    //    count = 0;
    //    increment = 1;
    //    Debug.Log(starting_anomaly);
    //    Debug.Log(i);
    //    full_path = false;
    //    while ((orbit.OrbitData.MeanAnomaly != starting_anomaly || i == 0) && count < 5)
    //    {


    //        tmp.Add(new Vector3());
    //        tmp[i] = sat_obj.transform.position;
    //        Debug.Log(tmp[i]);
    //        Debug.Log(orbit.OrbitData.MeanAnomaly);
    //        i++;
    //        //}
    //        //Debug.Log(count);
    //        if (orbit.OrbitData.MeanAnomaly == starting_anomaly && i > 0)
    //        {
    //            full_path = true;
    //        }
    //        count++;
    //        //Debug.Log(orbit.OrbitData.MeanAnomaly);
    //        orbit.OrbitData.MeanAnomaly += increment;
    //        if (orbit.OrbitData.MeanAnomaly + increment > 6.1)
    //        {
    //            orbit.OrbitData.MeanAnomaly = 6.1 - orbit.OrbitData.MeanAnomaly;
    //        }

    //    }
    //    sat_n.CirclePath = tmp;
    //}
}
