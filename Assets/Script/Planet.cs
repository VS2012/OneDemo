using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Planet : MonoBehaviour
{
    public float planetRadius;
    public float trackRadius;
    public Transform track;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ShowSelf()
    {
        gameObject.SetActive(true);
        StartCoroutine(ScaleTrack(trackRadius * 2));
    }

    public void HideSelf()
    {
        StartCoroutine(ScaleTrack(1));
    }

    IEnumerator ScaleTrack(float destScale)
    {
        var wait = new WaitForSeconds(0.02f);
        var duration = 0.5f;
        var scale = track.localScale;
        var speed = (scale.x - destScale) / duration;
        //var direction = scale.x > destScale ? 1 : -1;
        float time = 0;
        while(time < duration)
        {
            scale += Vector3.one * speed * -0.02f;
            time += 0.02f;
            track.localScale = scale;
            yield return wait;
        }
    }
}
