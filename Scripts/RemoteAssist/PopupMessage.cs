using HoloToolkit.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupMessage : Singleton<PopupMessage> {

    public string MessageText;

    TextMesh Message;

	// Use this for initialization
	void Start () {

        Message = transform.Find("Message").GetComponent<TextMesh>(); 
    }
	
	// Update is called once per frame
	void Update () {

        Message.text = MessageText;
		
	}

    public void ShowAt(Vector3 position)
    {
        transform.position = position;
        transform.localScale = Vector3.one;
    }

    public void Hide()
    {
        transform.localScale = Vector3.zero;
    }
}
