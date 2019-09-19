using HoloToolkit.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavigationService : Singleton<NavigationService> {



	// Use this for initialization
	void Start () {

        pageStack = new Stack<GameObject>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    Stack<GameObject> pageStack;

    public void NavigateTo(GameObject page)
    {
        if (pageStack.Count > 0)
        {
            GameObject current = pageStack.Peek();
            current.SetActive(false);
        }

        page.SetActive(true);
        pageStack.Push(page);

    }

    public void Back()
    {
        if (pageStack.Count > 1)
        {
            GameObject current = pageStack.Pop();
            current.SetActive(false);

            current = pageStack.Peek();

            if (current != null)
            {
                current.SetActive(true);
            }
        }
       
    }

}
