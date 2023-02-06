using System.Collections;
using System.Collections.Generic;
using QRFoundation;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Xml;
using System.IO;
using TMPro;


public class MainSceneUIController : MonoBehaviour
{
    #region References
    public static MainSceneUIController S;
    private int activeModelIndex = 0;

    [Header("Button Visible")]
    [SerializeField]    private Image btn_visible_image;
    [SerializeField]    private Sprite spriteHide;
    [SerializeField]    private Sprite spriteShow;
                        private bool isUIVisible;
                        private List<GameObject> hideUIObjects;


    [Header("Menu")]
    [SerializeField]    private GameObject menu;
    [SerializeField]    private Sprite spriteMenu;
    [SerializeField]    private Sprite spriteClose;
                        private Image btn_menu_image;
                        private GameObject menuText;
                        private Animator menuTextAnimator;
                        private bool isMenuTextVisible;


    [Header("Bottombar")]
    [SerializeField]    private GameObject bottombar;
    [SerializeField]    private Sprite spriteExpand;
    [SerializeField]    private Sprite spriteShrink;
    [SerializeField]    private GameObject btn_clear;
    [SerializeField]    private TMP_Text title_text;
    [SerializeField]    private TMP_Text subtitle_text;
    [SerializeField]    private TMP_Text description_text;
                        private Image btn_expand_image;
                        private Image btn_clear_image;
                        private Animator bottombarAnimator;
                        private bool isBottombarExpand;


    [Header("Dialog")]
    [SerializeField]    private GameObject dialog;
    [SerializeField]    private TMP_Text dialog_text;


    [Header("Focusbar")]
    [SerializeField]    private GameObject focusbar;


    [Header("Classes")]
                        private List<Exhibit.exhibit> exhibitList;
                        private museum thisMuseum;

    [Header("Debug")]
    [SerializeField]    private TMP_Text debug_text;
    public class museum
    {
        string title;
        string subtitle;
        string author;
        string version;
        string description;
        string link;

        public string Title { get { return title; } }
        public string Subtitle { get { return subtitle; } }
        public string Author { get { return author; } }
        public string Version { get { return version; } }
        public string Description { get { return description; } }
        public string Link { get { return link; } }

        public museum(string t, string st, string a, string v, string d, string l)
        {
            title = t;
            subtitle = st;
            author = a;
            version = v;
            description = d;
            link = l;
        }
    }
    #endregion


    #region Unity Methods
    private void Awake()
    {
        S = this;    
    }

    // Start is called before the first frame update
    private void Start()
    {
        isUIVisible = true;
        hideUIObjects = new List<GameObject>();
        hideUIObjects.Add(menu);
        hideUIObjects.Add(bottombar);
        hideUIObjects.Add(focusbar);

        isMenuTextVisible = false;
        btn_menu_image = GameObject.Find("Btn_Menu").GetComponent<Image>();
        menuText = GameObject.Find("MenuText");
        menuTextAnimator = menuText.GetComponent<Animator>();
        menuText.SetActive(false);

        isBottombarExpand = false;
        btn_expand_image = GameObject.Find("Btn_Expand").GetComponent<Image>();
        bottombarAnimator = bottombar.GetComponent<Animator>();

        dialog.SetActive(false);

        exhibitList = new List<Exhibit.exhibit>();

        InitializeXML();

        SetMuseumInfo();
    }

    // Update is called once per frame
    private void Update()
    {
        //SetDebugText("//Debug RS: " + QRCodeTracker.S.registeredString);
        if (QRCodeTracker.S.registeredString != null && QRCodeTracker.S.registeredString.Trim().EndsWith(".ab"))
        {
            // Found assetbundle link
            SetTitleAndDescriptionText(QRCodeTracker.S.registeredString.Trim());
        }
        else
        {
            SetMuseumInfo();
        }
    }
    #endregion


    #region Tools
    /// <summary>
    /// Initialize the xml file and store data into museum class and a list of exhibit class
    /// </summary>
    private void InitializeXML()
    {
        if (PlayerPrefs.HasKey("xmlfile"))
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(PlayerPrefs.GetString("xmlfile"));
            // Get root element
            XmlElement root = xmlDoc.DocumentElement;

            // Get museum config
            XmlNode museumNode = root.SelectSingleNode("museum");
            string m_title = "", m_subtitle = "", m_author = "", m_version = "", m_description = "", m_link = "";
            for (int i = 0; i < museumNode.ChildNodes.Count; i++)
            {
                switch (museumNode.ChildNodes[i].Name)
                {
                    case "title":
                        m_title = museumNode.ChildNodes[i].InnerText;
                        break;
                    case "subtitle":
                        m_subtitle = museumNode.ChildNodes[i].InnerText;
                        break;
                    case "author":
                        m_author = museumNode.ChildNodes[i].InnerText;
                        break;
                    case "version":
                        m_version = museumNode.ChildNodes[i].InnerText;
                        break;
                    case "description":
                        m_description = museumNode.ChildNodes[i].InnerText;
                        break;
                    case "link":
                        m_link = museumNode.ChildNodes[i].InnerText;
                        PlayerPrefs.SetString("rootUrl", m_link);
                        break;
                }
                thisMuseum = new museum(m_title, m_subtitle, m_author, m_version, m_description, m_link);
            }

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
        else return;
    }


    public void OnClickButtonVisible()
    {
        isUIVisible = !isUIVisible;
        foreach(GameObject gameobj in hideUIObjects)
            gameobj.SetActive(isUIVisible);
        if (isUIVisible)
            btn_visible_image.sprite = spriteHide;
        else
            btn_visible_image.sprite = spriteShow;
    }


    public void OnClickButtonMenu()
    {
        isMenuTextVisible = !isMenuTextVisible;
        menuText.SetActive(isMenuTextVisible);
        if (isMenuTextVisible)
        {
            menuTextAnimator.SetBool("IsMenuTextVisible", true);
            btn_menu_image.sprite = spriteClose;
        }
        else
        {
            menuTextAnimator.SetBool("IsMenuTextVisible", false);
            btn_menu_image.sprite = spriteMenu;
        }
    }


    public void OnClickButtonExpand()
    {
        isBottombarExpand = !isBottombarExpand;
        if (isBottombarExpand)
        {
            bottombarAnimator.SetBool("IsBottombarExpand", true);
            btn_expand_image.sprite = spriteShrink;
        }
        else
        {
            bottombarAnimator.SetBool("IsBottombarExpand", false);
            btn_expand_image.sprite = spriteExpand;
        }
    }


    public void OnClickButtonClear()
    {
        QRCodeTracker.S.Reset();
    }


    public void OnClickButtonOk()
    {
        StopAllCoroutines();
        dialog_text.text = "";
        dialog.SetActive(false);
    }


    public void SetMuseumInfo()
    {
        title_text.text = thisMuseum.Title;
        subtitle_text.text = thisMuseum.Subtitle;
        description_text.text = thisMuseum.Description;
    }


    public void SetTitleAndDescriptionText(string fileName)
    {
        for (int i = 0; i < exhibitList.Count; i++)
        {
            if (exhibitList[i].FileName == fileName)
            {
                title_text.text = exhibitList[i].Title;
                subtitle_text.text = exhibitList[i].Author;
                description_text.text = exhibitList[i].Description;
                break;
            }
        }
    }

    public void SetDebugText(string s)
    {
        debug_text.text = s;
    }

    public void SetDialogText(string s)
    {
        if (!dialog.activeInHierarchy)
            dialog.SetActive(true);
        dialog_text.text = s;
    }


    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    #endregion
}
