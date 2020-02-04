using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System;
using System.IO;
using Zeptomoby.OrbitTools;

#pragma warning disable CS3009 // Base type is not CLS-compliant
public class Read_data : MonoBehaviour
#pragma warning restore CS3009 // Base type is not CLS-compliant
{

    public List<Satalite_cord> satlist = new List<Satalite_cord>();

    protected FileInfo theSourceFile = null;
    protected StreamReader reader = null;
    protected string text = " "; // assigned to allow first line to be read below
    protected string text_2 = " "; // assigned to allow first line to be read below

    int     i;
    int     count;

    void Start()
    {
        i = 0;
        count = 0;
        theSourceFile = new FileInfo("Assets/Resources/satelites.txt");
        reader = theSourceFile.OpenText();
        StateOfMachine.Instance.SetSate = false;
        GenerateList();
        Debug.Log(satlist.Count);
    }

    void Update()
    {
    }

    private void GenerateList()
    {
        while (text != null && text_2 != null)
        {
            if ((i == 0) && (text != null && text_2 != null))
            {
                satlist.Add(new Satalite_cord());
            }
            if (i < 2)
            {
                text = reader.ReadLine();
                text_2 = reader.ReadLine();
                satlist[count].line1 = text;
                i++;
                satlist[count].line2 = text_2;
                i++;
                if (text == null && text_2 == null)
                {
                    satlist.Remove(satlist[count]);
                    break;
                }
                Tle tle = new Tle(satlist[count].str1, satlist[count].line1, satlist[count].line2);
                if (tle != null)
                {
                    satlist[count].CreatePosVel(tle);
                }
                count++;
            }
            else if (i == 2)
            {
                i = 0;
            }
        }
    }

    public List<Satalite_cord> GetList()
    {
        return satlist;
    }
}


