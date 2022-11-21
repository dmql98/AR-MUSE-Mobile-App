using System.Collections;
using System.Collections.Generic;
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
    public InformationListSO informationListSO;

    public Animator hintbarAnimator;

    private bool isHintbarFolded;
    // Start is called before the first frame update
    void Start()
    {
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
        for (int i = 1; i < informationListSO.InformationList.Count; i++)
        {
            GameObject insObj = Instantiate(lineOfList, socket);
            insObj.transform.Find("Text_Description").GetComponent<TextMeshProUGUI>().text =
                $"<b>{informationListSO.InformationList[i].title}</b>\nby {informationListSO.InformationList[i].author}";
            insObj.GetComponent<Button>().onClick.AddListener(() => OnClickLineOfCollectionList());
        }
    }

    private void OnClickLineOfCollectionList()
    {
        isHintbarFolded = false;
        GameObject clickedLine = EventSystem.current.currentSelectedGameObject;
        int indexInList = clickedLine.transform.GetSiblingIndex() + 1;
        text_title.text = $"<b>{informationListSO.InformationList[indexInList].title}</b> by {informationListSO.InformationList[indexInList].author}";
        text_hint.text = informationListSO.InformationList[indexInList].description;
    }

    public void OnClickButtonClose()
    {
        isHintbarFolded = !isHintbarFolded;
    }
}
