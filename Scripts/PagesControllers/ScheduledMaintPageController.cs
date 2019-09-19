using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ScheduledMaintPageController : MonoBehaviour {

    private List<MachineAssembly> assemblies;
    private List<AssemblyViewControl> assControls;

	// Use this for initialization
	void Start () {

        assemblies = new List<MachineAssembly>();
        assControls =  GetComponentsInChildren<AssemblyViewControl>().ToList();
        LoadAssembles();

        //register the click event
        foreach (AssemblyViewControl ctrl in assControls)
        {
            ctrl.OnModelClick += OnModelClick;
        }

    }

    private void OnModelClick(MachineAssembly ass)
    {
        //go to new page
        Debug.Log("going to detail page");
        GameObject partPage = transform.parent.Find("PartPage").gameObject;
        partPage.GetComponent<PartPageController>().TheAss = ass;
        NavigationService.Instance.NavigateTo(partPage);
        transform.parent.GetComponent<ButtonAudio>().ClickButton();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnBack()
    {
        NavigationService.Instance.Back();
    }

    public void LoadAssembles()
    {
        //load part type

        //ugly implementaion
        MachineAssembly a2 = new MachineAssembly("棉芯输送机");        
        a2.AssemblyModelUrl = "Press_module/Models/press_convery_normal";
        a2.AssemblyHilightUrl = "Press_module/Models/press_convery_highlight";
        //load parts

        MachinePart p;
        SubPart s;

        p = new MachinePart("上部传送模块");
        //p.ModelUrls.Add(PartTypeEnum.None, "Press_module/Models/upper_convery");
        p.ModelUrls.Add(PartTypeEnum.Bearing, "Press_module/Models/upper_convery_bearing");
        p.ModelUrls.Add(PartTypeEnum.Belt, "Press_module/Models/upper_convery_belt");
        //p.ModelUrl = "Press_module/Models/upper_convery";

        for (int i = 0; i < 8; i++)
        {
            s = new SubPart("SKF轴承 6204-2Z/C3/2RSH", i, PartTypeEnum.Bearing, CycleType.Replace, 12);
            s.VideoUrl = "Videos/ReplaceBearing";
            p.SubParts.Add(s);
        }

        s = new SubPart("SKF LGLT2/1润滑油", 0, PartTypeEnum.Bearing, CycleType.Luburicate, 6);
        s.VideoUrl = "Videos/LiburateBearing";
        p.SubParts.Add(s);

        s = new SubPart("GATES POWERGRIP 2GT226", 0, PartTypeEnum.Belt, CycleType.Replace, 12);
        s.VideoUrl = "Videos/ReplaceBelt";
        p.SubParts.Add(s);

        a2.PartList.Add(p);

        p = new MachinePart("下部传送模块");
        //p.ModelUrl = "Press_module/Models/lower_convery";
        p.ModelUrls.Add(PartTypeEnum.Bearing, "Press_module/Models/lower_convery_bearing");
        p.ModelUrls.Add(PartTypeEnum.Belt, "Press_module/Models/lower_convery_belt");

        for (int i = 0; i < 8; i++)
        {
            s = new SubPart("SKF轴承 6204-2Z/C3/2RSH", i, PartTypeEnum.Bearing, CycleType.Replace, 12);
            s.VideoUrl = "Videos/ReplaceBearing";
            p.SubParts.Add(s);
        }

        s = new SubPart("SKF LGLT2/1润滑油", 0, PartTypeEnum.Bearing, CycleType.Luburicate, 6);
        s.VideoUrl = "Videos/LiburateBearing";
        p.SubParts.Add(s);

        s = new SubPart("GATES POWERGRIP 2GT226", 0, PartTypeEnum.Belt, CycleType.Replace, 12);
        s.VideoUrl = "Videos/ReplaceBelt";
        p.SubParts.Add(s);

        a2.PartList.Add(p);

        p = new MachinePart("传送模块底座");
        p.ModelUrls.Add(PartTypeEnum.LinearGuide, "Press_module/Models/base_highlight");
       

        for (int i = 0; i < 4; i++)
        {
            s = new SubPart("HIWIN HGH15CA", i, PartTypeEnum.LinearGuide, CycleType.Replace, 24);
            s.VideoUrl = "Videos/ReplaceLinearGuide";
            p.SubParts.Add(s);
        }

        s = new SubPart("NSK AS2 GREASE", 0, PartTypeEnum.LinearGuide, CycleType.Luburicate, 6);
        s.VideoUrl = "Videos/LiburateLinearGuide";
        p.SubParts.Add(s);

        a2.PartList.Add(p);

        assemblies.Add(a2);



        MachineAssembly a1 = new MachineAssembly("热压模块");
        a1.AssemblyModelUrl = "Press_module/Models/thermal_press_module_normal";
        a1.AssemblyHilightUrl = "Press_module/Models/thermal_press_module_hightlight"; 

        //load parts
        p = new MachinePart("上压辊");
        p.ModelUrls.Add(PartTypeEnum.Bearing, "Press_module/Models/upper_roller");

        for (int i = 0; i < 2; i++)
        {
            s = new SubPart("SKF轴承 206EC", i, PartTypeEnum.Bearing, CycleType.Replace, 12);
            s.VideoUrl = "Videos/ReplaceBearing";
            p.SubParts.Add(s);
        }

        s = new SubPart("SKF重载润滑脂LGWA2/1", 0, PartTypeEnum.Bearing, CycleType.Luburicate, 6);
        s.VideoUrl = "Videos/LiburateBearing";
        p.SubParts.Add(s);
        a1.PartList.Add(p);


        p = new MachinePart("下压辊");
        p.ModelUrls.Add(PartTypeEnum.Bearing, "Press_module/Models/lower_roller");

        for (int i = 0; i < 2; i++)
        {
            s = new SubPart("SKF轴承 206EC", i, PartTypeEnum.Bearing, CycleType.Replace, 12);
            s.VideoUrl = "Videos/ReplaceBearing";
            p.SubParts.Add(s);
        }

        s = new SubPart("SKF重载润滑脂LGWA2/1", 0, PartTypeEnum.Bearing, CycleType.Luburicate, 6);
        s.VideoUrl = "Videos/LiburateBearing";
        p.SubParts.Add(s);
        a1.PartList.Add(p);

        assemblies.Add(a1);


        MachineAssembly a3 = new MachineAssembly("热压模块底座");
        a3.AssemblyModelUrl = "Press_module/Models/base_normal";
        a3.AssemblyHilightUrl = "Press_module/Models/base_highlight";

        p = new MachinePart("线性导轨组件");
        //p.ModelUrl = "Press_module/Models/base_highlight";
        p.ModelUrls.Add(PartTypeEnum.LinearGuide, "Press_module/Models/base_highlight");

        for (int i = 0; i < 4; i++)
        {
            s = new SubPart("HIWIN HGH35CA", i, PartTypeEnum.LinearGuide, CycleType.Replace, 24);
            s.VideoUrl = "Videos/ReplaceLinearGuide";
            p.SubParts.Add(s);            
        }

        s = new SubPart("NSK AS2 GREASE", 0, PartTypeEnum.LinearGuide, CycleType.Luburicate, 6);
        s.VideoUrl = "Videos/LiburateLinearGuide";
        p.SubParts.Add(s);

        a3.PartList.Add(p);

        assemblies.Add(a3);

        SetAssemblyControl();

    }

    private void SetAssemblyControl()
    {
        for (int i = 0; i < assControls.Count; i++)
        {
            AssemblyViewControl control = assControls[i];

            if (i < assemblies.Count)
                control.TheAss = assemblies[i];
        }
    }
}
