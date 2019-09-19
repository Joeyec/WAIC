using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowIndicator : MonoBehaviour {

    private static Stack<ArrowIndicator> s_indicators = new Stack<ArrowIndicator>();  

	// Use this for initialization
	void Start () {

        lock (s_indicators)
        {
            s_indicators.Push(this);
        }
		
	}

    // Update is called once per frame
    void Update() {

    }

    public static void PopIndicator()
    {
        try
        {
            ArrowIndicator indicator = null;
            lock (s_indicators)
            {
                indicator = s_indicators.Pop();
            }
            
            if (indicator != null)
                Destroy(indicator.gameObject);
        }
        catch (InvalidOperationException ex)
        {
            Debug.Log("the stack is empty");
        }
    }
}
