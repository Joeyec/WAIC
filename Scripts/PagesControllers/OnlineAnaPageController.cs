using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class OnlineAnaPageController : MonoBehaviour {

    List<IotDevice> iotDevices;
    List<IotDeviceTypeViewControl> typeControls;

	// Use this for initialization
	void Start () {

        iotDevices = new List<IotDevice>();
        LoadIotDevices();

        typeControls = transform.GetComponentsInChildren<IotDeviceTypeViewControl>().ToList();

        for (int i = 0; i < DataManager.IotDeviceTypes.Count; i ++)
        {
            IotDeviceType iotDeviceType = DataManager.IotDeviceTypes.Values.ToList()[i];
            typeControls[i].TheType = iotDeviceType;

            typeControls[i].OnIotTypeClick += OnIotTypeClick;
        }

    }

    private void OnIotTypeClick(IotDeviceType deviceType)
    {
        GameObject curvePage = transform.parent.Find("IotCurvePage").gameObject;
        IotCurvePageController ctrl = curvePage.GetComponent<IotCurvePageController>();
        ctrl.iotDevices = iotDevices.Where((p) => { return p.DeviceType == deviceType.DeviceType; }).ToList();
        NavigationService.Instance.NavigateTo(curvePage);
        
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void LoadIotDevices()
    {
        //ugly implementation
        IotDevice d;

        d = new IotDevice(IotDeviceTypeEnum.ServoMotor, "传送模块电机");

        DeviceCurve c = new DeviceCurve("电流时间曲线", "Videos/curvenormal");
        d.Curves.Add(c);
        

        c = new DeviceCurve("转速时间曲线", "Videos/curveabnormal");
        d.Curves.Add(c);

        iotDevices.Add(d);

        d = new IotDevice(IotDeviceTypeEnum.ServoMotor, "上压辊电机");

        c = new DeviceCurve("电流时间曲线", "Videos/curvenormal");
        d.Curves.Add(c);


        c = new DeviceCurve("转速时间曲线", "Videos/curvenormal");
        d.Curves.Add(c);

        iotDevices.Add(d);

        d = new IotDevice(IotDeviceTypeEnum.ServoMotor, "下压辊电机");

        c = new DeviceCurve("电流时间曲线", "Videos/curveabnormal");
        d.Curves.Add(c);


        c = new DeviceCurve("转速时间曲线", "Videos/curvenormal");
        d.Curves.Add(c);

        iotDevices.Add(d);




        d = new IotDevice(IotDeviceTypeEnum.Solenoid, "传送模块电磁阀");

        c = new DeviceCurve("I/O时间曲线", "Videos/1recircle");
        d.Curves.Add(c);

        iotDevices.Add(d);

        d = new IotDevice(IotDeviceTypeEnum.Solenoid, "上压辊电磁阀");

        c = new DeviceCurve("I/O时间曲线", "Videos/10notrecircle");
        d.Curves.Add(c);

        iotDevices.Add(d);

        d = new IotDevice(IotDeviceTypeEnum.Solenoid, "下压辊电磁阀");

        c = new DeviceCurve("I/O时间曲线", "Videos/1recircle");
        d.Curves.Add(c);

        iotDevices.Add(d);


        d = new IotDevice(IotDeviceTypeEnum.Sensor, "传送模块入口传感器");

        c = new DeviceCurve("I/O时间曲线", "Videos/10notrecircle");
        d.Curves.Add(c);

        iotDevices.Add(d);

        d = new IotDevice(IotDeviceTypeEnum.Sensor, "传送模块出口传感器");

        c = new DeviceCurve("I/O时间曲线", "Videos/1recircle");
        d.Curves.Add(c);

        iotDevices.Add(d);

        d = new IotDevice(IotDeviceTypeEnum.Sensor, "热压模块入口传感器");

        c = new DeviceCurve("I/O时间曲线", "Videos/1recircle");
        d.Curves.Add(c);

        iotDevices.Add(d);

        d = new IotDevice(IotDeviceTypeEnum.Sensor, "热压模块出口传感器");

        c = new DeviceCurve("I/O时间曲线", "Videos/10notrecircle");
        d.Curves.Add(c);

        iotDevices.Add(d);

    }

    public void OnBack()
    {
        NavigationService.Instance.Back();
    }
}
