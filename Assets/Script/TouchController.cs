using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//将恒星位置置于ControlPanel原点，方便处理
public class TouchController : MonoBehaviour
{
    public float RayCheckDistance = 100;
    private Transform touchedObject;
    public Transform virtualPlane; //用于为小行星定位
    private float floattingSpeed = 1;

    public static float LONG_TOUCH_TRIGGER_TIME = 1; //长按1s触发长按
    private bool touchedDown = false; //长按状态标志
    private float touchedTime = 0;
    private Vector3 touchedPos;

    //参数1表示序号，参数2表示值
    public Action<int, int> OnValueChanged;

    private int[] lastValues;
    public Planet[] Planets;
    private int selectedPlanet = -1;

    public Transform Sun;
    private Renderer sunRenderer;
    private Color orignColor;
    private Coroutine highlightCor;
    private bool panelOpened = false;

    private bool floattingUp = false;
    private Vector3 floattingOrignPos; //初始位置
    private Vector3 floattingDestPos; //上浮到目标位置
    private Coroutine floattingCor;

    void Awake()
    {
        for (int i = 0; i < Planets.Length; i++)
            Planets[i].gameObject.SetActive(false);
        lastValues = new int[Planets.Length];
        sunRenderer = Sun.GetComponent<Renderer>();
        floattingOrignPos = transform.localPosition;
        floattingDestPos = floattingOrignPos + new Vector3(0, 0.5f, 0);
        virtualPlane.GetComponent<Collider>().enabled = false;
    }

    void SwitchPanel()
    {
        panelOpened = !panelOpened;
        virtualPlane.GetComponent<Collider>().enabled = panelOpened;
        for (int i = 0; i < Planets.Length; i++)
        {
            //Planets[i].gameObject.SetActive(panelOpened);
            if (panelOpened)
                Planets[i].ShowSelf();
            else
                Planets[i].HideSelf();
        }
            
        //var dir = panelOpened ? 1 : -1;
        //StartCoroutine(Floating(dir));
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0)
        {
            var touch = Input.GetTouch(0);
            Check(touch.position);
        }
        

        if (touchedDown)
        {
            if (Input.mousePosition == touchedPos)
            {
                touchedTime += Time.deltaTime;
                if (touchedTime >= LONG_TOUCH_TRIGGER_TIME)
                {
                    Debug.Log("Long touch triggered on " + touchedObject.name);
                    touchedTime = 0;
                    touchedDown = false;
                    SwitchPanel();
                }
            }
            else
            {
                touchedTime = 0;
                touchedDown = false;
            }
        }
        else if(!panelOpened) //控制浮起
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, RayCheckDistance))
            {
                if(!floattingUp && hit.transform == Sun) //上浮
                {
                    Debug.Log("Uppppppppp");
                    if (floattingCor != null)
                        StopCoroutine(floattingCor);
                    floattingUp = true;
                    floattingCor = StartCoroutine(Floating(floattingDestPos));
                }
                else if(floattingUp && hit.transform != Sun) //下落
                {
                    Debug.Log("Downnnnnnnn");
                    if (floattingCor != null)
                        StopCoroutine(floattingCor);
                    floattingUp = false;
                    floattingCor = StartCoroutine(Floating(floattingOrignPos));
                }
            }
        }

        if (selectedPlanet >= 0)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, RayCheckDistance))
            {
                if (hit.transform == virtualPlane)
                {
                    var localPos = virtualPlane.InverseTransformPoint(hit.point);
                    Planets[selectedPlanet].transform.localPosition = localPos * Planets[selectedPlanet].trackRadius / localPos.magnitude;
                    var per = GetPercentage(Planets[selectedPlanet].transform.localPosition, Planets[selectedPlanet].trackRadius);
                    var value = (int)(per * 100 + 0.5);
                    if (lastValues[selectedPlanet] >= 99 && value == 0)
                        value = 100;
                    if (OnValueChanged != null)
                        OnValueChanged(selectedPlanet, value);
                    lastValues[selectedPlanet] = value;
                    Debug.Log("" + value);
                }
            }
        }

        if (Input.GetMouseButtonDown(0)) //鼠标点下时检测点击到的物体
        {
            touchedPos = Input.mousePosition;
            Check(touchedPos);
        }
        else if (Input.GetMouseButtonUp(0)) //鼠标按键释放时清除状态
        {
            touchedDown = false;
            touchedTime = 0;
            selectedPlanet = -1;
            Sun.GetComponent<Collider>().enabled = true;
            if (highlightCor != null)
            {
                StopCoroutine(highlightCor);
                sunRenderer.material.color = orignColor;
            }
        }
    }

    float GetPercentage(Vector3 pos, float radius)
    {
        //var tan = Mathf.Abs(pos.y / pos.x);
        var sin = pos.z / radius;
        var d = Mathf.Asin(sin);

        if (pos.x > 0)
            d = Mathf.PI - d;
        else if (pos.x < 0 && pos.z < 0)
            d = 2 * Mathf.PI + d;

        Debug.Log("sin:" + sin + ", d:" + d);
        return d / 6.28318548f;
    }

    void Check(Vector3 pos)
    {
        Ray ray = Camera.main.ScreenPointToRay(pos);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, RayCheckDistance))
        {
            touchedObject = hit.transform;
            Debug.Log("Ray: " + touchedObject.name + ", " + hit.point);

            if (Sun == hit.transform)
            {
                touchedDown = true;
                highlightCor = panelOpened ? StartCoroutine(Hightlighting(-1)) : StartCoroutine(Hightlighting(1));
            }
            else //点击位置与行星距离小于半径时认为选中行星
            {
                for (int i = 0; i < Planets.Length; i++)
                {
                    var distance = (hit.point - Planets[i].transform.position).magnitude;
                    if (distance < Planets[i].planetRadius)
                    {
                        selectedPlanet = i;
                        Debug.Log("selected " + i + ":" + Planets[i].name);
                    }
                }
            }

            if (selectedPlanet > -1)
            {
                Sun.GetComponent<Collider>().enabled = false;
            }
        }
    }


    //
    IEnumerator Floating(Vector3 destPos)
    {
        Debug.Log("Move to " + destPos);
        var wait = new WaitForSeconds(0.02f);
        Vector3 pos = transform.localPosition;
        var direction = destPos.y > pos.y ? 1 : -1;
        var distance = (destPos.y - pos.y) * direction;
        float tmp = 0;
        float delta = 0;
        var color = sunRenderer.material.color;
        while (tmp < distance)
        {
            delta = floattingSpeed * Time.deltaTime * direction;
            pos.y += delta;
            tmp += delta * direction;
            transform.localPosition = pos;

            yield return wait;
        }
    }

    IEnumerator Hightlighting(int direction)
    {
        var wait = new WaitForSeconds(0.02f);
        var material = sunRenderer.material;
        var color = material.color;
        orignColor = material.color;
        var tmp = 0f;
        var delta = 1f / 255f * direction;
        while (tmp < LONG_TOUCH_TRIGGER_TIME)
        {
            tmp += 0.02f;
            color.r += delta;
            color.g += delta;
            color.b += delta;
            material.color = color;
            yield return wait;
        }
    }

}
