using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ImageClick : MonoBehaviour, IPointerClickHandler
{
    private ExpertPageController ctr;
    private void Start()
    {
        ctr = transform.parent.parent.GetComponent<ExpertPageController>();
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        ctr.insertedPicture.sprite = transform.GetComponent<Image>().sprite;
    }
}
