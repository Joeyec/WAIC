using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SpeciatyEnum
{
    Mechanical = 0,
    Eletrical
}

public enum ExpertSourceEnum
{
    Factory = 0,
    Vendor,
    Industry
}

public class Expert {

    public string ExpertName;
    public string ExpertGuid;
    public string ExpertGender;
    public SpeciatyEnum ExpertSpecialty; //
    public ExpertSourceEnum ExpertSource;
    public string HeadImageUrl;

    public Expert(string name, ExpertSourceEnum source, string url)
    {
        ExpertName = name;
        ExpertSource = source;
        HeadImageUrl = url;
    }

}
