using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum IotDeviceTypeEnum

{
    ServoMotor=0,
    Sensor,
    Solenoid
}
public class IotDeviceType 
 {
    public string TypeName;
    public string PictureUrl;
    public IotDeviceTypeEnum DeviceType;
    public string TypeTitleUrl;

    public IotDeviceType(IotDeviceTypeEnum deviceType, string typeName, string url ,string titleurl)
    {
        DeviceType = deviceType;
        TypeName = typeName;
        PictureUrl = url;
        TypeTitleUrl = titleurl;
    }
}
