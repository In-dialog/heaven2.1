using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System;
using System.IO;
using UnityEngine.UI;

public class Camera_Settings : MonoBehaviour
{
    public GameObject cam;
    public GameObject ToggleStatic;
    public GameObject ToggleFreemode;
    public GameObject ToggleAnimation;

    public void SpeedOfCamera(string newText)
    {
        float temp;
        if (float.TryParse(newText, out temp))
        {
            cam.GetComponent<Camera_Controller>().lerpTime = temp;
            SaveData();
        }
    }

    public void Toggle_Animation(bool NewValue)
    {
        if (NewValue == true)
        {
            cam.GetComponent<Camera_Controller>().Case = 1;
            ToggleStatic.GetComponent<Toggle>().isOn = false;
            ToggleFreemode.GetComponent<Toggle>().isOn = false;
        }

    }

    public void Toggle_Static(bool NewValue)
    {
        if (NewValue == true)
        {
            cam.GetComponent<Camera_Controller>().Case = 0;
            ToggleAnimation.GetComponent<Toggle>().isOn = false;
            ToggleFreemode.GetComponent<Toggle>().isOn = false;
        }
    }

    public void Toggle_FreeMode(bool NewValue)
    {
        if (NewValue == true)
        {
            cam.GetComponent<Camera_Controller>().Case = 2;
            ToggleStatic.GetComponent<Toggle>().isOn = false;
            ToggleAnimation.GetComponent<Toggle>().isOn = false;
        }
    }

    public void Delete_KeyFrames()
    {
        cam.GetComponent<Camera_Controller>().Delete_KeyFrames = true;
    }

    public void Delete_LastPosition()
    {
        cam.GetComponent<Camera_Controller>().Remove_Last_Position = true;
    }
    public void SaveData()
    {
        string savePath = Application.dataPath + "/Saves";

        if (!Directory.Exists(savePath))
            Directory.CreateDirectory(savePath);
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream saveFile = File.Create(savePath + "/saveValues.binary" + cam.transform.name);
        formatter.Serialize(saveFile, cam.GetComponent<Camera_Controller>().lerpTime);
        saveFile.Close();
    }
}
