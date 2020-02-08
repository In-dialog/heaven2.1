using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System;
using System.IO;


public class Satelite_Settings : MonoBehaviour
{
    public GameObject sat_man;

    public void Nr_Of_Satelites(string newText)
    {
        int temp = int.Parse(newText);
        sat_man.GetComponent<Satalite_manager>().nr_sat = temp;
        SaveData();
    }

    public void SaveData()
    {
        string savePath = Application.dataPath + "/Saves";
        
        if (!Directory.Exists(savePath))
            Directory.CreateDirectory(savePath);
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream saveFile = File.Create(savePath + "/save.binary" + sat_man.transform.name);
        formatter.Serialize(saveFile, sat_man.GetComponent<Satalite_manager>().nr_sat);
        saveFile.Close();
    }

}
