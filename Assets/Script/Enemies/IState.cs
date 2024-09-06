using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IState 
{
    void onEnter(Enemies enemies);

    void onExit(Enemies enemies);

    void onExcute(Enemies enemies);
   
}
