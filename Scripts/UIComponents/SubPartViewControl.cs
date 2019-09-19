using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class SubPartViewControl : MonoBehaviour {

    public Action<SubPart> OnViewVideoClick;

    private Text NameText;
    private Text CountText;
    private Text CycleText;

    private SubPart thePart;
    public SubPart ThePart
    {
        get
        {
            return thePart;
        }

        set
        {
            if (thePart != value)
            {
                thePart = value;

                if (NameText == null)
                {
                    NameText = transform.Find("SubPartName").GetComponent<Text>(); 
                }

                NameText.text = thePart.SubPartName;

                if (CycleText == null)
                {
                    CycleText = transform.Find("SubPartCycle")?.GetComponent<Text>();
                }

                if (CycleText != null)
                    CycleText.text = thePart.CycleInMonth.ToString();

            }
        }
    }

    private int countOfPart;
    public int CountOfPart
    {
        get
        {
            return countOfPart;
        }

        set
        {
            if (countOfPart != value)
            {
                countOfPart = value;

                if (CountText == null)
                {
                    CountText = transform.Find("SubPartNumber").GetComponent<Text>();
                }

                if (CountText != null)
                    CountText.text = countOfPart.ToString();
            }
        }
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnViewVideo()
    {
        if (OnViewVideoClick != null)
            OnViewVideoClick.Invoke(thePart);
    }
}
