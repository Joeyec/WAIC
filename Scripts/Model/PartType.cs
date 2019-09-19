using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PartTypeEnum{
    None = -1,
    Bearing = 0,
    LinearGuide,
    Belt
}

public class PartType {

    public PartTypeEnum TypeId;
    public string TypeName;
    public string TypePictureUrl;

    public PartType(PartTypeEnum typeId, string name, string url)
    {
        TypeId = typeId;
        TypeName = name;
        TypePictureUrl = url;
    }
}
