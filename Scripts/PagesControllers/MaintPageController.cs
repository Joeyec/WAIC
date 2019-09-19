using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MaintPageController : MonoBehaviour {

    private MaintenanceType currentMaintenanceType;

    [HideInInspector]
    public MaintenanceType CurrentMaintenanceType
    {
        get
        {
            return currentMaintenanceType;
        }

        set
        {
            if (currentMaintenanceType != value)
            {
                currentMaintenanceType = value;

                switch (CurrentMaintenanceType)
                {
                    case MaintenanceType.Mechanical:
                        {
                            button2Text.text = "定 期 保 养";
                        }
                        break;
                    case MaintenanceType.Electrical:
                        {
                            button2Text.text = "在 线 分 析";
                        }
                        break;
                }
            }
        }
    }

    private Text button2Text;    

	// Use this for initialization
	void Start () {

    }

    private void OnEnable()
    {
        button2Text = transform.Find("Button2/Text").GetComponent<Text>();
    }

    // Update is called once per frame
    void Update () {
        
	}

    public void OnBack()
    {
        NavigationService.Instance.Back();
    }

    public void OnButton1()
    {
        //always go to daily maintenance
        GameObject DailyMaintPage = transform.parent.Find("DailyMaintPage").gameObject;
        DailyMaintPage.GetComponent<DailyMaintPageController>().CurrentMaintType = currentMaintenanceType;


        NavigationService.Instance.NavigateTo(DailyMaintPage);
    }

    public void OnButton2()
    {
        switch (CurrentMaintenanceType)
        {
            case MaintenanceType.Electrical:
                {
                    //go to online analysis
                    GameObject OnlineAnaPage = transform.parent.Find("OnlineAnalysisPage").gameObject;
                    NavigationService.Instance.NavigateTo(OnlineAnaPage);

                }
                break;

            case MaintenanceType.Mechanical:
                {
                    //go to fixed interval maintenance
                    GameObject ScheduledMaintPage = transform.parent.Find("ScheduledMaintPage").gameObject;
                    NavigationService.Instance.NavigateTo(ScheduledMaintPage);
                }
                break;
        }

    }
}
