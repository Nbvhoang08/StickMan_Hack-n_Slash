using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeCanvas : UICanvas
{
    // Start is called before the first frame update
    public void PlayBtn()
    {
        SenceController.Instance.ChangeScene("GamePlay");
        UIManager.Instance.CloseAll();
    }
    public void SettingBtn()
    {
        UIManager.Instance.QuitGame();
    }

    public void CreditBtn()
    {
        UIManager.Instance.CloseAll();
        UIManager.Instance.OpenUI<CreditCanvas>();
    }


}
