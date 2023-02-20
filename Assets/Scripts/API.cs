using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using QRFoundation;

public class API : MonoBehaviour
{
    public void GetBundleObject(string assetFileName, string rootUrl, UnityAction<GameObject> callback, Transform bundleParent)
    {
        StartCoroutine(GetDisplayBundleRoutine(assetFileName, rootUrl, callback, bundleParent));
    }

    IEnumerator GetDisplayBundleRoutine(string assetFileName, string url, UnityAction<GameObject> callback, Transform bundleParent)
    {
        string bundleURL = $"{url}/asset-bundle/";

        // Append platform to asset bundle name
#if UNITY_ANDROID
        bundleURL += "android/";
#else
        bundleURL += "ios/";
#endif
        bundleURL += assetFileName;
        MainSceneUIController.S.SetDialogText($"Requesting bundle...");

        // Request asset bundle
        UnityWebRequest www = UnityWebRequestAssetBundle.GetAssetBundle(bundleURL);
        yield return www.SendWebRequest();
        if (www.result == UnityWebRequest.Result.ConnectionError)
        {
            QRCodeTracker.S.downloadingRegisteredString = "";
            MainSceneUIController.S.SetDialogText("Download model failed:\nNetwork error");
        }
        else
        {
            MainSceneUIController.S.OnClickButtonOk();
            AssetBundle ab = DownloadHandlerAssetBundle.GetContent(www);
            if (ab != null)
            {
                string rootAssetPath = ab.GetAllAssetNames()[0];
                GameObject bundleObj = ab.LoadAsset(rootAssetPath) as GameObject;
                QRCodeTracker.S.prefab = bundleObj;
                ab.Unload(false);
                callback(bundleObj);
            }
            else
            {
                MainSceneUIController.S.SetDialogText("Invalid asset, please try again later");
            }
        }
    }
}
