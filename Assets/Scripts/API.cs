using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using QRFoundation;

public class API : MonoBehaviour
{
    private QRCodeTracker qrt;
    private void Start()
    {
        qrt = GameObject.Find("AR Camera").GetComponent<QRCodeTracker>();
        //StartCoroutine(GetDisplayBundleRoutine("2-car.ab", "https://armuse.oss-us-west-1.aliyuncs.com/"));
    }


    public void GetBundleObject(string assetFileName, string rootUrl) => StartCoroutine(GetDisplayBundleRoutine(assetFileName, rootUrl));

    IEnumerator GetDisplayBundleRoutine(string assetFileName, string url)
    {
        string bundleURL = $"{url}/asset-bundle/";

        // Append platform to asset bundle name
#if UNITY_ANDROID
        bundleURL += "android/";
#else
        bundleURL += "ios/";
#endif
        bundleURL += assetFileName;

        // Request asset bundle
        UnityWebRequest uwr = UnityWebRequestAssetBundle.GetAssetBundle(bundleURL);
        MainSceneUIController.S.SetDialogText($"Requesting bundle...");
        // Send the request and then wait until it returns
        yield return uwr.SendWebRequest();
        if (uwr.result == UnityWebRequest.Result.ConnectionError)
        {
            MainSceneUIController.S.SetDialogText("Download model failed:\n<b>Network error!</b>");
        }
        else
        {
            AssetBundle ab = DownloadHandlerAssetBundle.GetContent(uwr);
            if (ab != null)
            {
                string rootAssetPath = ab.GetAllAssetNames()[0];
                GameObject bundleObj = ab.LoadAsset(rootAssetPath) as GameObject;
                ab.Unload(false);
                qrt.prefab = bundleObj;
                MainSceneUIController.S.OnClickButtonOk();
            }
            else
            {
                MainSceneUIController.S.SetDialogText("Invalid asset, please try again later.");
            }
        }
    }
}


