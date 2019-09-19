using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineAssembly
{
    public string AssemblyName;
    public string AssemblyGuid;
    public string AssemblyModelUrl;
    public string AssemblyHilightUrl;
    public bool Completed;
    public string AssemblyDes;
    public string AssemblyID;
    public List<MachinePart> PartList;

    public MachineAssembly(string name)
    {
        AssemblyName = name;
        PartList = new List<MachinePart>();
    }

}
