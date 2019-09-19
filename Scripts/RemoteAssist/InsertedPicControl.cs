using HoloToolkit.Unity;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class InsertedPicControl : MonoBehaviour {

    public Material pinMat;
    public Material unPinMat;

    // Use this for initialization
    void Start () {

    }
	
	// Update is called once per frame
	void Update () {


    }

    public void Close()
    {
        UIController.Instance.RemoteFromCollection(gameObject);
        Destroy(gameObject);
    }

}
