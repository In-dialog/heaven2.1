using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class GraphicElements : MonoBehaviour
{
    public GameObject prefabCenter;
    public GameObject prefabLines;
    public GameObject prefabText;

    public Material mat;
    int _step = 10;
    public int _step2 = 90;

    int count,count2,count3;
    public int stepX =140, stepY;
    LineRenderer lineRenderer;
    GameObject lineHolder;
    Vector3[] dots;
    GameObject MasterHolder;

    public LineRenderer lr;
    public GameObject gides;
    List<float> radiouses = new List<float>();
    // Start is called before the first frame update
    void Start()
    {
        dots = new Vector3[_step];
        MasterHolder = new GameObject("Master");
        MasterHolder.AddComponent<DimOnDistroy>();
        var angle = gides.transform.rotation;

        for (int i = 0; i < 11; i++)
        {
         GameObject temp = Instantiate(gides);
         temp.transform.position = new Vector3((i * 10) - 50, - 90, 0);
            temp.transform.rotation = angle;
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (radiouses.Count > 1)
        {
            lr.positionCount = radiouses.Count;
            lr.SetPosition(0, new Vector3( - 50, - 100, 0));

            for (int i = 0; i < lr.positionCount; i++)
            {
                lr.SetPosition(i, new Vector3((i * 10)-50, (radiouses[i]*0.3f)-100, 0));
            }
        }
 
    }
    

    public void InstanceObject(LineProperties lP)
    {
        if (lP.type == "Arc")
        {
            if(count % 3 == 0 & count!=0)
            {
                //Destroy(GameObject.Find("Master"));
                MasterHolder.GetComponent<DimOnDistroy>().DestroyMe();
                MasterHolder = new GameObject("Master");
                MasterHolder.AddComponent<DimOnDistroy>();
            }
            GameObject center = new GameObject("Center" + count++);
            center.transform.SetParent(MasterHolder.transform);
            CreateKnots(lP.starPosition,lP.endPosition,lP.center.transform.position,center);
            GameObject _temp = Instantiate(prefabText);
            _temp.transform.SetParent(MasterHolder.transform);
            _temp.transform.position = lP.center.transform.position + Vector3.down;
            _temp.GetComponent<TextMeshPro>().text = lP.angle.ToString();
            if (radiouses.Count > 10)
            {
                radiouses.Clear();
                lr.positionCount = 1;
                lr.SetPosition(0, new Vector3( - 50, - 100, 0));
            }
            radiouses.Add(lP.angle);
        }


    }
    void CreateKnots(Vector3 sp,Vector3 ep,Vector3 cs, GameObject center)
    {
        float _scale = 0.8f;
        GameObject temp = Instantiate(prefabCenter);
        temp.transform.SetParent(center.transform);
        LineRenderer _lineRenderer = temp.AddComponent<LineRenderer>();
        temp.transform.position = cs;
        temp.transform.localScale *= _scale;
        _lineRenderer.positionCount = 3;
        _lineRenderer.SetPosition(0, sp);
        _lineRenderer.SetPosition(1, cs);
        _lineRenderer.SetPosition(2, ep);
        _lineRenderer.startWidth = _scale / 5;
        _lineRenderer.endWidth = _scale / 5;
        _lineRenderer.material = mat;
        temp = Instantiate(prefabLines);
        temp.transform.localScale *= _scale;
        temp.transform.SetParent(center.transform);
        temp.transform.position = sp;
        temp = Instantiate(prefabLines);
        temp.transform.localScale *= _scale;
        temp.transform.SetParent(center.transform);
        temp.transform.position = ep;
    }
  public void CreatePoints(LineProperties lp)
    {
        if (count2 == _step * 6)
        {
            count2 = 0;
        }
        float _scale = 0.18f;
        Vector3 sp = Vremap(lp.endPosition);
        Vector3 ep = Vremap(lp.starPosition);
        if (count2 % _step == 0 | count2 == 0)
        {
            if (count2 != 0)
            {
                Vector3[] temp = new Vector3[lineRenderer.positionCount];
                lineRenderer.GetPositions(temp);
                foreach (var item in temp)
                {
                    Dots(_scale, item);
                }
                stepY -= _step2;
            }

            if (GameObject.Find("lineHolder" + count2) == true)
            {
                Destroy(GameObject.Find("lineHolder" + count2));
            }

            lineHolder = new GameObject("lineHolder" + count2);
            lineRenderer = lineHolder.AddComponent<LineRenderer>();
            lineHolder.transform.SetParent(this.transform);
            lineRenderer.positionCount = 0;
            lineRenderer.material = mat;
   
        }
        if (lp.type == "Arc")
        {
            GameObject _temp = Instantiate(prefabText);
            _temp.transform.SetParent(lineHolder.transform);
            _temp.transform.position = ep + Vector3.down + Vector3.left;
            _temp.GetComponent<TextMeshPro>().text = lp.angle.ToString();
        }
        count2++;

        lineRenderer.positionCount += 1;
        if (lineRenderer.positionCount >= 2)
        {
            lineRenderer.SetPosition(lineRenderer.positionCount - 1, ep);
            lineRenderer.startWidth = _scale/2;
            lineRenderer.endWidth = _scale;
        }
        else
        {
            lineRenderer.SetPosition(0,FindObjectOfType<Mover>().transform.position);
        }

    }
    void Dots(float _scale, Vector3 sp)
    {
        GameObject temp = Instantiate(prefabLines);
        temp.transform.localScale *= _scale*4;
        temp.transform.SetParent(lineHolder.transform);
        temp.transform.position = sp;

    

    }

    Vector3 Vremap(Vector3 origin)
    {
        if (stepY < _step2*-1)
        {
            stepY = _step2;
        }
        Vector3 temp = new Vector3(origin.x* 0.8f, origin.y * 0.8f, 0);
        temp = new Vector3(temp.x -160, temp.y+stepY, 0);
        return temp;
    }


}
