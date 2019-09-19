using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;

public class HoverHighlight : MonoBehaviour, IFocusable
{

    Material[] _materials;
    // Use this for initialization
    void Start()
    {
        _materials = GetComponent<Renderer>().materials;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnFocusEnter()
    {
        foreach (var m in _materials)
        {
            m.SetColor("_EmissionColor", new Color(1, 0, 0));
        }
    }
    public void OnFocusExit()
    {
        foreach (var m in _materials)
        {
            m.SetColor("_EmissionColor", new Color(0, 0, 0));
        }
    }
}
