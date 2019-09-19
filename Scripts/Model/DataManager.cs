using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour {

    public static Dictionary<PartTypeEnum, PartType> PartTypes;
    public static Dictionary<IotDeviceTypeEnum, IotDeviceType> IotDeviceTypes;



	// Use this for initialization
	void Start () {

        PartTypes = new Dictionary<PartTypeEnum,PartType>();
        IotDeviceTypes = new Dictionary<IotDeviceTypeEnum, IotDeviceType>();

        //suppose is to load from database

        PartType p = new PartType(PartTypeEnum.Bearing, "轴承", "Sprites/Picture/MachineDailyMaintance/Bearing");
        PartTypes.Add(PartTypeEnum.Bearing, p);

        p = new PartType(PartTypeEnum.LinearGuide, "线性导轨", "Sprites/Picture/MachineDailyMaintance/Linear_Guide");
        PartTypes.Add(PartTypeEnum.LinearGuide, p);

        p = new PartType(PartTypeEnum.Belt, "皮带", "Sprites/Picture/MachineDailyMaintance/Belt");
        PartTypes.Add(PartTypeEnum.Belt, p);


        IotDeviceType d = new IotDeviceType(IotDeviceTypeEnum.ServoMotor, "伺服电机", "Sprites/Picture/OnlineAna/ServoMotor", "Sprites/SubOnlineAna/ServoMotor");
        IotDeviceTypes.Add(d.DeviceType, d);

        d = new IotDeviceType(IotDeviceTypeEnum.Sensor, "传感器", "Sprites/Picture/OnlineAna/Sensor", "Sprites/SubOnlineAna/Sensor");
        IotDeviceTypes.Add(d.DeviceType, d);

        d = new IotDeviceType(IotDeviceTypeEnum.Solenoid, "电磁阀", "Sprites/Picture/OnlineAna/Solenoid", "Sprites/SubOnlineAna/Solenoid");
        IotDeviceTypes.Add(d.DeviceType, d);

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
