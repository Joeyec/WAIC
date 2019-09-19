using HoloToolkit.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootController : Singleton<RootController> {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void EnableTagalong(bool eanbled)
    {
        GetComponent<Tagalong>().enabled = eanbled;
        GetComponent<Billboard>().enabled = enabled;
    }
}
