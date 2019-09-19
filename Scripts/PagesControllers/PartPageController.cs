using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class PartPageController : MonoBehaviour
{
    //private List<MachinePart> machineParts;
    public MachineAssembly TheAss;
    private List<PartTypeViewControl> partViewControls;
    private Transform modelContainer;
    private List<Transform> FindTransform;

    private GameObject modelInstance;

    private GameObject ReplaceUI;
    private GameObject LubricateUI;

    private MachinePart currentPart;

    private List<PartButtonControl> partButtons;
    private List<SubPartViewControl> lublicatePartView;
    private VideoPlayer videoPlayer;
    private GameObject videoPanel;

    // Use this for initialization
    void Start () {

        FindTransform = new List<Transform>();
        videoPanel = transform.Find("VideoPanel").gameObject;
        videoPlayer = transform.Find("VideoPanel/VideoPlayer").GetComponent<VideoPlayer>();
        videoPlayer.SetTargetAudioSource(0, transform.Find("VideoPanel/AudioSource").GetComponent<AudioSource>());
    }

    private void OnEnable()
    {
        if (ReplaceUI == null)
            ReplaceUI = transform.Find("CheckPhotoPanel/Replace").gameObject;

        if (LubricateUI == null)
            LubricateUI = transform.Find("CheckPhotoPanel/Lubricate").gameObject;

        if (partButtons == null)
        {
            partButtons = transform.Find("CheckPhotoPanel/Lubricate").GetComponentsInChildren<PartButtonControl>().ToList();
            foreach(var button in partButtons)
            {
                button.OnPartClick += OnPartClick;
            }
        }
            

        if (lublicatePartView == null)
        {
            lublicatePartView = transform.Find("CheckPhotoPanel/Lubricate").GetComponentsInChildren<SubPartViewControl>().ToList();

            foreach (var ctrl in lublicatePartView)
            {
                ctrl.OnViewVideoClick += OnSubPartVideo;
            }
        }
            

        if (partViewControls == null)
        {
            partViewControls = GetComponentsInChildren<PartTypeViewControl>().ToList();
            foreach (var ctrl in partViewControls)
            {
                ctrl.OnTypeHover += OnTypeHover;
                ctrl.OnTypeLeave += OnTypeLeave;
                ctrl.OnTypeClick += OnTypeClick;
            }
        }
            

        if (modelContainer == null)
            modelContainer = transform.Find("ModelContainer");

        if (TheAss != null)
        {
            LoadAssModel();
            SetPartTypeControl();
        }

        OnReplaceClick();
        
    }

    private void OnDisable()
    {
        if (modelInstance != null)
            Destroy(modelInstance);
    }

    private void OnTypeHover(PartType theType)
    {
        //highlight models
        var objsOfType = FindForModel(theType.TypeId.ToString(), modelContainer);
        Debug.Log("found objects: " + objsOfType.Count);


        foreach (var par in objsOfType)
        {
            var materials = par.GetComponentInChildren<Renderer>().materials;

            foreach (var m in materials)
            {
                m.SetColor("_EmissionColor", new Color(1, 0, 0));
            }
        }
        

    }

    private void OnTypeLeave(PartType theType)
    {
        //highlight models

        var objsOfType = FindForModel(theType.TypeId.ToString(), modelContainer);
        Debug.Log("found objects: " + objsOfType.Count);


        foreach (var par in objsOfType)
        {
            var materials = par.GetComponentInChildren<Renderer>().materials;

            foreach (var m in materials)
            {
                m.SetColor("_EmissionColor", new Color(0, 0, 0));
            }
        }

    }

    private void OnTypeClick(PartType theType)
    {
        GameObject typeDetailPage = transform.parent.Find("ReplaceDetailPage").gameObject;

        ReplaceDetailPageController ctrl = typeDetailPage.GetComponent<ReplaceDetailPageController>();
        ctrl.TheAss = TheAss;
        ctrl.CurrentType = theType.TypeId;

        NavigationService.Instance.NavigateTo(typeDetailPage);
    }

    private List<Transform> FindForModel(string name,Transform currentTransform)
    {
        List<Transform> found = new List<Transform>();
        for (int i = 0; i < currentTransform.childCount; i ++)
        {
            var currentchild = currentTransform.GetChild(i);
            var childFound = FindForModel(name, currentchild);

            if (currentchild.name == name)
            {
                found.Add(currentchild);
            }

            found.AddRange(childFound);
        }

        return found;
      
    }

    // Update is called once per frame
    void Update () {
		
	}

    private void LoadAssModel()
    {
        
        modelInstance = Instantiate(Resources.Load<GameObject>(TheAss.AssemblyHilightUrl));
        modelInstance.transform.SetParent(modelContainer);
        modelInstance.transform.localPosition = Vector3.zero;
        modelInstance.transform.localScale = Vector3.one;
    }

    private void SetPartTypeControl()
    {

        List<PartType> partTypes = new List<PartType>();

        foreach(var par in TheAss.PartList)
        {
            foreach (var sub in par.SubParts)
            {
                if (!partTypes.Select((p) => p.TypeId).Contains(sub.PartType))
                {
                    partTypes.Add(DataManager.PartTypes[sub.PartType]);
                }
            }
        }

        for (int i = 0; i < partViewControls.Count; i++)
        {
            PartTypeViewControl control = partViewControls[i];

            if (i < partTypes.Count)
            {
                control.TheType = partTypes[i];
                control.gameObject.SetActive(true);
            }                
            else
            {
                control.gameObject.SetActive(false);
            }
        }
    }

    public void OnBack()
    {
        if (modelInstance != null)
            Destroy(modelInstance);

        videoPanel.SetActive(false);
        NavigationService.Instance.Back();
    }

    public void OnReplaceClick()
    {
        GameObject button1 = transform.Find("CheckPhotoPanel/Change").gameObject;
        button1.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/NewUI/ScheduledChange/change");
        GameObject button2 = transform.Find("CheckPhotoPanel/Runhuan").gameObject;
        button2.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/NewUI/ScheduledChange/lubricate_clicked");
        ReplaceUI.SetActive(true);
        LubricateUI.SetActive(false);
    }

    public void OnLubricateClick()
    {
        GameObject button1 = transform.Find("CheckPhotoPanel/Change").gameObject;
        button1.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/NewUI/ScheduledChange/change_clicked");
        GameObject button2 = transform.Find("CheckPhotoPanel/Runhuan").gameObject;
        button2.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/NewUI/ScheduledChange/lubricate");
        ReplaceUI.SetActive(false);
        LubricateUI.SetActive(true);

        

        for (int i = 0; i < partButtons.Count; i ++)
        {
            PartButtonControl ctrl = partButtons[i];

            if (i < TheAss.PartList.Count)
            {
                ctrl.ThePart = TheAss.PartList[i];
                ctrl.gameObject.SetActive(true);
            }

        }

        for (int i = TheAss.PartList.Count; i < partButtons.Count; i++)
        {
            PartButtonControl ctrl = partButtons[i];
            ctrl.gameObject.SetActive(false);
        }

      

        if (TheAss.PartList.Count > 0)
        {
            OnPartClick(TheAss.PartList[0]);
        }

    }

    private void UpdateLublicateContent(MachinePart part)
    {
        List<SubPart> lublicatePart = part.SubParts.Where((p) => { return p.CycleType == CycleType.Luburicate; }).ToList();

        for (int i = 0; i < lublicatePartView.Count; i++)
        {
            SubPartViewControl ctrl = lublicatePartView[i];

            if (i < lublicatePart.Count)
            {
                ctrl.ThePart = lublicatePart[i];
                ctrl.gameObject.SetActive(true);
            }
        }

        for (int i = lublicatePart.Count; i < lublicatePartView.Count; i++)
        {
            SubPartViewControl ctrl = lublicatePartView[i];
            ctrl.gameObject.SetActive(false);
        }
    }

    private void OnPartClick(MachinePart part)
    {
        UpdateLublicateContent(part);
    }

    private void OnSubPartVideo(SubPart sub)
    {
        //TODO: Show the sub video
        if (!string.IsNullOrEmpty(sub.VideoUrl))
        {
            VideoClip clip = Resources.Load<VideoClip>(sub.VideoUrl);
            videoPlayer.clip = clip;
            videoPlayer.Play();
            videoPanel.SetActive(true);

            if (modelInstance != null)
                modelInstance.SetActive(false);
        }        
    }

    public void CloseVideoPanel()
    {
        videoPlayer.Stop();
        videoPanel.SetActive(false);

        if (modelInstance != null)
            modelInstance.SetActive(true);
    }

}
