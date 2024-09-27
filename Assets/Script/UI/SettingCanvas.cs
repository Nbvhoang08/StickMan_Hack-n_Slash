using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingCanvas : UICanvas
{
    // Start is called before the first frame update
    public void BackBtn()
    {
        UIManager.Instance.OpenUI<HomeCanvas>();
        this.gameObject.SetActive(false);
    }
}
