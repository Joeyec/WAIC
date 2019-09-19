using HoloToolkit.Unity;
using HoloToolkit.Unity.Collections;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UIController : Singleton<UIController> {

    public GameObject LoginBase;

    public Material pinMat;
    public Material unPinMat;

    private List<Button> pinButtons;
    private Tagalong tl;

    public GameObject[] Pages;

    public int _currentPage;
    public int CurrentPage
    {

        get { return _currentPage; }

        set {

            _currentPage = value;

            for ( int i = 0; i < Pages.Length; i ++) {

                GameObject p = Pages[i];
                p.SetActive(i == _currentPage);
            }
        }

    }

	// Use this for initialization
	void Start () {
        //LoginBase = GameObject.Find("LoginBase");
        //ContactsBase = GameObject.Find("ContactsBase");

        pinButtons = GetComponentsInChildren<Button>(true).Where((b) => { return b.name == "PinButton"; }).ToList() ;
        tl = GetComponentInChildren<Tagalong>();

    }
	
	// Update is called once per frame
	void Update () {

        foreach (Button b in pinButtons)
        {
            if (b.gameObject.activeInHierarchy)
            {
                Renderer renderer = b.GetComponent<Renderer>();
                renderer.material = tl.enabled ? pinMat : unPinMat;

            }

        }
	}

    public void HideLogin()
    {
        LoginBase.SetActive(false);
    }

    public void DisableTagalong() {

        tl.enabled = !tl.enabled;

    }

    public void AddToCollection(GameObject obj)
    {
        obj.transform.SetParent(transform);
        obj.transform.localScale = Vector3.one;
        CollectionNode node = new CollectionNode();
        node.transform = obj.transform;

        ObjectCollection oc = GetComponent<ObjectCollection>();
        oc.NodeList.Add(node);
        oc.UpdateCollection();
    }

    public void RemoteFromCollection(GameObject obj)
    {
        ObjectCollection oc = GetComponent<ObjectCollection>();

        CollectionNode node = oc.NodeList.Where((n) => { return n.transform == obj.transform; }).FirstOrDefault();
        if (node != null)
        {
            oc.NodeList.Remove(node);
            oc.UpdateCollection();
        }

    }

}
