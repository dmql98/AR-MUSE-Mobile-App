using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

public class API : MonoBehaviour
{
    const string BundleFolder = "";
    public void GetBundleObject(string assetName, UnityAction<GameObject> callback, Transform bundleParent)
    {
        StartCoroutine(GetDisplayBundleRoutine(assetName, callback, bundleParent));
    }

    IEnumerator GetDisplayBundleRoutine(string assetName, UnityAction<GameObject> callback, Transform bundleParent)
    {
        string bundleURL = BundleFolder + assetName + "-";

        // Append platform to asset bundle name
#if UNITY_ANDROID
        bundleURL += "Android";
#else
        bundleURL += "IOS";
#endif
        Debug.Log($"Requesting bundle at {bundleURL}");


        // Request asset bundle
        UnityWebRequest www = UnityWebRequestAssetBundle.GetAssetBundle(bundleURL);
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.ConnectionError)
            Debug.Log("Network error");
        else
        {
            AssetBundle ab = DownloadHandlerAssetBundle.GetContent(www);
            if (ab != null)
            {
                string rootAssetPath = ab.GetAllAssetNames()[0];
                GameObject arObject = Instantiate(ab.LoadAsset(rootAssetPath) as GameObject, bundleParent);
                ab.Unload(false);
                callback(arObject);
            }
            else
                Debug.Log("Not a valid asset bundle");
        }
    }
}
