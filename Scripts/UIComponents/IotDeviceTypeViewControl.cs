using HoloToolkit.Unity.InputModule;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class IotDeviceTypeViewControl : MonoBehaviour, IInputClickHandler {

    private Text TypeNameText;
    private Image TypeImage;
   

    public Action<IotDeviceType> OnIotTypeClick;

    private IotDeviceType theType;
    public IotDeviceType TheType
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

                //TODO: update ui
                if (TypeNameText == null)
                {
                    TypeNameText = transform.Find("TypeName").GetComponent<Text>();
                }
                

                TypeNameText.text = theType.TypeName;

                if (TypeImage == null)
                {
                    TypeImage = transform.Find("TypeImage").GetComponent<Image>();
                }

                TypeImage.sprite = Resources.Load<Sprite>(theType.PictureUrl);

            
            }
        }
    }

    public void OnInputClicked(InputClickedEventData eventData)
    {
        OnIotTypeClick?.Invoke(theType);
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
