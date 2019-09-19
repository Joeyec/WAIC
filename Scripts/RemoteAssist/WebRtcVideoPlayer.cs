using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WebRtcVideoPlayer : MonoBehaviour {

    private Texture2D tex;
    public FrameQueue FrameQueue; 
    float lastUpdateTime;

    [SerializeField]
    private bool _playing;
    [SerializeField]
    private bool _failed;
    [SerializeField]
    private float _fpsLoad;
    [SerializeField]
    private float _fpsShow;
    [SerializeField]
    private float _fpsSkip;

    // Use this for initialization
    void Start () {

        if (FrameQueue == null)
            FrameQueue = new FrameQueue(5, false);


        tex = new Texture2D(2, 2);
        tex.SetPixel(0, 0, Color.blue);
        tex.SetPixel(1, 1, Color.blue);
        tex.Apply();
        GetComponent<RawImage>().texture = tex;

        //change the material to Remote materia
        //GetComponent<RawImage>().material = SepctatorMat;
        GetComponent<RectTransform>().localRotation = Quaternion.Euler(new Vector3(0, 0, 180));


    }

    // Update is called once per frame
    void Update()
    {

        if (Time.fixedTime - lastUpdateTime > 1.0 / 31.0)
        {
            lastUpdateTime = Time.fixedTime;
            TryProcessFrame();
        }

        if (FrameQueue != null) {
            _fpsLoad = FrameQueue.Stats.fpsLoad();
            _fpsShow = FrameQueue.Stats.fpsShow();
            _fpsSkip = FrameQueue.Stats.fpsSkip();
        }


    }

    private void TryProcessFrame()
    {
        if (FrameQueue != null)
        {
            FramePacket packet = FrameQueue.Pop();
            //Debug.Log((packet == null ? "no frame to consume." : "frame consumed.") + "framesCount : " + frameQueue.Count);
            if (packet != null)
            {
                ProcessFrameBuffer(packet);                
            }
        }
    }

    private void ProcessFrameBuffer(FramePacket packet)
    {
        if (packet == null) {
            return;
        }

        if (tex == null || (tex.width != packet.width || tex.height != packet.height)) {
            Debug.Log("Create Texture. width:"+packet.width+" height:"+packet.height);
            //tex = new Texture2D(packet.width, packet.height, TextureFormat.RGBA32, false);
            tex = new Texture2D(packet.width, packet.height, TextureFormat.BGRA32, false);
        }
        //Debug.Log("Received Packet. " + packet.ToString());
        tex.LoadRawTextureData(packet.Buffer);
    
        tex.Apply();
        GetComponent<RawImage>().texture = tex;
    }
}
