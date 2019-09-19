using HoloToolkit.Unity.InputModule;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ExpertViewControl : MonoBehaviour, IInputClickHandler {

    public Action<Expert> OnExpertClick;

    private Image headImag;
    public Image HeadImage
    {
        get
        {
            if (headImag == null)
            {
                headImag = transform.Find("HeadImage").GetComponent<Image>();
            }
            return headImag;
        }
    }

    private Text expertNameText;
    public Text ExpertNameText
    {
        get
        {
            if (expertNameText == null)
            {
                expertNameText = transform.Find("NameText").GetComponent<Text>();
            }
            return expertNameText;
        }
    }

    private GameObject hightlightBg;
    public GameObject HightlightBg
    {
        get
        {
            if (hightlightBg == null)
            {
                hightlightBg = transform.Find("HighlightBg").gameObject;
            }
            return hightlightBg;
        }
    }

    private Expert theExpert;
    public Expert TheExpert
    {
        get
        {
            return theExpert;
        }

        set
        {
            if (theExpert != value)
            {
                theExpert = value;

                HeadImage.sprite = Resources.Load<Sprite>(theExpert.HeadImageUrl);
                ExpertNameText.text = theExpert.ExpertName;

            }
        }
    }

    private bool isSelected;
    public bool IsSelected
    {
        get
        {
            return isSelected;
        }

        set
        {
            if (isSelected != value)
            {
                isSelected = value;
                HightlightBg.SetActive(isSelected);
            }
        }
    }

    private bool isEnabled = true;
    public bool IsEnabled
    {
        get
        {
            return isEnabled;
        }

        set
        {
            if (isEnabled != value)
                isEnabled = value;
        }
    }


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnInputClicked(InputClickedEventData eventData)
    {
        if (isEnabled)
        {
            IsSelected = true;
            OnExpertClick?.Invoke(theExpert);
        }
        
    }
}
