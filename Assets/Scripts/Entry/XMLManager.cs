using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Networking;
using QRFoundation;

public class XMLManager : MonoBehaviour
{
    #region References
    public string scannedLink;
    private string fileName;

    public class XMLLoadProgress
    {
        public xmlStatus status;
        // Attributes of this class
        public xmlStatus Status
        {
            get { return status; }
            set { status = value; }
        }
        public XMLLoadProgress(xmlStatus s) { status = s; }
    }
    public XMLLoadProgress xlp = new XMLLoadProgress(xmlStatus.scanning);

    public enum xmlStatus
    {
        scanning,
        detected,
        downloading,
        analyzing,
        done,
    }
    #endregion


    #region Unity lifetime activities
    private void Start()
    {
        // test
        //StartCoroutine(DownLoadXMLFile("https://armuse.oss-us-west-1.aliyuncs.com/cfg/armuse-test-cfg.xml"));
        PlayerPrefs.DeleteAll();
    }


    private void Update()
    {
        if (xlp.status == xmlStatus.scanning)
        {
            scannedLink = QRCodeTracker.S.registeredString;
            if (scannedLink != null && scannedLink.EndsWith("xml"))
            {
                fileName = scannedLink.Substring(scannedLink.LastIndexOf('/'), scannedLink.Length - scannedLink.LastIndexOf('/') - 1);
                // Detected a xml file
                xlp.Status = xmlStatus.detected;
                EntrySceneUIManager.S.AcitivePopup(xlp.Status);
                EntrySceneUIManager.S.PopupText($"Detected a config file\nDownloading from server...");
                // Download xml file
                StartCoroutine(DownLoadXMLFile(scannedLink));
            }
            else return;
        }
    }
    #endregion


    #region Tools
    IEnumerator DownLoadXMLFile(string url)
    {
        xlp.Status = xmlStatus.downloading;
        // Create a request
        UnityWebRequest request = UnityWebRequest.Get(url);
        // Wait for request to complete
        yield return request.SendWebRequest();
        if (request.result == UnityWebRequest.Result.ConnectionError)
        {
            EntrySceneUIManager.S.PopupText("Download failed:\nNetword error\nPlease try again later");
            StopCoroutine(DownLoadXMLFile(url));
        }
        else if (request.isDone)
        {
            string xml = request.downloadHandler.text;
            bool isValid = AnalyzeXML(xml);
            if (isValid)
            {
                // Save xml file path
                PlayerPrefs.SetString("xmlfile", xml);
                if (PlayerPrefs.HasKey("xmlfile"))
                    xlp.Status = xmlStatus.done;
                else
                {
                    EntrySceneUIManager.S.PopupText("Save failed:\nPlease try again later");
                }
            }
        }
    }

    private bool AnalyzeXML(string xml)
    {
        xlp.Status = xmlStatus.analyzing;
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(xml);
        // Get root element
        XmlElement root = xmlDoc.DocumentElement;
        //Debug.Log($"root: {root.Name}");
        if (root.Name != "museum")
        {
            EntrySceneUIManager.S.PopupText("Invalid config file");
            return false;
        }
        else
        {
            // Hashset for verifying integrity
            ISet<string> museumSet = new HashSet<string>();
            // Get museum config
            XmlNode museumNode = root.SelectSingleNode("museum");
            for (int i = 0; i < museumNode.ChildNodes.Count; i++)
            {
                //Debug.Log($"{museumNode.ChildNodes[i].Name}: {museumNode.ChildNodes[i].InnerText}");
                if (museumNode.ChildNodes[i].Name == "title")
                    EntrySceneUIManager.S.PopupText($"Welcome to\n<b>{museumNode.ChildNodes[i].InnerText}</b>");
                if (!museumSet.Contains(museumNode.ChildNodes[i].Name))
                    museumSet.Add(museumNode.ChildNodes[i].Name);
            }
            if (!(museumSet.Contains("title") && museumSet.Contains("subtitle") && museumSet.Contains("author") && museumSet.Contains("version") && museumSet.Contains("description") && museumSet.Contains("link") && museumSet.Count == 6))
            {
                EntrySceneUIManager.S.PopupText("Invalid config file");
                return false;
            }

            // Get others config
            XmlNodeList exhibitNodeList = root.SelectSingleNode("exhibit").ChildNodes;
            for (int i = 0; i < exhibitNodeList.Count; i++)
            {
                ISet<string> exhibitSet = new HashSet<string>();
                for (int j = 0; j < exhibitNodeList[i].ChildNodes.Count; j++)
                {
                    //Debug.Log($"{exhibitNodeList[i].ChildNodes[j].Name}: {exhibitNodeList[i].ChildNodes[j].InnerText}");
                    if (!exhibitSet.Contains(exhibitNodeList[i].ChildNodes[j].Name))
                        exhibitSet.Add(exhibitNodeList[i].ChildNodes[j].Name);
                }
                if (!(exhibitSet.Contains("id") && exhibitSet.Contains("status") && exhibitSet.Contains("title") && exhibitSet.Contains("subtitle") && exhibitSet.Contains("author") && exhibitSet.Contains("description") && exhibitSet.Contains("fileName") && exhibitSet.Count == 7))
                {
                    EntrySceneUIManager.S.PopupText("Invalid config file");
                    return false;
                }
            }
        }
        return true;
    }

    private void SaveXML(string xml)
    {
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(xml);
        string savePath = Application.dataPath + fileName;
        if (!Directory.Exists(savePath))
        {
            Directory.CreateDirectory(savePath);
        }
        xmlDoc.Save(savePath);
    }
    #endregion
}
