using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class CollectionListManager : MonoBehaviour
{
    public GameObject lineOfList;
    public RectTransform socket;
    public TMP_Text text_title;
    public TMP_Text text_hint;

    public Animator hintbarAnimator;

    private List<Exhibit.exhibit> exhibitList;
    private bool isHintbarFolded;
    // Start is called before the first frame update
    void Start()
    {
        exhibitList = new List<Exhibit.exhibit>();
        isHintbarFolded = true;
        SetCollectionList();
    }

    // Update is called once per frame
    void Update()
    {
        if (isHintbarFolded)
            hintbarAnimator.SetBool("IsHintBarFold", true);
        else 
            hintbarAnimator.SetBool("IsHintBarFold", false);
    }

    void SetCollectionList()
    {
        if (PlayerPrefs.HasKey("xmlfile"))
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(PlayerPrefs.GetString("xmlfile"));
            // Get root element
            XmlElement root = xmlDoc.DocumentElement;
            // Get all the exhibit node
            XmlNodeList exhibitNodeList = root.SelectSingleNode("exhibit").ChildNodes;
            for (int i = 0; i < exhibitNodeList.Count; i++)
            {
                string id = "", status = "", title = "", subtitle = "", author = "", description = "", fileName = "";
                for (int j = 0; j < exhibitNodeList[i].ChildNodes.Count; j++)
                {
                    switch (exhibitNodeList[i].ChildNodes[j].Name)
                    {
                        case "id":
                            id = exhibitNodeList[i].ChildNodes[j].InnerText;
                            break;
                        case "status":
                            status = exhibitNodeList[i].ChildNodes[j].InnerText;
                            break;
                        case "title":
                            title = exhibitNodeList[i].ChildNodes[j].InnerText;
                            break;
                        case "subtitle":
                            subtitle = exhibitNodeList[i].ChildNodes[j].InnerText;
                            break;
                        case "author":
                            author = exhibitNodeList[i].ChildNodes[j].InnerText;
                            break;
                        case "description":
                            description = exhibitNodeList[i].ChildNodes[j].InnerText;
                            break;
                        case "fileName":
                            fileName = exhibitNodeList[i].ChildNodes[j].InnerText;
                            break;
                        default:
                            break;
                    }
                }
                exhibitList.Add(new Exhibit.exhibit(id, status, title, subtitle, author, description, fileName));
            }
        }
        else
            return;
        for (int i = 0; i < exhibitList.Count; i++)
        {
            if (exhibitList[i].IsActive == "on") 
            {
                GameObject insObj = Instantiate(lineOfList, socket);
                insObj.transform.Find("Text_Description").GetComponent<TextMeshProUGUI>().text =
                    $"<b>{exhibitList[i].Title}</b>\nby {exhibitList[i].Author}";
                insObj.name = exhibitList[i].ID;
                insObj.GetComponent<Button>().onClick.AddListener(() => OnClickLineOfCollectionList());
            }
        }
    }

    private void OnClickLineOfCollectionList()
    {
        isHintbarFolded = false;
        GameObject clickedLine = EventSystem.current.currentSelectedGameObject;
        for (int i = 0; i < exhibitList.Count; i++)
        {
            if (clickedLine.name == exhibitList[i].ID)
            {
                text_title.text = $"<b>{exhibitList[i].Title}</b> by {exhibitList[i].Author}";
                text_hint.text = exhibitList[i].Description;
                return;
            }
        }
    }

    public void OnClickButtonClose()
    {
        isHintbarFolded = !isHintbarFolded;
    }
}
