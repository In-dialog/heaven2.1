using UnityEngine;
using SimpleKeplerOrbits;
using System.IO;
using Zeptomoby.OrbitTools;
using System.Runtime.Serialization.Formatters.Binary;
using System;


public class Satalite_manager : MonoBehaviour
{
    public GameObject   Satelatite;
    public Transform    Atractor;
    public float        attractorMass;
    public int          nr_sat = 30;

    public string Folder_Name;
    public string Name_of_file;

    string savePath;
    ///Users/segalcodin/Documents/Sat_data_base
    void Start()
    {
        savePath = Application.dataPath + "/Saves";
        if (File.Exists(savePath + "/save.binary" + this.transform.name))
        {
            LoadData();
        }
        string appPath = "";
        var info = new DirectoryInfo(Application.dataPath);
        var fileInfo = info.GetFiles();
        foreach (var item in fileInfo)
        {
            if (item.FullName.Contains(".txt")& !item.FullName.Contains(".meta"))
                appPath = item.FullName;
        }
        if (File.Exists(appPath))
        {
            string[] lines = File.ReadAllLines(appPath);
            for (int i = 0; i < (lines.Length - (lines.Length - nr_sat)) - 2; i += 2)
            {
                Vector3 position;
                Vector3 velocity;
                Satellite sat = new Satellite(new Tle("SGP4", lines[i], lines[i + 1]));
                Eci eci = sat.PositionEci(360);
                position.x = (float)eci.Position.X / 100;
                position.y = (float)eci.Position.Y / 100;
                position.z = (float)eci.Position.Z / 100;
                velocity.x = (float)eci.Velocity.X;
                velocity.y = (float)eci.Velocity.Y;
                velocity.z = (float)eci.Velocity.Z;

                GameObject satilite = Instantiate(Satelatite);
                satilite.name = "A" + i;
                satilite.transform.position = position;
                KeplerOrbitMover body = satilite.GetComponent<KeplerOrbitMover>();
                body.AttractorSettings.AttractorObject = Atractor;
                body.AttractorSettings.AttractorMass = attractorMass;
                body.CreateNewOrbitFromPositionAndVelocity(body.transform.position, velocity);
                body.SetAutoCircleOrbit();
                body.LockOrbitEditing = true;
                satilite.transform.parent = this.transform;
            }
            StateOfMachine.Instance.SetSate = false;
        }
    }

    public void LoadData()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string filePath = savePath + "/save.binary" + this.transform.name;
        FileStream saveFile = File.Open(filePath, FileMode.Open);
        nr_sat = (int)formatter.Deserialize(saveFile);
        saveFile.Close();
    }
}
