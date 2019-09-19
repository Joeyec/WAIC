using HoloToolkit.Unity.InputModule;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class PartButtonControl : MonoBehaviour, IInputClickHandler {

    public Action<MachinePart> OnPartClick;

    private Text PartNameText;

    private Image bgImage;
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

                if (bgImage == null)
                    bgImage = GetComponent<Image>();

                if (isSelected)
                {
                    bgImage.sprite = Resources.Load<Sprite>("Sprites/DailyMaintanence/select");
                }
                else
                {
                    bgImage.sprite = Resources.Load<Sprite>("Sprites/DailyMaintanence/noselect");
                }
            }
        }
    }

    private MachinePart thePart;
    public MachinePart ThePart
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

                if (PartNameText == null)
                {
                    PartNameText = transform.Find("Text").GetComponent<Text>();
                }

                PartNameText.text = thePart.PartName;
            }

        }
    }

    public void OnInputClicked(InputClickedEventData eventData)
    {
        if (OnPartClick != null)
        {
            OnPartClick.Invoke(thePart);
        }
            
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
