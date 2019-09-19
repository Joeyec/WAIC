using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BigImagePageController : MonoBehaviour {

    [HideInInspector]
    public DailyMaintAction TheAction;

    private Image bigImage;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnEnable()
    {
        bigImage = transform.Find("BigImage").GetComponent<Image>();
        if (TheAction != null && !string.IsNullOrEmpty(TheAction.BigPictureUrl))
        {
            bigImage.sprite = Resources.Load<Sprite>(TheAction.BigPictureUrl);
            Resources.UnloadUnusedAssets();
        }
    }

    public void OnConfirm()
    {
        TheAction.Completed = true;
        NavigationService.Instance.Back();

    }

    public void OnContinue()
    {
        NavigationService.Instance.Back();
    }
}
