using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreatorSliders : MonoBehaviour
{
    public GameObject Object;
    // Start is called before the first frame update
    public Slider XPositionSlider;

    public Slider YPositionSlider;

    public Slider ZPositionSlider;

    public Slider VRotateSlider;

    public Slider HRotateSlider;

    public Slider ScaleSlider;

    // Update is called once per frame
    void Update()
    {
        Object.transform.position = new Vector3(XPositionSlider.value, YPositionSlider.value, ZPositionSlider.value);
        Object.transform.eulerAngles = new Vector3(VRotateSlider.value, HRotateSlider.value, 0);
        Object.transform.localScale = new Vector3(ScaleSlider.value, ScaleSlider.value, ScaleSlider.value);
    }
}
