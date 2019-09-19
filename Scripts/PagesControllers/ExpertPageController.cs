using HoloLogicLibs.Azure;
using HoloToolkit.Unity;
using HoloToolkit.Unity.SpatialMapping;
using HoloToolkit.Unity.UX;
using Newtonsoft.Json.Linq;
using RemoteAssistControl;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TriLib;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.WSA;

public class ExpertPageController : MonoBehaviour {

    const int CAPTURE_WIDTH = 896; //1280;
    const int CAPTURE_HEIGHT = 504; //720;
    const int CAPTURE_RATE = 30;

    private const string SDKUserName = "18916153434";
    private const string SDKPassword = "12345";

    private List<byte> tobeProcessed = null;
    public WebRtcVideoPlayer RemoteVideo;

    private IntPtr spatialCoordinateSystemPtr = IntPtr.Zero;

    const string SESSION_GUID = "7c2640ec-df99-3113-9034-aa9d81f217ea";

    List<Expert> experts;
    public ExpertSourceEnum CurrentSource;

    List<ExpertViewControl> expertViews;
    GameObject pictureContainer;
    GameObject pictureBar;
    GameObject videoContainer;
    [HideInInspector]
    public Image insertedPicture;
    

    void Awake()
    {
        


    }

    // Use this for initialization
    async void Start () {

        Conductor.Initialize();
        Conductor.Instance.LocalPeer.PeerName = "HOLOLENS";
        Conductor.Instance.OnInRoom += Instance_OnInRoom;
        Conductor.Instance.OnLeftRoom += Instance_OnLeftRoom;
        Conductor.Instance.SetPreferredVideoCaptureFormat(CAPTURE_WIDTH, CAPTURE_HEIGHT, CAPTURE_RATE, true, false);

        try
        {
            await AzureService.Instance.LogonUser(SDKUserName, SDKPassword);
            await Conductor.Instance.Login(SDKUserName, SDKPassword);
        }
        catch (Exception ex)
        {
            Debug.Log("failed to logon user: "  + ex.Message);
        }


#if UNITY_WSA && !UNITY_EDITOR
        //the following will need to comment when building from Unity

        if (WorldManager.state == PositionalLocatorState.Active)
        {
            Windows.Perception.Spatial.SpatialCoordinateSystem camStreamCS = Windows.Perception.Spatial.SpatialLocator.GetDefault().CreateStationaryFrameOfReferenceAtCurrentLocation().CoordinateSystem;
            Conductor.Instance.SetSpatialCoordinateSystem(camStreamCS);
        }

        // for the future change
        WorldManager.OnPositionalLocatorStateChanged += (PositionalLocatorState oldState, PositionalLocatorState newState) =>
        {

            if (newState == PositionalLocatorState.Active)
            {
                Windows.Perception.Spatial.SpatialCoordinateSystem camStreamCS = Windows.Perception.Spatial.SpatialLocator.GetDefault().CreateStationaryFrameOfReferenceAtCurrentLocation().CoordinateSystem;
                Conductor.Instance.SetSpatialCoordinateSystem(camStreamCS);
            }
        };

#endif

    }

    // Update is called once per frame
    void Update () {
        if (tobeProcessed != null)
        {

            ProcessPicture(tobeProcessed);
            tobeProcessed = null;

        }
    }

    private void OnEnable()
    {
        if (expertViews == null)
        {
            expertViews = transform.GetComponentsInChildren<ExpertViewControl>().ToList();

            foreach (var view in expertViews)
            {
                view.OnExpertClick += OnExpertClick;
            }
        }

        if (experts == null)
            LoadExperts();

        if (videoContainer == null)
        {
            videoContainer = transform.Find("VideoBg").gameObject;
        }

        if (RemoteVideo == null)
        {
            RemoteVideo = transform.Find("VideoBg/VideoImage").GetComponent<WebRtcVideoPlayer>();
        }

        if (pictureContainer == null)
        {
            pictureContainer = transform.Find("InsertedPicturePanel").gameObject;
        }

        if (pictureBar == null)
        {
            pictureBar = transform.Find("PictureBar").gameObject;
        }

        if (insertedPicture == null)
        {
            insertedPicture = transform.Find("InsertedPicturePanel/Image").GetComponent<Image>();
        }
        if (CurrentPage == null)
        {
            CurrentPage = transform.Find("PictureBar/CurrentPage").GetComponent<Text>();
        }

        ShowExperts();
    }

    private void OnExpertClick(Expert expert)
    {
        foreach (var view in expertViews)
        {
            if (view.TheExpert != expert)
            {
                view.IsSelected = false;
            }
        }

        OnJoinClicked();


    }

    private void ShowExperts()
    {
        //set the expert view
        List<Expert> shownExperts = experts.Where((p) => { return p.ExpertSource == CurrentSource; }).ToList();
        for (int i = 0; i < shownExperts.Count; i++)
        {
            Expert expert = shownExperts[i];
            if (i < expertViews.Count)
            {
                ExpertViewControl ctrl = expertViews[i];
                ctrl.TheExpert = expert;
                ctrl.gameObject.SetActive(true);
            }

        }

        for (int i = experts.Count; i < expertViews.Count; i++)
        {
            ExpertViewControl ctrl = expertViews[i];
            ctrl.gameObject.SetActive(false);
        }
    }

    private void LoadExperts()
    {
        //ugly implementation
        experts = new List<Expert>();
        Expert e = new Expert("李玲", ExpertSourceEnum.Factory, "Sprites/Expert/Portrait1");
        experts.Add(e);

        e = new Expert("马寄梅", ExpertSourceEnum.Factory, "Sprites/Expert/Portrait2");
        experts.Add(e);

        e = new Expert("吕代薇", ExpertSourceEnum.Factory, "Sprites/Expert/Portrait3");
        experts.Add(e);

        e = new Expert("林莽", ExpertSourceEnum.Factory, "Sprites/Expert/Portrait6");
        experts.Add(e);

        e = new Expert("刘巧安", ExpertSourceEnum.Vendor, "Sprites/Expert/Portrait4");
        experts.Add(e);

        e = new Expert("李山荷", ExpertSourceEnum.Vendor, "Sprites/Expert/Portrait5");
        experts.Add(e);

        e = new Expert("赵南青", ExpertSourceEnum.Vendor, "Sprites/Expert/Portrait7");
        experts.Add(e);

        e = new Expert("Elton Gates", ExpertSourceEnum.Vendor, "Sprites/Expert/Portrait8");
        experts.Add(e);
        
    }

    private void OnApplicationQuit()
    {
        //house clean for unexpected situation
        if (Conductor.Instance.CurrentRoom != null)
        {
            foreach (Peer peer in Conductor.Instance.CurrentRoom.Peers)
            {

                peer.OnStringMessage -= Peer_OnStringMessage;
                peer.OnBinaryMessage -= Peer_OnBinaryMessage;
                peer.OnRemoteArgbWithCameraTransform -= Peer_OnRemoteArgbWithCameraTransform;

                peer.ClosePeerConnection();
            }

        }

        Conductor.Instance.OnInRoom -= Instance_OnInRoom;
        Conductor.Instance.OnLeftRoom -= Instance_OnLeftRoom;
    }

    private void EnableAllExpers(bool enabled)
    {
        foreach (var view in expertViews)
        {
            view.IsEnabled = enabled;
        }
    }


    public async void OnJoinClicked()
    {
        Debug.Log("Joining session: " + SESSION_GUID);

        EnableAllExpers(false);
        videoContainer.SetActive(true);

        RtcRoom rtcRoom = null;
        try
        {
            CancellationToken cancellation = new CancellationToken();
            JToken result = await AzureService.Instance.findRoomForSpace(SESSION_GUID, cancellation);
            rtcRoom = result.ToObject<RtcRoom>();

            if (rtcRoom != null)
            {
                await EnterSession(rtcRoom.RoomNumber.ToString(), rtcRoom.PinCode);
            }

        }
        catch (Exception ex)
        {

            Debug.Log("failed to find room: " + ex.Message);

            if (ex.Message == "room not found")
            {

                //no room, create it
                try
                {
                    rtcRoom = await NewSession();

                    CancellationToken cancellation = new CancellationToken();
                    /*
                    var serializer = new JsonSerializer
                    {
                        ContractResolver = new CamelCasePropertyNamesContractResolver()
                    };
                    */
                    await AzureService.Instance.updateRoomForSpace(SESSION_GUID, JToken.FromObject(rtcRoom), cancellation);                    
                }
                catch (Exception subEx)
                {
                    EnableAllExpers(true);
                    Debug.Log("failed to create room: " + subEx.Message);
                    showErr("无法加入会话");
                }
            }
            else if (ex.Message == "locked")
            {
                EnableAllExpers(true);
                showErr("另一位参与者正在创建会话，请稍候尝试");
            }
            else
            {
                EnableAllExpers(true);
                showErr("无法加入会话");
            }
        }
    }

    private void showErr(string msg)
    {
        PopupMessage.Instance.MessageText = msg;
        PopupMessage.Instance.ShowAt(transform.position - new Vector3(0f, 0f, 0.2f));
    }

    async public Task<RtcRoom> NewSession()
    {
        RtcRoom rtcRoom = await Conductor.Instance.CreateRtcRoom();

        if (rtcRoom != null)
        {
            //SessionNumber = rtcRoom.RoomNumber.ToString();
            //PinCode = rtcRoom.PinCode;
            return rtcRoom;
        }

        return null;
    }

    async public Task<RtcRoom> EnterSession(string sessionNumber, string pinCode)
    {
        RtcRoom rtcRoom = null;
        try
        {
            long roomNumber = long.Parse(sessionNumber);
            rtcRoom = await Conductor.Instance.EnterRtcRoom(roomNumber, pinCode);

            //SessionNumber = rtcRoom.RoomNumber.ToString();
            //PinCode = rtcRoom.PinCode;
            //Connected = true;

            foreach (Peer peer in rtcRoom.Peers)
            {
                peer.OnStringMessage += Peer_OnStringMessage;
                peer.OnBinaryMessage += Peer_OnBinaryMessage;
                peer.OnRemoteArgbWithCameraTransform += Peer_OnRemoteArgbWithCameraTransform;

                await peer.Call();
            }

        }
        catch (FormatException ex)
        {
            Debug.Log("wrong format of session number");
        }

        return rtcRoom;
    }

    public async void LeaveSession()
    {
        RtcRoom rtcRoom = Conductor.Instance.CurrentRoom;

        if (rtcRoom != null)
        {

            foreach (Peer peer in rtcRoom.Peers)
            {
                //since we call each other, we should say bye to each other
                Task t = peer.SendBye();

                peer.OnStringMessage -= Peer_OnStringMessage;
                peer.OnBinaryMessage -= Peer_OnBinaryMessage;
                peer.OnRemoteArgbWithCameraTransform -= Peer_OnRemoteArgbWithCameraTransform;
            }

            await Conductor.Instance.LeaveRtcRoom();
        }


        DeselectAllExperts();
        EnableAllExpers(true);
        videoContainer.SetActive(false);
        pictureContainer.SetActive(false);
        pictureBar.SetActive(false);

    }

    private void DeselectAllExperts()
    {
        foreach (var view in expertViews)
        {
            view.IsSelected = false;
        }
    }

    public void ClearLastIndicator()
    {

        ArrowIndicator.PopIndicator();

    }

    private void Instance_OnInRoom(Peer peer)
    {
        peer.OnStringMessage += Peer_OnStringMessage;
        peer.OnBinaryMessage += Peer_OnBinaryMessage;
        peer.OnRemoteArgbWithCameraTransform += Peer_OnRemoteArgbWithCameraTransform;
        
    }

    private void Instance_OnLeftRoom(Peer peer)
    {
        peer.OnStringMessage -= Peer_OnStringMessage;
        peer.OnBinaryMessage -= Peer_OnBinaryMessage;
        peer.OnRemoteArgbWithCameraTransform -= Peer_OnRemoteArgbWithCameraTransform;
    }

    private void Peer_OnBinaryMessage(Peer peer, List<byte> obj)
    {
        tobeProcessed = obj;
    }

    private void Peer_OnStringMessage(Peer peer, string obj)
    {
        //throw new NotImplementedException();
    }

    private void Peer_OnRemoteArgbWithCameraTransform(Peer peer, uint width, uint height, byte[] buffer, float posX, float posY, float poxZ, float rotX, float rotY, float rotZ, float rotW)
    {
        //FramePacket packet = new FramePacket((int)width * (int)height * 4);
        FramePacket packet = new FramePacket(buffer);

        if (packet == null)
        {
            Debug.Log("frame packet cannot be created.");
        }

        packet.width = (int)width;
        packet.height = (int)height;

        //buffer.CopyTo(packet.Buffer, 0);

        RemoteVideo.FrameQueue.Push(packet);
    }

    private void ProcessPicture(List<byte> rawbytes)
    {

        MessageParser parser = new MessageParser(rawbytes);

        switch (parser.MessageType)
        {

            case (int)BinaryMessageType.VideoStreamWithCameraTransform:
                {
                    GameObject tempCam = new GameObject();
                    tempCam.transform.position = new Vector3(parser.CameraPosition.X, parser.CameraPosition.Y, parser.CameraPosition.Z);
                    tempCam.transform.rotation = new Quaternion(parser.CameraRotation.X, parser.CameraRotation.Y, parser.CameraRotation.Z, parser.CameraRotation.W);

                    float pictureDistance = Camera.main.nearClipPlane + 0.01f;

                    RaycastHit spatialMapHit;

                    if (SpatialMappingRaycast(tempCam.transform.position, tempCam.transform.forward, out spatialMapHit))
                    {
                        pictureDistance = spatialMapHit.distance;  //0.05 is a magic number to adjust the real distance
                    }

                    ShowDrawing(parser.Payload, tempCam.transform, pictureDistance);

                }
                break;

            case (int)BinaryMessageType.Picture:
                {
                    ShowPicture(parser.Payload);
                }

                break;

            case (int)BinaryMessageType.SpatialIndicator:
                {
                    Vector3 CamPosition = new Vector3(parser.CameraPosition.X, parser.CameraPosition.Y, parser.CameraPosition.Z);
                    Quaternion CamRotation = new Quaternion(parser.CameraRotation.X, parser.CameraRotation.Y, parser.CameraRotation.Z, parser.CameraRotation.W);

                    int width = parser.ImageWidth;
                    int height = parser.ImageHeight;

                    Debug.Log("width: " +  width + "  height: " + height);

                    GameObject temp = new GameObject();

                    temp.transform.position = CamPosition;
                    temp.transform.rotation = CamRotation;

                    float scaleFactor = 0.001f;

                    //float realDistance = height / (2.0f * Mathf.Tan(0.5f * Camera.main.fieldOfView * Mathf.Deg2Rad)) * scaleFactor; //0.001 is the scale of the 

                    //Debug.Log("realDistance: " + realDistance);

                    //realDistance += 0.4f; //magic number to adjust the positon
                    float realDistance = 1.50f;

                    temp.transform.Translate(Vector3.forward * realDistance, Space.Self);
                    Vector3 central = temp.transform.position;

                    foreach (RemoteAssistControl.Numerics.Point point in parser.IndicatorPostions)
                    {
                        Debug.Log("point.x: " + point.x + " point.y: " + point.y);
                        temp.transform.position = central; //reset to central

                        //need to compensate indicator's position, 40 is the size of the indicator
                        RemoteAssistControl.Numerics.Point realPoint = new RemoteAssistControl.Numerics.Point(point.x, point.y);

                        //the origin of the picture if at topleft
                        Vector3 translation = new Vector3(((float)realPoint.x + 103 - width / 2 ) * scaleFactor, (height / 2 - 118 - (float)realPoint.y) * scaleFactor, 0);

                        temp.transform.Translate(translation, Space.Self);

                        //Debug.Log(" originalCamPosition " + originalCamPosition + " central: " + central + " realpoint: " + realPoint + " translation: " + translation + " new position: " + temp.transform.position);

                        //Debug.Log(" cast direction " + (temp.transform.position - originalCamPosition).normalized + " camera forward: " + temp.transform.forward);

                        RaycastHit spatialMapHit;
                        if (SpatialMappingRaycast(CamPosition, (temp.transform.position - CamPosition).normalized, out spatialMapHit))
                        {
                            GameObject indicator = Instantiate(Resources.Load<GameObject>("Prefabs/Arrow"));
                            indicator.transform.position = spatialMapHit.point;
                            indicator.transform.up = spatialMapHit.normal;

                            indicator.AddComponent<WorldAnchor>();

                            Debug.Log("hitPostioin: " + spatialMapHit.point + " hitNormal: " + spatialMapHit.normal);
                        }

                    }
                }
                break;

            case (int)BinaryMessageType.Model:
                {
                    string fileExtension = parser.FileExtension;

                    using (var assetLoaderAsync = new AssetLoaderAsync())
                    {
                        assetLoaderAsync.LoadFromMemory(parser.Payload, fileExtension, null, null, (loadedGameObject) =>
                        {

                            GameObject BBParent = Instantiate(Resources.Load<GameObject>("Prefabs/BBParent"));
                            BBParent.transform.position = Vector3.zero;
                            loadedGameObject.transform.position = Vector3.zero;
                            loadedGameObject.transform.SetParent(BBParent.transform);

                            BoundingBoxRig bbr = BBParent.GetComponent<BoundingBoxRig>();
                            BoundingBox bb = bbr.BoundingBoxPrefab;
                            bb.Target = BBParent;
                            //force BoundingBox to call RefreshTargetBounds, ideally the function should be public
                            bb.RefreshTargetBounds();

                            Vector3 position = bb.TargetBoundsCenter;
                            Vector3 size = bb.TargetBoundsLocalScale;

                            Debug.Log("TargetBoundsCenter: " + position + "TargetBoundsScale: " + size);

                            //this is tricky, we get the box collider center, and then we need move the bbparent to the position.
                            //in order not to affect the model, we disconnect the bbparent temporarily

                            loadedGameObject.transform.parent = null;
                            BBParent.transform.position = position;

                            BoxCollider bc = BBParent.EnsureComponent<BoxCollider>();
                            bc.center = Vector3.zero;
                            bc.size = size;
                            loadedGameObject.transform.SetParent(BBParent.transform);

                            //scale the model to fit in the fov at the 2 meter position
                            float desiredHeight = 1.5f / (2.0f * Mathf.Tan(0.5f * Camera.main.fieldOfView * Mathf.Deg2Rad));

                            //divide another 2 is because that the RGB camer FOV is 2 times bigger than the real fov
                            float scaleFactor = desiredHeight / Mathf.Max(new float[] { size.x, size.y, size.z }) / 2 / 4;

                            BBParent.transform.position = Camera.main.transform.position;
                            BBParent.transform.rotation = Camera.main.transform.rotation;
                            BBParent.transform.Translate(Vector3.forward * 1.5f, Space.Self);
                            Vector3 directionToTarget = Camera.main.transform.position - BBParent.transform.position;
                            BBParent.transform.rotation = Quaternion.LookRotation(-directionToTarget);

                            BBParent.transform.localScale = new Vector3(scaleFactor, scaleFactor, scaleFactor);

                        });
                    }

                }
                break;

            default:
                break;

        }

    }

    public void PrePage()
    {
        if (currentPage > 1)
        {
            currentPage -= 1;
            for (int i = (currentPage-1)*5; i < currentPage*5; i++)
            {
                Images[i].sprite = ImageSprites[i];
            }
        }
        CurrentPage.text = currentPage + "/" + totalPage;
        
    }
    public void NextPage()
    {
        if (currentPage < totalPage - 1)
        {
            currentPage += 1;
            for (int i = (currentPage - 1) * 5; i < currentPage * 5; i++)
            {
                Images[i].sprite = ImageSprites[i];
            }
        }
        else if (currentPage == totalPage - 1)
        {
            currentPage += 1;
            for (int i = (currentPage - 1) * 5; i < (currentPage-1)*5+restImage; i++)
            {
                Images[i].sprite = ImageSprites[i];
            }
            for (int i = 0; i < 5-restImage; i++)
            {
                Images[i].gameObject.SetActive(false);
            }

        }
        CurrentPage.text = currentPage + "/" + totalPage;
    }
    List<Sprite> ImageSprites;
    public Image[] Images;
    int totalPage;
    int currentPage=1;
    int restImage;
    Text CurrentPage; 
    private void ShowPicture(byte[] pictureData)
    {
     
        //GameObject InsertedPictureInstance = Instantiate(Resources.Load<GameObject>("Prefabs/InsertedPicture"));

        //InsertedPictureInstance.SetActive(true);

        Texture2D texture = new Texture2D(1, 1);  //the size will be replaced with the real image size after the LoaImage call
        texture.LoadImage(pictureData);
        texture.Apply();
        Sprite sprite = Sprite.Create(texture, new Rect(0f, 0f, texture.width, texture.height), new Vector2(0.5f, 0.5f));

        ImageSprites = new List<Sprite>();
        ImageSprites.Add(sprite);
        int totolPage = (ImageSprites.Count() / 5)+1;
        int restImage = ImageSprites.Count() % 5;
        if (totalPage <= 1)
        {
            for (int i = 0; i < restImage; i++)
            {
                Images[i].sprite = ImageSprites[i];
            }
            for (int i = restImage; i <= 5; i++)
            {
                Images[i].gameObject.SetActive(false);
            }
        }
        else
        {
            for (int i = 0; i < 5; i++)
            {
                Images[i].sprite = ImageSprites[i];
            }
        }
        CurrentPage.text = currentPage + "/" + totalPage;
        // insertedPicture.sprite = sprite;
        pictureBar.SetActive(true);
        pictureContainer.SetActive(true);

        /*
        Image image = InsertedPictureInstance.transform.Find("Panel/Canvas/Image").GetComponent<Image>();
        image.sprite = sprite;
        image.preserveAspect = true;
        image.type = Image.Type.Simple;
        */

        //UIController.Instance.AddToCollection(InsertedPictureInstance);

        /*
        GameObject InsertedPictureInstance = Instantiate(Resources.Load<GameObject>("Prefabs/InsertedPicture"));

        InsertedPictureInstance.SetActive(true);

        Texture2D texture = new Texture2D(1, 1);  //the size will be replaced with the real image size after the LoaImage call
        texture.LoadImage(pictureData);
        texture.Apply();
        Sprite sprite = Sprite.Create(texture, new Rect(0f, 0f, texture.width, texture.height), new Vector2(0.5f, 0.5f));

        Image image = InsertedPictureInstance.transform.Find("Panel/Canvas/Image").GetComponent<Image>();
        image.sprite = sprite;
        image.preserveAspect = true;
        image.type = Image.Type.Simple;
        
        InsertedPictureInstance.transform.position = Camera.main.transform.position;
        InsertedPictureInstance.transform.rotation = Camera.main.transform.rotation;

        InsertedPictureInstance.transform.Translate(Vector3.forward * 2.0f, Space.Self);

        Vector3 directionToTarget = Camera.main.transform.position - InsertedPictureInstance.transform.position;

        InsertedPictureInstance.transform.rotation = Quaternion.LookRotation(-directionToTarget);
        */
    }


    private void ShowDrawing(byte[] pictureData, Transform cameraTransform, float pictureDistance)
    {
        GameObject DrawingInstance = Instantiate(Resources.Load<GameObject>("Prefabs/InsertedDrawingCanvas"));
        //DrawingInstance.SetActive(true);

        Texture2D texture = new Texture2D(1, 1);  //the size will be replaced with the real image size after the LoaImage call
        texture.LoadImage(pictureData);
        texture.Apply();
        Sprite sprite = Sprite.Create(texture, new UnityEngine.Rect(0f, 0f, texture.width, texture.height), new Vector2(0.5f, 0.5f));

        Image image = DrawingInstance.transform.Find("Image").GetComponent<Image>();
        image.sprite = sprite;
        image.preserveAspect = true;
        image.type = Image.Type.Simple;

        float realDistance = (float)CAPTURE_HEIGHT / (2.0f * Mathf.Tan(0.5f * Camera.main.fieldOfView * Mathf.Deg2Rad));



        float scalefator = (float)CAPTURE_HEIGHT / (float)texture.height;

        scalefator = (pictureDistance + 0.35f) / realDistance * scalefator;

        Debug.Log("scalefactor: " + scalefator);

        GameObject temp = new GameObject();
        temp.transform.position = cameraTransform.position;
        temp.transform.rotation = cameraTransform.rotation;

        temp.transform.Translate(Vector3.forward * pictureDistance, Space.Self);

        DrawingInstance.transform.position = temp.transform.position;

        Vector3 directionToTarget = cameraTransform.position - DrawingInstance.transform.position;

        DrawingInstance.transform.rotation = Quaternion.LookRotation(-directionToTarget);
        DrawingInstance.transform.localScale = new Vector3(scalefator, scalefator, 0f);

        //magic 
        DrawingInstance.transform.Translate(new Vector3(0.03f, -0.03f, 0.0f));

        Destroy(temp);

        return;

    }

    /// <summary>
    /// Does a raycast on the spatial mapping layer to try to find a hit.
    /// </summary>
    /// <param name="origin">Origin of the raycast</param>
    /// <param name="direction">Direction of the raycast</param>
    /// <param name="spatialMapHit">Result of the raycast when a hit occurred</param>
    /// <returns>Whether it found a hit or not</returns>
    private bool SpatialMappingRaycast(Vector3 origin, Vector3 direction, out RaycastHit spatialMapHit)
    {
        if (SpatialMappingManager.Instance != null)
        {
            RaycastHit hitInfo;
            if (Physics.Raycast(origin, direction, out hitInfo, 30.0f, SpatialMappingManager.Instance.LayerMask))
            {
                spatialMapHit = hitInfo;
                return true;
            }
        }
        spatialMapHit = new RaycastHit();
        return false;
    }

    public void OnBack()
    {

        LeaveSession();
        NavigationService.Instance.Back();
    }
}
