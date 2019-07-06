using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Controller : MonoBehaviour
{
    //private GameObject mCenterBtn;
    //private bool panelOpened = false;

    private Text[] Params;
    private Text paramB;
    public TouchController touchController;

    //private CircleMoveController Planet1;

    void Awake()
    {
        //mCenterBtn = transform.Find("CenterBtn").gameObject;
        //mCenterBtn.GetComponent<LongPressTrigger>().OnLongPressTriggered += () => {
        //    panelOpened = !panelOpened;
        //    Refresh();
        //};
        Params = new Text[2];
        Params[0] = transform.Find("ParamPanel/ParamA/Value").GetComponent<Text>();
        Params[1] = transform.Find("ParamPanel/ParamB/Value").GetComponent<Text>();
        //Planet1 = transform.Find("Planet1").GetComponent<CircleMoveController>();

        touchController.OnValueChanged += (index, value) =>
        {
            //paramA.text = string.Format("{0:N2}", value * 100);
            Params[index].text = value + "";
        };
    }
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void Refresh()
    {

    }
}
