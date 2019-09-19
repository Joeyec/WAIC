using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Video;

public class ReplaceDetailPageController : MonoBehaviour {

    public MachineAssembly TheAss;
    public PartTypeEnum CurrentType;

    private List<PartButtonControl> partButtons;
    private List<SubPartViewControl> replacePartView;
    private VideoPlayer videoPlayer;
    private GameObject videoPanel;

    private Transform modelContainer;
    private GameObject modelInstance;

    // Use this for initialization
    void Start () {

        videoPanel = transform.Find("VideoPanel").gameObject;
        videoPlayer = transform.Find("VideoPanel/VideoPlayer").GetComponent<VideoPlayer>();
        videoPlayer.SetTargetAudioSource(0, transform.Find("VideoPanel/AudioSource").GetComponent<AudioSource>());
        modelContainer = transform.Find("ModelContainer");

    }

    private void OnEnable()
    {
        if (modelContainer == null)
            modelContainer = transform.Find("ModelContainer");

        if (partButtons == null)
        {
            partButtons = transform.Find("Bg").GetComponentsInChildren<PartButtonControl>().ToList();
            foreach (var button in partButtons)
            {
                button.OnPartClick += OnPartClick;
            }
        }

        if (replacePartView == null)
        {
            replacePartView = transform.Find("Bg").GetComponentsInChildren<SubPartViewControl>().ToList();

            foreach (var ctrl in replacePartView)
            {
                ctrl.OnViewVideoClick += OnSubPartVideo;
            }
        }

        List<MachinePart> selectedParts = TheAss.PartList.Where((p) => {

            return p.SubParts.Count((s) => { return s.PartType == CurrentType; }) > 0;

        }).ToList();

        for (int i = 0; i < partButtons.Count; i++)
        {
            PartButtonControl ctrl = partButtons[i];

            if (i < selectedParts.Count)
            {
                ctrl.ThePart = selectedParts[i];
                ctrl.gameObject.SetActive(true);
            }

        }

        for (int i = selectedParts.Count; i < partButtons.Count; i++)
        {
            PartButtonControl ctrl = partButtons[i];
            ctrl.gameObject.SetActive(false);
        }

        if (selectedParts.Count > 0)
        {
            OnPartClick(selectedParts[0]);
        }
    }

    private void OnPartClick(MachinePart part)
    {
        if (modelInstance != null)
            Destroy(modelInstance);

        string modelUrl = part.ModelUrls[CurrentType];
        modelInstance = Instantiate(Resources.Load<GameObject>(modelUrl));
        modelInstance.transform.SetParent(modelContainer);
        modelInstance.transform.localPosition = Vector3.zero;
        modelInstance.transform.localScale = Vector3.one;

        UpdateReplaceContent(part);

        foreach (var p in partButtons)
        {
            if (p.ThePart != part)
            {
                p.IsSelected = false;
            }
            else
            {
                p.IsSelected = true;
            }
            
        }

        
    }

    private void UpdateReplaceContent(MachinePart part)
    {
        List<SubPart> replacePart = part.SubParts.Where((p) => {
            return p.CycleType == CycleType.Replace 
            && p.PartType == CurrentType;
        }).ToList();

        Dictionary<string, int> unique = new Dictionary<string, int>();

        foreach(var p in replacePart)
        {
            if (!unique.Keys.Contains(p.SubPartName))
            {
                unique.Add(p.SubPartName, 1);
            }
            else
            {
                unique[p.SubPartName] += 1;
            }
        }
        
        for (int i = 0; i < replacePartView.Count; i++)
        {
            SubPartViewControl ctrl = replacePartView[i];

            if (i < unique.Count)
            {
                string subpartname = unique.Keys.ToList()[i];
                ctrl.ThePart = replacePart.Find((p) => { return p.SubPartName == subpartname; });
                ctrl.CountOfPart = unique[subpartname];
                ctrl.gameObject.SetActive(true);
            }
        }

        for (int i = unique.Keys.Count; i < replacePartView.Count; i++)
        {
            SubPartViewControl ctrl = replacePartView[i];
            ctrl.gameObject.SetActive(false);
        }
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



    // Update is called once per frame
    void Update () {
		
	}

    public void OnBack()
    {
        if (modelInstance != null)
            Destroy(modelInstance);

        videoPanel.SetActive(false);
        NavigationService.Instance.Back();
    }

    public void CloseVideoPanel()
    {
        videoPlayer.Stop();
        videoPanel.SetActive(false);

        if (modelInstance != null)
            modelInstance.SetActive(true);
    }


}
