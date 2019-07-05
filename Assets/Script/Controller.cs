using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Controller : MonoBehaviour
{
    private GameObject mCenterBtn;
    private bool panelOpened = false;

    private Text paramA;
    private Text paramB;

    private CircleMoveController Planet1;

    void Awake()
    {
        mCenterBtn = transform.Find("CenterBtn").gameObject;
        mCenterBtn.GetComponent<LongPressTrigger>().OnLongPressTriggered += () => {
            panelOpened = !panelOpened;
            Refresh();
        };
        paramA = transform.Find("ParamPanel/ParamA/Value").GetComponent<Text>();
        Planet1 = transform.Find("Planet1").GetComponent<CircleMoveController>();

        Planet1.OnPosChange += (value) =>
        {
            paramA.text = string.Format("{0:N2}", value / (Mathf.PI*2));
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
