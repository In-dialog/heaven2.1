using System.Collections;
using System.Collections.Generic;
using UnityEngine;


    public class LineProperties
    {
        public string type;
        public Vector3 starPosition, endPosition;
        public int LR;
        public Transform center;
        public float radious;
        public float angle;

        
    public LineProperties()
    {
    }

    public LineProperties(Vector3 starPosition, Vector3 endPosition, string type)
    {
        this.starPosition = starPosition;
        this.endPosition = endPosition;
        this.type = type;
    }

    public LineProperties(Vector3 starPosition, Vector3 endPosition, string type, float radious, int lR)
    {
        this.starPosition = starPosition;
        this.endPosition = endPosition;
        this.type = type;
        this.radious = radious;
        LR = lR;
    }

    public LineProperties(Vector3 sp, Vector3 ep, Transform c, string t, float r, int lr)
    {
        starPosition = sp;
        endPosition = ep;
        center = c;
        radious = r;
        type = t;
        LR = lr;
    }


}
