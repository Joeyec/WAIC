using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MachinePart
{
    public string PartName;    
    public string MachinePartGuid;
    public string PartDesc;
    public List<SubPart> SubParts;
    //public string ModelUrl;

    public Dictionary<PartTypeEnum, string> ModelUrls;

    public MachinePart(string name)
    {
        PartName = name;
        SubParts = new List<SubPart>();
        ModelUrls = new Dictionary<PartTypeEnum, string>();
    }

}
