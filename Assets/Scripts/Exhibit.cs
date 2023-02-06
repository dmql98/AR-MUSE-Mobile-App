using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exhibit 
{
    public class exhibit
    {
        string id;
        string isactive;
        string title;
        string subtitle;
        string author;
        string description;
        string fileName;

        public string ID { get { return id; } }
        public string IsActive { get { return isactive; } }
        public string Title { get { return title; } }
        public string Subtitle { get { return subtitle; } }
        public string Author { get { return author; } }
        public string Description { get { return description; } }
        public string FileName { get { return fileName; } }
        public exhibit(string identity, string isActive, string exhibitTitle = null, string exhibitSubtitle = null, string exhibitAuthor = null, string exhibitDescription = null, string exhibitFileName = null)
        {
            id = identity;
            isactive = isActive;
            title = exhibitTitle;
            subtitle = exhibitSubtitle;
            author = exhibitAuthor;
            description = exhibitDescription;
            fileName = exhibitFileName;
        }
    }

}
