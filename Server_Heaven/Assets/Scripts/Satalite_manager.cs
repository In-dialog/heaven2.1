using UnityEngine;
using SimpleKeplerOrbits;
using System.IO;
using Zeptomoby.OrbitTools;

public class Satalite_manager : MonoBehaviour
{
    public GameObject Satelatite;
    public Transform Atractor;
    public float attractorMass;
    public int count;

    void Start()
    {

        FileInfo theSourceFile = new FileInfo("Assets/Resources/satelites.txt");
        string[] lines = File.ReadAllLines("Assets/Resources/satelites.txt");
        int cnt = lines.Length;
        for (int i = 0; i < (lines.Length-2); i +=2)
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
