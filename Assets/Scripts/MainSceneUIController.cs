using System.Collections;
using System.Collections.Generic;
using QRFoundation;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
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


    [Header("Focusbar")]
    [SerializeField]    private GameObject focusbar;


    [Header("SO")]
                        public InformationListSO informationListSO;
    #endregion

    #region Unity Methods
    // Start is called before the first frame update
    private void Awake()
    {
        S = this;    
    }

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
    }

    // Update is called once per frame
    private void Update()
    {
        /*
        if (activeModelIndex == 0)
            btn_clear.SetActive(false);
        else
            btn_clear.SetActive(true);
        */
    }
    #endregion


    #region Tools
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
        QRCodeTracker.S.DestroyActivedModel();
        activeModelIndex = 0;
    }


    public void SetTitleAndDescriptionText(string prefabName)
    {
        for (int i = 1; i < informationListSO.InformationList.Count; i++)
        {
            if (informationListSO.InformationList[i].title == prefabName)
            {
                activeModelIndex = i;
                break;
            }
        }
        title_text.text = informationListSO.InformationList[activeModelIndex].title.Contains("WT") ? informationListSO.InformationList[activeModelIndex].title.Substring(0, informationListSO.InformationList[activeModelIndex].title.Length - 2) : informationListSO.InformationList[activeModelIndex].title;
        description_text.text = informationListSO.InformationList[activeModelIndex].description;
        if (activeModelIndex == 0)
            subtitle_text.text = informationListSO.InformationList[activeModelIndex].subtitle;
        else
            subtitle_text.text = $"by {informationListSO.InformationList[activeModelIndex].author}";
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    #endregion
}
