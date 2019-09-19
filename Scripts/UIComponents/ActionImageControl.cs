using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ActionImageControl : MonoBehaviour, IPointerClickHandler
{

    private GameObject checkedSign;
    private Image image;
    public Action<DailyMaintAction> OnImageClick;

    private DailyMaintAction theAction;

    public DailyMaintAction TheAction
    {
        get
        {
            return theAction;
        }

        set
        {
            if (theAction != value)
            {
                theAction = value;                
                LoadImage(theAction.PictureUrl);
            }

        }
    }

    //private bool isCompleted;
    //public bool IsCompleted
    //{
    //    get
    //    {
    //        return isCompleted;
    //    }

    //    set
    //    {
    //        if (isCompleted != value)
    //        {
    //            isCompleted = value;
    //            checkedSign.SetActive(isCompleted);
    //        }
    //    }
    //}

    //private string imageUrl;
    //public string ImageUrl
    //{
    //    get
    //    {
    //        return imageUrl;
    //    }

    //    set
    //    {
    //        if (imageUrl != value)
    //        {
    //            imageUrl = value;
    //            LoadImage();
    //        }

    //    }
    //}

	// Use this for initialization
	void Start () {        

    }

    void OnEnable()
    {
        
    }

    void Awake()
    {
        checkedSign = transform.Find("Completed").gameObject;
        image = GetComponent<Image>();
        if (TheAction != null && !string.IsNullOrEmpty(TheAction.PictureUrl))
        {
            LoadImage(TheAction.PictureUrl);
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (checkedSign != null && TheAction != null)
            checkedSign.SetActive(TheAction.Completed);		
	}

    private void LoadImage(string imageUrl)
    {
        Debug.Log("Loading Image: " + imageUrl);
        if (image != null)
        {
            image.sprite = Resources.Load<Sprite>(imageUrl);
            Resources.UnloadUnusedAssets();
        }            
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (OnImageClick != null)
            OnImageClick.Invoke(TheAction);
    }
}
