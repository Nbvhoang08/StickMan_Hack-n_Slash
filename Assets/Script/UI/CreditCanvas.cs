using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditCanvas : UICanvas
{
    // Start is called before the first frame update
    public void BackBtn() 
    {
        UIManager.Instance.CloseAll();
        UIManager.Instance.OpenUI<HomeCanvas>();          
    }



}
