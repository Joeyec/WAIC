using HoloToolkit.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PinButtonControl : MonoBehaviour {

    private Tagalong taga;
    public Tagalong Taga
    {
        get
        {
            if (taga == null)
            {
                taga = RootController.Instance.GetComponent<Tagalong>();
            }

            return taga;
        }
        
    }

    private Billboard bill;
    public Billboard Bill
    {
        get
        {
            if (bill == null)
            {
                bill = RootController.Instance.GetComponent<Billboard>();
            }

            return bill;
        }
    }

    private Button theButton;
    public Button TheButton
    {
        get
        {
            if (theButton == null)
            {
                theButton = GetComponent<Button>();
            }

            return theButton;

        }
    }

    SpriteState PinState;
    SpriteState UnpinState;
    Sprite PinSprite;
    Sprite UnpinSprite;

    // Use this for initialization
    void Start () {

        PinState.highlightedSprite = Resources.Load<Sprite>("Sprites/NewUI/ScheduledChange/pin_highlight");
        UnpinState.highlightedSprite = Resources.Load<Sprite>("Sprites/NewUI/ScheduledChange/unpin_highlight");
        PinSprite = Resources.Load<Sprite>("Sprites/NewUI/ScheduledChange/pin");
        UnpinSprite = Resources.Load<Sprite>("Sprites/NewUI/ScheduledChange/unpin");
    }
	
	// Update is called once per frame
	void Update () {

        
        if (Taga.enabled)
        {
            TheButton.image.sprite = PinSprite;
            TheButton.spriteState = PinState;
        }
        else
        {
            TheButton.image.sprite = UnpinSprite;
            TheButton.spriteState = UnpinState;
        }

    }

    public void OnClick()
    {
        Taga.enabled = !Taga.enabled;
        Bill.enabled = !Bill.enabled;

    }
}
