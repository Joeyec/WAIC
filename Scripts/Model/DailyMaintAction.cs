using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ActionTypeEnum
{
    Inspect = 0,
    Clean
}

public class DailyMaintAction {

    public string ActionGuid;
    public string ActionName;
    public string ActionDesc;
    public ActionTypeEnum ActionType;
    public string PictureUrl;
    public string BigPictureUrl;
    public bool Completed;
    public string UserGuid; //TODO: leave for the future

    public DailyMaintAction(string name,  string smallUrl, string bigUrl, ActionTypeEnum actionType)
    {
        ActionName = name;
        PictureUrl = smallUrl;
        BigPictureUrl = bigUrl;
        ActionType = actionType;
    }

}
