using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class LongPressTrigger : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
{
    private static float LONG_PRESSS_TIME = 0.6f;
    private WaitForSeconds waitForLongPress = new WaitForSeconds(0.02f);
    private Coroutine mCounterCor;

    public Action OnLongPressTriggered;

    void Start()
    {

    }

    void Update()
    {

    }

    IEnumerator LongPressCounter()
    {
        float count = 0;
        while(count < LONG_PRESSS_TIME)
        {
            yield return waitForLongPress;
            count += 0.02f;
        }
        Debug.Log("LongPress on " + gameObject.name + " triggered");
        if (OnLongPressTriggered != null)
            OnLongPressTriggered();
    }

    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        mCounterCor = StartCoroutine(LongPressCounter());
    }

    void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
    {
        StopCoroutine(mCounterCor);
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        StopCoroutine(mCounterCor);
    }
}
