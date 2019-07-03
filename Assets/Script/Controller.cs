using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    private GameObject mCenterBtn;
    private bool panelOpened = false;


    void Awake()
    {
        mCenterBtn = transform.Find("CenterBtn").gameObject;
        mCenterBtn.GetComponent<LongPressTrigger>().OnLongPressTriggered += () => {
            panelOpened = !panelOpened;
            Refresh();
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
