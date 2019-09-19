using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum CycleType
{
    Replace = 0,
    Luburicate
}

public class SubPart {

    public string SubPartName;
    public string SubPartGuid;
    public int SubPartNumber;
    public PartTypeEnum PartType;
    public CycleType CycleType;
    public int CycleInMonth;
    public DateTime LastCycleTime;
    public string VideoUrl;

    public SubPart(string name, int partNumber, PartTypeEnum partype, CycleType cycleType, int month)
    {
        SubPartName = name;
        SubPartNumber = partNumber;
        PartType = partype;
        CycleType = cycleType;
        CycleInMonth = month;
    }
}
