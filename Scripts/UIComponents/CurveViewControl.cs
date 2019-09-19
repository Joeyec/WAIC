using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class CurveViewControl : MonoBehaviour {

    private VideoPlayer curvePlayer;

    private DeviceCurve deviceCurve;
    private RenderTexture renderTexture;
    private RawImage targetImage;

    private Text deviceNameText;
    private Text DeviceNameText
    {
        get
        {
            if (deviceNameText == null)
            {
                deviceNameText = transform.Find("CurveName").GetComponent<Text>();
            }

            return deviceNameText;
        }
    }

    private string deviceName;
    public string DeviceName
    {
        get
        {
            return deviceName;
        }

        set
        {
            if (deviceName != value)
            {
                deviceName = value;
                DeviceNameText.text = deviceName;
            }
        }
    }

    public DeviceCurve DeviceCurve
    {
        get
        {
            return deviceCurve;
        }

        set
        {
            if (deviceCurve != value)
            {
                deviceCurve = value;

                if (curvePlayer == null)
                {
                    curvePlayer = transform.Find("VideoPlayer").GetComponent<VideoPlayer>();
                }

                if (renderTexture == null)
                {
                    renderTexture = new RenderTexture(256, 256, 24);
                }

                if (targetImage == null)
                {
                    targetImage = transform.Find("CurveTexture").GetComponent<RawImage>(); ;
                }

                VideoClip clip = Resources.Load<VideoClip>(deviceCurve.CurveVideoUrl);
                curvePlayer.targetTexture = renderTexture;
                targetImage.texture = renderTexture;

                curvePlayer.clip = clip;
                curvePlayer.Play();
            }
        }
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
