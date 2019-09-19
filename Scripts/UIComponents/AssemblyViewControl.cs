using HoloToolkit.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AssemblyViewControl : MonoBehaviour {
    

    //view
    private Transform container;
    private Text assemblyNameText;
    private Text completedText;

    //data

    private MachineAssembly theAss;
    public MachineAssembly TheAss
    {
        get
        {
            return theAss;
        }

        set
        {
            if (theAss != value)
            {
                theAss = value;
                if (assemblyNameText != null && container != null)
                {
                    assemblyNameText.text = theAss.AssemblyName;
                    LoadModel(theAss.AssemblyModelUrl);
                }
                    
            }
        }
    }

    public Action<MachineAssembly> OnModelClick;


    // Use this for initialization
    void Start () {

        
		
	}

    private void Awake()
    {
        container = transform.Find("ModelContainer");
        assemblyNameText = transform.Find("UI/AssemblyName").GetComponent<Text>();
        completedText = transform.Find("UI/CompleteStatus").GetComponent<Text>();
    }

    // Update is called once per frame
    void Update () {
		
	}

    private void LoadModel(string url)
    {
        GameObject modelInstance = Instantiate(Resources.Load<GameObject>(url));

        //need to add box collider to the imported model
        float maxX = float.NegativeInfinity;
        float maxY = float.NegativeInfinity;
        float maxZ = float.NegativeInfinity;
        float minX = float.PositiveInfinity;
        float minY = float.PositiveInfinity;
        float minZ = float.PositiveInfinity;

        Renderer[] renders = modelInstance.GetComponentsInChildren<Renderer>();
        foreach (Renderer child in renders)
        {
            minX = Math.Min(minX, child.bounds.min.x);
            minY = Math.Min(minY, child.bounds.min.y);
            minZ = Math.Min(minZ, child.bounds.min.z);

            maxX = Math.Max(maxX, child.bounds.max.x);
            maxY = Math.Max(maxY, child.bounds.max.y);
            maxZ = Math.Max(maxZ, child.bounds.max.z);
        }

        Bounds bounds = new Bounds();
        bounds.SetMinMax(new Vector3(minX, minY, minZ), new Vector3(maxX, maxY, maxZ));

        BoxCollider boxCollider = modelInstance.EnsureComponent<BoxCollider>();
        boxCollider.center = new Vector3(0, 0, 0);
        boxCollider.size = bounds.size;

        modelInstance.transform.SetParent(container);
        modelInstance.transform.localScale = new Vector3(1.5f,1.5f,1.5f);
        //modelInstance.transform.localScale = Vector3.one;
        modelInstance.transform.localPosition = Vector3.zero;

        var button = boxCollider.EnsureComponent<Button>();
        button.onClick.AddListener(() => {

            if (OnModelClick != null)
                OnModelClick.Invoke(theAss);

        });
    }
}
