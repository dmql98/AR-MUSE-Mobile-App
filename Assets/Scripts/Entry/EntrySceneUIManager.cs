using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using QRFoundation;
using UnityEngine.UI;

public class EntrySceneUIManager : MonoBehaviour
{
    public static EntrySceneUIManager S;

    private void Awake()
    {
        S = this;
    }

    public XMLManager xmlManager;
    public GameObject popup;
    public TMP_Text popupText;
    public GameObject btn_enter;

    // Start is called before the first frame update
    void Start()
    {
        btn_enter.SetActive(false);
        popup.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (xmlManager.xlp.Status == XMLManager.xmlStatus.done)
        {
            btn_enter.SetActive(true);
        }
    }

    /// <summary>
    /// Active popup
    /// </summary>
    /// <param name="status">the status of downloading a xml file from server</param>
    public void AcitivePopup(XMLManager.xmlStatus status)
    {
        if (status != XMLManager.xmlStatus.scanning)
            popup.SetActive(true);
    }

    public void OnClickPopupExit()
    {
        popup.SetActive(false);
        StopAllCoroutines();
        xmlManager.xlp.Status = XMLManager.xmlStatus.scanning;
        xmlManager.scannedLink = null;
        QRCodeTracker.S.Reset();
    }


    public void PopupText(string s)
    {
        if (popup.activeSelf)
        {
            popupText.text = s;
        }
    }
}
