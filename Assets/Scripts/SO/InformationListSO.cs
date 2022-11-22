using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InformationList", menuName = "MUSE/Museum Information", order = 1)]
public class InformationListSO : ScriptableObject
{
    // Index 0 is reversed for the museum info
    public List<Information> InformationList;

    [System.Serializable]
    public class Information
    {
                        public string title;
                        public string subtitle;
                        public string id;
                        public string author;
        [Multiline]     public string description;
    }


}
