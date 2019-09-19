using HoloToolkit.Unity.InputModule;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class CurveButtonControl : MonoBehaviour, IInputClickHandler {

    public Action<string> OnCurveTypeClick;

    private string curveType;
    public string CurveType
    {
        get
        {
            return curveType;
        }

        set
        {
            if (curveType != value)
            {
                curveType = value;

                if (CurveTypeText == null)
                {
                    CurveTypeText = transform.Find("Text").GetComponent<Text>();
                }

                CurveTypeText.text = curveType;
            }
        }
    }

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
                    bgImage.sprite = Resources.Load<Sprite>("Sprites/SubOnlineAna/select");
                }
                else
                {
                    bgImage.sprite = Resources.Load<Sprite>("Sprites/SubOnlineAna/noselect");
                }
            }
        }
    }
    private Text CurveTypeText;

    // Use this for initialization
    void Start () {

		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnInputClicked(InputClickedEventData eventData)
    {
        OnCurveTypeClick?.Invoke(curveType);
    }
}
