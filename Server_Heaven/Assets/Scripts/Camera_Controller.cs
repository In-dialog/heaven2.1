using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System;
using System.IO;

public class Camera_Controller : MonoBehaviour
{
    public List<Camera_KeyFrames> CK;

    public float lerpTime = 5;
    public bool Remove_Last_Position;
    public int Case;
    public bool Free_Mode;
    public bool Animation_Loop;
    public bool Delete_KeyFrames;

    public Transform currentView;
    static Vector3 StaticPos;
    static Quaternion StaticRot;

    public float sensitivityX = 15F;
    public float sensitivityY = 15F;

    public float minimumX = -360F;
    public float maximumX = 360F;

    public float minimumY = -60F;
    public float maximumY = 60F;

    public float Speed;

    float rotationY = 0F;
    int i;

    string savePath;
    // Start is called before the first frame update
    void Start()
    {
        i = 0;
        savePath = Application.dataPath + "/Saves";
        if (File.Exists(savePath + "/saveValues.binary" + this.transform.name))
        {
            LoadValues();
        }
        if (File.Exists(savePath + "/save.binary" + this.transform.name))
        {
            LoadKeyFrames();
        }
        else
        {
            CK = new List<Camera_KeyFrames>();
        }
        StaticPos = transform.position;
        StaticRot = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        switch (Case)
        {
            case 0:
                transform.position = StaticPos;
                transform.rotation = StaticRot;
                break;
            case 1:
                AnimationLoop();
                break;

            case 2:
                FreeMode();
                break;
        }
        if (Delete_KeyFrames == true)
        {
            Delete_file();
            Delete_KeyFrames = false;
        }
        if (Remove_Last_Position == true)
        {
            RemoveLastPosition();
            SaveKeyFrames();
            Remove_Last_Position = false;
        }
    }

    private void LateUpdate()
    {
        if (Case == 1)
        {
            transform.position = Vector3.Lerp(transform.position, currentView.position, Time.deltaTime * lerpTime);
            Vector3 currentAngle = new Vector3
                (
                    Mathf.LerpAngle(transform.rotation.eulerAngles.x, currentView.rotation.eulerAngles.x, Time.deltaTime * lerpTime),
                    Mathf.LerpAngle(transform.rotation.eulerAngles.y, currentView.rotation.eulerAngles.y, Time.deltaTime * lerpTime),
                    Mathf.LerpAngle(transform.rotation.eulerAngles.z, currentView.rotation.eulerAngles.z, Time.deltaTime * lerpTime)
                );

            transform.eulerAngles = currentAngle;
        }

    }

    void FreeMode()
    {
            float rotationX = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * sensitivityX;

            rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
            rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);

            transform.localEulerAngles = new Vector3(-rotationY, rotationX, 0);
            transform.Translate(Vector3.forward * Input.GetAxis("Vertical") * Speed * Time.deltaTime);
            transform.Translate(Vector3.right * Input.GetAxis("Horizontal") * Speed * Time.deltaTime);
        if (Input.GetKeyDown(KeyCode.K))
        {
            AddPosition(this.transform.position, this.transform.eulerAngles);
            SaveKeyFrames();
        }
    }

    void AnimationLoop()
    {
        if (i < CK.Count)
        {
            currentView.position = new Vector3(CK[i].Pos_X, CK[i].Pos_Y, CK[i].Pos_Z);
            currentView.eulerAngles = new Vector3(CK[i].Rot_X, CK[i].Rot_Y, CK[i].Rot_Z);
            if (Vector3.Distance(transform.position, currentView.position) < 0.5f)
            {
                i++;
            }
            if (i == CK.Count)
            {
                i = 0;
            }
        }
    }

    void AddPosition(Vector3 position, Vector3 rotation)
    {
        Camera_KeyFrames tmp = new Camera_KeyFrames();
        tmp.SetValues(position, rotation);
        Debug.Log("This is my position" + position);
        Debug.Log("This is my rotation" + rotation);
        CK.Add(tmp);
    }

    void RemoveLastPosition()
    {
        CK.Remove(CK[CK.Count - 1]);
    }

    void Delete_file()
    {
        if (File.Exists(savePath + "/save.binary" + this.transform.name))
        {
            File.Delete("Saves/save.binary" + this.transform.name);
            CK.Clear();
            CK = new List<Camera_KeyFrames>();
        }
    }

    public void SaveKeyFrames()
    {
        if (!Directory.Exists(savePath))
            Directory.CreateDirectory(savePath);
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream saveFile = File.Create(savePath + "/save.binary" + this.transform.name);
        formatter.Serialize(saveFile, CK);

        saveFile.Close();
    }

    public void LoadKeyFrames()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string filePath = savePath + "/save.binary" + this.transform.name;
        FileStream saveFile = File.Open(filePath, FileMode.Open);
        CK = (List<Camera_KeyFrames>)formatter.Deserialize(saveFile);
        saveFile.Close();
    }

    public void LoadValues()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string filePath = savePath + "/saveValues.binary" + this.transform.name;
        FileStream saveFile = File.Open(filePath, FileMode.Open);
        lerpTime = (float)formatter.Deserialize(saveFile);
        saveFile.Close();
    }
}
