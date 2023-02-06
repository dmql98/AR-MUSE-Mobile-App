using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

public class ButtonTestEvents : MonoBehaviour
{
    public ContentController cc;
    // Just for test, asset bundles will not be found in build ver, use Application.StreamingAssetsPath() instead
    //public string cubePath = "AssetBundles/cube.ab";
    //public string spherePath = "AssetBundles/sphere.ab";

    public void InstantiateCube()
    {
        /*
        AssetBundle bundle = AssetBundle.LoadFromFile(cubePath);
        Object[] obj = bundle.LoadAllAssets<GameObject>();
        foreach (Object o in obj)
        {
            Instantiate(o);
        }
        */
        cc.LoadContent("cube");
    }

    public void InstantiateSphere()
    {
        /*
        AssetBundle bundle = AssetBundle.LoadFromFile(spherePath);
        GameObject obj = bundle.LoadAsset<GameObject>("sphere");
        Instantiate(obj);
        */
        cc.LoadContent("sphere");
    }
}
