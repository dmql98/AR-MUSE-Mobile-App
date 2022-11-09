using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;


public class MainSceneUIController : MonoBehaviour
{
    #region References
    [Header("Button Visible")]
    [SerializeField]    private Image btn_visible_image;
    [SerializeField]    private Sprite spriteHide;
    [SerializeField]    private Sprite spriteShow;
                        private bool isUIVisible;
                        private List<GameObject> hideUIObjects;


    [Header("Menu")]
    [SerializeField]    private GameObject menu;
                        private GameObject menuText;
                        private Animator menuTextAnimator;
                        private bool isMenuTextVisible;


    [Header("Bottombar")]
    [SerializeField]    private GameObject bottombar;



    [Header("Focusbar")]
    [SerializeField]    private GameObject focusbar;
    #endregion


    // Start is called before the first frame update
    void Start()
    {
        isUIVisible = true;
        hideUIObjects = new List<GameObject>();
        hideUIObjects.Add(menu);
        hideUIObjects.Add(bottombar);
        hideUIObjects.Add(focusbar);

        isMenuTextVisible = false;
        menuText = GameObject.Find("MenuText");
        menuTextAnimator = menuText.GetComponent<Animator>();
        menuText.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

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
            menuTextAnimator.SetBool("IsMenuTextVisible", true);
        else
            menuTextAnimator.SetBool("IsMenuTextVisible", false);   
    }



    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    #endregion
}
