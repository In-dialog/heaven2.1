using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class Test : MonoBehaviour
{
    public List<Vector3> satelite = new List<Vector3>();
    public List<float> distFromOnePoint = new List<float>();
    public List<int> pastIndex = new List<int>();

    int[,] bla;
    int lastResult;
    // Start is called before the first frame update
    void Start()
    {
     
        for (int i = 0; i < 10; i++)
        {
            satelite.Add(UnityEngine.Random.insideUnitCircle * 5);
        }
        
        int random = UnityEngine.Random.Range(0, satelite.Count);

        foreach (var item in satelite)
        {
            distFromOnePoint.Add(Vector3.Distance(item, satelite[random]));
        }
    }

    // Update is called once per frame
    void Update()
    {

        lastResult = FindNextIndex(satelite, bla.GetRow(lastResult));
        pastIndex.Add(lastResult);
    }

    int FindNextIndex(List<Vector3> list, int [] row)
    {
        int result = 0;
        List<int> temp =new List<int>();
        foreach (var item in row)
        {
            temp.Add(item);
        }
        List<int> temp2 = temp;
        temp.Sort((IComparer<int>)new List<int>());
        result=temp2.IndexOf(result);
        return result;
    }
}
public static class ArrayExt
{
    public static T[] GetRow<T>(this T[,] array, int row)
    {
        if (!typeof(T).IsPrimitive)
            throw new InvalidOperationException("Not supported for managed types.");

        if (array == null)
            throw new ArgumentNullException("array");

        int cols = array.GetUpperBound(1) + 1;
        T[] result = new T[cols];

        int size;

        if (typeof(T) == typeof(bool))
            size = 1;
        else if (typeof(T) == typeof(char))
            size = 2;
        else
            size = Marshal.SizeOf<T>();

        Buffer.BlockCopy(array, row * cols * size, result, 0, cols * size);

        return result;
    }
}