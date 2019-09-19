using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModulePageController : MonoBehaviour {

    [HideInInspector]
    public Module MainModule;

    private Transform MaintPageTransform;

    //private MaintPageController maintPageController;

	// Use this for initialization
	void Start () {

        NavigationService.Instance.NavigateTo(gameObject);
        MaintPageTransform = transform.parent.Find("MaintPage");

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnMechanicalMaint()
    {
        //MaintPageTransform.gameObject.SetActive(true);
        NavigationService.Instance.NavigateTo(MaintPageTransform.gameObject);
        MaintPageTransform.GetComponent<MaintPageController>().CurrentMaintenanceType = MaintenanceType.Mechanical;

    }

    public void OnElectricalMaint()
    {
        //MaintPageTransform.gameObject.SetActive(true);
        NavigationService.Instance.NavigateTo(MaintPageTransform.gameObject);
        MaintPageTransform.GetComponent<MaintPageController>().CurrentMaintenanceType = MaintenanceType.Electrical;
    }
}
