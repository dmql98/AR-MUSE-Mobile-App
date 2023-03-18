using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QRFoundation;
public class ContentController : MonoBehaviour
{
    public API api;

    public void LoadContent(string fileName)
    {
        //DestroyAllChildrenObject();
        api.GetBundleObject(fileName, PlayerPrefs.GetString("rootUrl"));
    }


    private void DestroyAllChildrenObject()
    {
        foreach(Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }
}
