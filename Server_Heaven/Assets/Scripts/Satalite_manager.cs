using UnityEngine;
using SimpleKeplerOrbits;
using System.IO;
using Zeptomoby.OrbitTools;

public class Satalite_manager : MonoBehaviour
{
    public GameObject   Satelatite;
    public Transform    Atractor;
    public float        attractorMass;
    public int          nr_sat;
    public string       User_name;
    public string MainDirectory;
    public string Directory_containing_database;
    public string Name_of_file;


    ///Users/segalcodin/Documents/Sat_data_base
    void Start()
    {

        //FileInfo theSourceFile = new FileInfo(path_to_database + "satelites.txt");
        string[] lines = File.ReadAllLines("/Users/" + User_name + "/" + MainDirectory + "/" + Directory_containing_database +"/"+ Name_of_file + ".txt");
        int cnt = lines.Length;
        for (int i = 0; i < (lines.Length - (lines.Length - nr_sat)) - 2; i +=2)
        {
            Vector3 position;
            Vector3 velocity;
            Satellite sat = new Satellite(new Tle("SGP4", lines[i], lines[i+1]));
            Eci eci = sat.PositionEci(360);
            position.x = (float)eci.Position.X/100;
            position.y = (float)eci.Position.Y/100;
            position.z = (float)eci.Position.Z/100;
            velocity.x = (float)eci.Velocity.X;
            velocity.y = (float)eci.Velocity.Y;
            velocity.z = (float)eci.Velocity.Z;

            GameObject satilite = Instantiate(Satelatite);
            satilite.name = "A" + i;
            satilite.transform.position = position;
            KeplerOrbitMover body = satilite.GetComponent<KeplerOrbitMover>();
            body.AttractorSettings.AttractorObject = Atractor;
            body.AttractorSettings.AttractorMass = attractorMass;
            body.CreateNewOrbitFromPositionAndVelocity(body.transform.position,velocity);
            body.SetAutoCircleOrbit();
            body.LockOrbitEditing = true;
            satilite.transform.parent = this.transform;
        }
        StateOfMachine.Instance.SetSate = false;
    }
 
 
}
