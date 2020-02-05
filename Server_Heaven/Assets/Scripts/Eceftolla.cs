using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Eceftolla : MonoBehaviour
{
    public static double a = 6378137;
    public static double f = 0.0034;
    public static double b = 6.3568e6;
    public static double e = Mathf.Sqrt((Mathf.Pow((float)a, 2) - Mathf.Pow((float)b, 2)) / Mathf.Pow((float)a, 2));
    public static double e2 = Mathf.Sqrt((Mathf.Pow((float)a, 2) - Mathf.Pow((float)b, 2)) / Mathf.Pow((float)b, 2));

    public double[] ecef2lla(double x, double y, double z)
    {

        double[] lla = { 0, 0, 0 };
        double lat, lon, height, N, theta, p;

        p = Mathf.Sqrt(Mathf.Pow((float)x, 2) + Mathf.Pow((float)y, 2));

        theta = Mathf.Atan2((float)(z * a) , (float)(p * b));

        lon = Mathf.Atan2((float)y , (float)x);

        lat = Mathf.Atan2((float)(z + Mathf.Pow((float)e2, 2) * b * Mathf.Pow(Mathf.Sin((float)theta), 3)) , (float)(p - Mathf.Pow((float)e, 2) * a * Mathf.Pow(Mathf.Cos((float)theta), 3)));
        N = (float)a / (Mathf.Sqrt(1 - (Mathf.Pow((float)e, 2) * Mathf.Pow(Mathf.Sin((float)lat), 2))));

        double m = (p / Mathf.Cos((float)lat));
        height = m - N;


        lon = lon * 180 / Mathf.PI;
        lat = lat * 180 / Mathf.PI;
        lla[0] = lat;
        lla[1] = lon;
        lla[2] = height;
        return lla;
    }
}
