using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PannelControl : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void pannelButton(GameObject pannel)
    {
        if (pannel.activeSelf)
        {
            pannel.SetActive(false);
        }
        else
        {
            pannel.SetActive(true);
        }
    }
}
