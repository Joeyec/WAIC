using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IotDevice
{
    public string DeviceName;
    public string DeviceGuid;
    public IotDeviceTypeEnum DeviceType;
    public List<DeviceCurve> Curves;

    public IotDevice(IotDeviceTypeEnum deviceType, string name)
    {
        Curves = new List<DeviceCurve>();
        DeviceType = deviceType;
        DeviceName = name;
    }


}
