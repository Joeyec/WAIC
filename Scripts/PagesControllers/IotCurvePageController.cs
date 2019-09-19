using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class IotCurvePageController : MonoBehaviour {

    public List<IotDevice> iotDevices;
    private List<CurveButtonControl> curveButtons;
    private List<CurveViewControl> curveViews;
    private Image backgroundImage;
  

	// Use this for initialization
	void Start () {
        
	}

    private void OnEnable()
    {
        if (curveButtons == null)
        {
            curveButtons = transform.GetComponentsInChildren<CurveButtonControl>().ToList();
        }

        if (curveViews == null)
        {
            curveViews = transform.GetComponentsInChildren<CurveViewControl>().ToList();
        }

        List<string> curveTypes = iotDevices[0].Curves.Select((p) => p.CurveName).ToList();

        for (int i = 0; i < curveButtons.Count; i ++)
        {
            CurveButtonControl button = curveButtons[i];

            button.OnCurveTypeClick += OnCurveTypeClick;


            if (i < curveTypes.Count)
            {
                button.CurveType = curveTypes[i];
                button.gameObject.SetActive(true);
            }
                
        }

        for (int i = curveTypes.Count; i < curveButtons.Count; i++ )
        {
            CurveButtonControl button = curveButtons[i];
            button.gameObject.SetActive(false);
        }

        OnCurveTypeClick(curveTypes[0]);

        //load the background picture

        if (backgroundImage == null)
        {
            backgroundImage = transform.Find("Title").GetComponent<Image>();
        }

        if (iotDevices.Count > 0)
        {
            IotDeviceType dt = DataManager.IotDeviceTypes[iotDevices[0].DeviceType];
            backgroundImage.sprite = Resources.Load<Sprite>(dt.TypeTitleUrl);
        }


    }

    private void OnCurveTypeClick(string curveType)
    {
        List<DeviceCurve> curves = new List<DeviceCurve>();

        foreach (var d in iotDevices)
        {
            DeviceCurve c = d.Curves.Find((p) => { return p.CurveName == curveType; } );
            curves.Add(c);
        }

        for (int i = 0; i < curveViews.Count; i++)
        {
            CurveViewControl ctrl = curveViews[i];

            if (i < curves.Count)
            {
                ctrl.DeviceCurve = curves[i];
                ctrl.DeviceName = iotDevices[i].DeviceName;  //dt.TypeName + (i + 1);
                ctrl.gameObject.SetActive(true);
            }

        }

        for (int i = curves.Count; i < curveViews.Count; i++)
        {
            CurveViewControl ctrl = curveViews[i];
            ctrl.gameObject.SetActive(false);
        }

        foreach (var p in curveButtons)
        {
            if (p.CurveType != curveType)
            {
                p.IsSelected = false;
            }
            else
            {
                p.IsSelected = true;
            }

        }

    }

    // Update is called once per frame
    void Update () {


		
	}

    public void OnBack()
    {
        NavigationService.Instance.Back();
    }
}
