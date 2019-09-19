using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using HoloToolkit.Unity.InputModule;

public class PartTypeViewControl : MonoBehaviour, IFocusable,IInputClickHandler
{

    public Action<PartType> OnTypeHover;
    public Action<PartType> OnTypeLeave;
    public Action<PartType> OnTypeClick;


    private Text TypeName;
    private Image PartImage;

    private PartType theType;
    public PartType TheType
    {
        get
        {
            return theType;
        }

        set
        {
            if (theType != value)
            {
                theType = value;

                if (TypeName == null)
                    TypeName = transform.Find("PartPicture/Text").GetComponent<Text>();

                if (PartImage == null)
                    PartImage = GetComponent<Image>();

                if (TypeName != null && PartImage != null)
                {
                    
                    TypeName.text = theType.TypeName;
                    LoadImage(theType.TypePictureUrl);
                }

            }
        }
    }

    private void Awake()
    {
        
    }
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void LoadImage(string url)
    {
        if (url != null && PartImage != null)
            PartImage.sprite = Resources.Load<Sprite>(url);
    }

    public void OnFocusEnter()
    {
        if (OnTypeHover != null)
            OnTypeHover.Invoke(theType);
    }

    public void OnFocusExit()
    {
        if (OnTypeLeave != null)
            OnTypeLeave.Invoke(theType);
    }

    public void OnInputClicked(InputClickedEventData eventData)
    {        
        if (OnTypeClick != null)
        {
            OnTypeClick.Invoke(theType);
        }
    }
}
