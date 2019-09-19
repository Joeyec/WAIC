using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class DailyMaintPageController : MonoBehaviour {

    List<DailyMaintAction> actions;

    public MaintenanceType CurrentMaintType;

    private ActionTypeEnum CurrentActionType;

    List<ActionImageControl> imageControls;
    private GameObject bigImage;

    private VideoClip inspectVideo;
    private VideoClip cleanVideo;
    private VideoPlayer videoPlayer;
    private GameObject playButton;

	// Use this for initialization
	void Start () {

        //load videos;
        
       
        


        //bigImage = transform.Find("CheckPhotoPanel/BigImage").gameObject;

        //for (int i =0; i < 4)

        //register the click event
        foreach (ActionImageControl imageCtrl in imageControls)
        {
            imageCtrl.OnImageClick = OnImageClick;
        }

        
    }

    private void OnEnable()
    {
        if (imageControls == null)
            imageControls = GetComponentsInChildren<ActionImageControl>().OrderBy((p) => { return p.name; }).ToList();

        if (videoPlayer == null)
            videoPlayer = transform.Find("Video/VideoPlayer").GetComponent<VideoPlayer>();

        videoPlayer.SetTargetAudioSource(0, transform.Find("Video/AudioSource").GetComponent<AudioSource>());

        if (playButton == null)
            playButton = transform.Find("Video/Play").gameObject;

        

        switch (CurrentMaintType)
        {
            case MaintenanceType.Mechanical:

                inspectVideo = Resources.Load<VideoClip>("Videos/MachineInspect");

                cleanVideo = Resources.Load<VideoClip>("Videos/MachineClean");
                break;
            case MaintenanceType.Electrical:
                inspectVideo = Resources.Load<VideoClip>("Videos/ElectricInspectVideo");

                cleanVideo = Resources.Load<VideoClip>("Videos/ElectricCleanVideo");
                break;
            default:
                break;
        }

        if (actions == null)
            LoadActions();

        switch (CurrentActionType)
        {
            case ActionTypeEnum.Inspect:
                OnInspect();
                break;
            case ActionTypeEnum.Clean:
                OnClean();
                break;
            default:
                break;
        }
        //CurrentActionType = ActionTypeEnum.Inspect;
        //OnInspect();


    }

    private void OnImageClick(DailyMaintAction action)
    {
        GameObject bigImagePage = transform.parent.Find("BigImagePage").gameObject;
        bigImagePage.GetComponent<BigImagePageController>().TheAction = action;
        NavigationService.Instance.NavigateTo(bigImagePage);
    }

    // Update is called once per frame
    void Update () {

        

	}

    public void OnInspect()
    {
        GameObject button1 = transform.Find("CheckPhotoPanel/Check").gameObject;
        button1.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/NewUI/DailyMaint/check");
        GameObject button2 = transform.Find("CheckPhotoPanel/Clean").gameObject;
        button2.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/NewUI/DailyMaint/clean_clicked");
        CurrentActionType = ActionTypeEnum.Inspect;

        List<DailyMaintAction> inspectActoin = actions.Where((p) => {
            return p.ActionType == ActionTypeEnum.Inspect 
                && (CurrentMaintType == MaintenanceType.Mechanical ? !p.ActionName.StartsWith("Electric") : p.ActionName.StartsWith("Electric"));

        }).OrderBy((p) => { return p.ActionName; }).ToList();
        SetImageControls(inspectActoin);

        videoPlayer.clip = inspectVideo;
        //videoPlayer.Prepare();
        //videoPlayer.frame = 0;
        //videoPlayer.Pause();
    }

    public void OnClean()
    {
        GameObject button1 = transform.Find("CheckPhotoPanel/Check").gameObject;
        button1.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/NewUI/DailyMaint/check_clicked");
        GameObject button2 = transform.Find("CheckPhotoPanel/Clean").gameObject;
        button2.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/NewUI/DailyMaint/clean");
        CurrentActionType = ActionTypeEnum.Clean;
        List<DailyMaintAction> cleanActoins = actions.Where((p) => {
            return p.ActionType == ActionTypeEnum.Clean
                && (CurrentMaintType == MaintenanceType.Mechanical ? !p.ActionName.StartsWith("Electric") : p.ActionName.StartsWith("Electric"));
        }).OrderBy((p) => { return p.ActionName; }).ToList();
        SetImageControls(cleanActoins);

        videoPlayer.clip = cleanVideo;
        //videoPlayer.Prepare();
        //videoPlayer.frame = 0;
        //videoPlayer.Pause();
    }

    public void OnPlayVideo()
    {
        GameObject PlayButton = transform.Find("Video/Play").gameObject;
        Image PlayButtonImage = PlayButton.GetComponent<Image>();
        PlayButtonImage.sprite = Resources.Load<Sprite>("Sprites/PlayPauseButton/Play2");
        GameObject PauseButton = transform.Find("Video/Pause").gameObject;
        Image PauseButtonImage = PauseButton.GetComponent<Image>();
        PauseButtonImage.sprite = Resources.Load<Sprite>("Sprites/PlayPauseButton/Pause1");
        videoPlayer.Play();
    }

    public void OnPauseVideo()
    {
        GameObject PlayButton = transform.Find("Video/Play").gameObject;
        Image PlayButtonImage = PlayButton.GetComponent<Image>();
        PlayButtonImage.sprite = Resources.Load<Sprite>("Sprites/PlayPauseButton/Play1");
        GameObject PauseButton = transform.Find("Video/Pause").gameObject;
        Image PauseButtonImage = PauseButton.GetComponent<Image>();
        PauseButtonImage.sprite = Resources.Load<Sprite>("Sprites/PlayPauseButton/Pause2");
        videoPlayer.Pause();
    }

    public void OnBack()
    {
        NavigationService.Instance.Back();
    }

    private void SetImageControls(List<DailyMaintAction> actions)
    {
        for (int i = 0; i < imageControls.Count; i++)
        {
            ActionImageControl control = imageControls[i];

            if (i < actions.Count)
                control.TheAction = actions[i];
        }
    }

    void LoadImage()
    {
        //transform.Find("Image1").GetComponent<Sprite>()=
    }

    void LoadActions()
    {
        //ugly implementation

        actions = new List<DailyMaintAction>();

        DailyMaintAction a = new DailyMaintAction("InspectImage1", "Sprites/Picture/InspectImage/InspectImage1", "Sprites/Picture/InspectImage/BigInspectImage1", ActionTypeEnum.Inspect);
        actions.Add(a);

        a = new DailyMaintAction("InspectImage2", "Sprites/Picture/InspectImage/InspectImage2", "Sprites/Picture/InspectImage/BigInspectImage2", ActionTypeEnum.Inspect);
        actions.Add(a);

        a = new DailyMaintAction("InspectImage3", "Sprites/Picture/InspectImage/InspectImage3", "Sprites/Picture/InspectImage/BigInspectImage3", ActionTypeEnum.Inspect);
        actions.Add(a);

        a = new DailyMaintAction("InspectImage4", "Sprites/Picture/InspectImage/InspectImage4", "Sprites/Picture/InspectImage/BigInspectImage4", ActionTypeEnum.Inspect);
        actions.Add(a);

        a = new DailyMaintAction("CleanImage1", "Sprites/Picture/MachineCleanImage/CleanImage1", "Sprites/Picture/MachineCleanImage/BigCleanImage1", ActionTypeEnum.Clean);
        actions.Add(a);

        a = new DailyMaintAction("CleanImage2", "Sprites/Picture/MachineCleanImage/CleanImage2", "Sprites/Picture/MachineCleanImage/BigCleanImage2", ActionTypeEnum.Clean);
        actions.Add(a);

        a = new DailyMaintAction("CleanImage3", "Sprites/Picture/MachineCleanImage/CleanImage3", "Sprites/Picture/MachineCleanImage/BigCleanImage3", ActionTypeEnum.Clean);
        actions.Add(a);

        a = new DailyMaintAction("CleanImage4", "Sprites/Picture/MachineCleanImage/CleanImage4", "Sprites/Picture/MachineCleanImage/BigCleanImage4", ActionTypeEnum.Clean);
        actions.Add(a);

        a = new DailyMaintAction("ElectricInspect1", "Sprites/Picture/ElectricInspect/ElectricInspect1", "Sprites/Picture/ElectricInspect/ElectricInspectBig1", ActionTypeEnum.Inspect);
        actions.Add(a);

        a = new DailyMaintAction("ElectricInspect2", "Sprites/Picture/ElectricInspect/ElectricInspect2", "Sprites/Picture/ElectricInspect/ElectricInspectBig2", ActionTypeEnum.Inspect);
        actions.Add(a);

        a = new DailyMaintAction("ElectricInspect3", "Sprites/Picture/ElectricInspect/ElectricInspect3", "Sprites/Picture/ElectricInspect/ElectricInspectBig3", ActionTypeEnum.Inspect);
        actions.Add(a);

        a = new DailyMaintAction("ElectricInspect4", "Sprites/Picture/ElectricInspect/ElectricInspect4", "Sprites/Picture/ElectricInspect/ElectricInspectBig4", ActionTypeEnum.Inspect);
        actions.Add(a);

        a = new DailyMaintAction("ElectricClean1", "Sprites/Picture/ElectricClean/ElectricClean1", "Sprites/Picture/ElectricClean/ElectricCleanBig1", ActionTypeEnum.Clean);
        actions.Add(a);

        a = new DailyMaintAction("ElectricClean2", "Sprites/Picture/ElectricClean/ElectricClean2", "Sprites/Picture/ElectricClean/ElectricCleanBig2", ActionTypeEnum.Clean);
        actions.Add(a);

        a = new DailyMaintAction("ElectricClean3", "Sprites/Picture/ElectricClean/ElectricClean3", "Sprites/Picture/ElectricClean/ElectricCleanBig3", ActionTypeEnum.Clean);
        actions.Add(a);

        a = new DailyMaintAction("ElectricClean4", "Sprites/Picture/ElectricClean/ElectricClean4", "Sprites/Picture/ElectricClean/ElectricCleanBig4", ActionTypeEnum.Clean);
        actions.Add(a);

        /*
        switch (CurrentMaintType)
        {
            case MaintenanceType.Mechanical:
                {
                    DailyMaintAction action1 = new DailyMaintAction("InspectImage1", "Sprites/Picture/InspectImage/InspectImage1", "Sprites/Picture/InspectImage/BigInspectImage1", ActionTypeEnum.Inspect);
                    actions.Add(action1);

                    DailyMaintAction action2 = new DailyMaintAction("InspectImage2", "Sprites/Picture/InspectImage/InspectImage2", "Sprites/Picture/InspectImage/BigInspectImage2", ActionTypeEnum.Inspect);                  
                    actions.Add(action2);

                    DailyMaintAction action3 = new DailyMaintAction("InspectImage3", "Sprites/Picture/InspectImage/InspectImage3", "Sprites/Picture/InspectImage/BigInspectImage3", ActionTypeEnum.Inspect);                    
                    actions.Add(action3);

                    DailyMaintAction action4 = new DailyMaintAction("InspectImage4", "Sprites/Picture/InspectImage/InspectImage4", "Sprites/Picture/InspectImage/BigInspectImage4", ActionTypeEnum.Inspect);                   
                    actions.Add(action4);

                    DailyMaintAction action5 = new DailyMaintAction("CleanImage1", "Sprites/Picture/MachineCleanImage/CleanImage1", "Sprites/Picture/MachineCleanImage/BigCleanImage1", ActionTypeEnum.Clean);
                    actions.Add(action5);

                    DailyMaintAction action6 = new DailyMaintAction("CleanImage2", "Sprites/Picture/MachineCleanImage/CleanImage2", "Sprites/Picture/MachineCleanImage/BigCleanImage2", ActionTypeEnum.Clean);
                    actions.Add(action6);

                    DailyMaintAction action7 = new DailyMaintAction("CleanImage3", "Sprites/Picture/MachineCleanImage/CleanImage3", "Sprites/Picture/MachineCleanImage/BigCleanImage3", ActionTypeEnum.Clean);
                    actions.Add(action7);

                    DailyMaintAction action8 = new DailyMaintAction("CleanImage4", "Sprites/Picture/MachineCleanImage/CleanImage4", "Sprites/Picture/MachineCleanImage/BigCleanImage4", ActionTypeEnum.Clean);
                    actions.Add(action8);
                }
                break;

            case MaintenanceType.Electrical:
                {
                    DailyMaintAction action1 = new DailyMaintAction("ElectricInspect1", "Sprites/Picture/ElectricInspect/ElectricInspect1", "Sprites/Picture/ElectricInspect/ElectricInspectBig1", ActionTypeEnum.Inspect);
                    actions.Add(action1);

                    DailyMaintAction action2 = new DailyMaintAction("ElectricInspect2", "Sprites/Picture/ElectricInspect/ElectricInspect2", "Sprites/Picture/ElectricInspect/ElectricInspectBig2", ActionTypeEnum.Inspect);
                    actions.Add(action2);

                    DailyMaintAction action3 = new DailyMaintAction("ElectricInspect3", "Sprites/Picture/ElectricInspect/ElectricInspect3", "Sprites/Picture/ElectricInspect/ElectricInspectBig3", ActionTypeEnum.Inspect);
                    actions.Add(action3);

                    DailyMaintAction action4 = new DailyMaintAction("ElectricInspect4", "Sprites/Picture/ElectricInspect/ElectricInspect4", "Sprites/Picture/ElectricInspect/ElectricInspectBig4", ActionTypeEnum.Inspect);
                    actions.Add(action4);

                    DailyMaintAction action5 = new DailyMaintAction("ElectricClean1", "Sprites/Picture/ElectricClean/ElectricClean1", "Sprites/Picture/ElectricClean/ElectricCleanBig1", ActionTypeEnum.Clean);
                    actions.Add(action5);

                    DailyMaintAction action6 = new DailyMaintAction("ElectricClean2", "Sprites/Picture/ElectricClean/ElectricClean2", "Sprites/Picture/ElectricClean/ElectricCleanBig2", ActionTypeEnum.Clean);
                    actions.Add(action6);

                    DailyMaintAction action7 = new DailyMaintAction("ElectricClean3", "Sprites/Picture/ElectricClean/ElectricClean3", "Sprites/Picture/ElectricClean/ElectricCleanBig3", ActionTypeEnum.Clean);
                    actions.Add(action7);

                    DailyMaintAction action8 = new DailyMaintAction("ElectricClean4", "Sprites/Picture/ElectricClean/ElectricClean4", "Sprites/Picture/ElectricClean/ElectricCleanBig4", ActionTypeEnum.Clean);
                    actions.Add(action8);
                }
                break;
        }
        */

        
    }

    public void OnFactorExpert()
    {
        GameObject expertPage = transform.parent.Find("ExpertPage").gameObject;
        expertPage.GetComponent<ExpertPageController>().CurrentSource = ExpertSourceEnum.Factory;

        NavigationService.Instance.NavigateTo(expertPage);
    }

    public void OnVendorExpert()
    {
        GameObject expertPage = transform.parent.Find("ExpertPage").gameObject;
        expertPage.GetComponent<ExpertPageController>().CurrentSource = ExpertSourceEnum.Vendor;

        NavigationService.Instance.NavigateTo(expertPage);
    }
}
