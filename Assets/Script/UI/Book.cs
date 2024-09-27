using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Book : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Animator anim;
    public String currentAnimName;
    public List<GameObject> character;
    [SerializeField]private int _index;
    private void OnEnable()
    {
        ChangeAnim("default");
        ShowCharacter();
        _index = 0;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       if(_index < 0)
        {
            _index = character.Count -1;
        }else if (_index > character.Count-1)
        {
            _index = 0;
        } 
        
    }
    public void BtnPrev()
    {
        ChangeAnim("prev");
        _index++;
     
    }
    public void BtnBack()
    {
        ChangeAnim("back");
        _index--;   
    }
    void SetDefault()
    {
        ChangeAnim("default");
        ShowCharacter();
    }

    void ShowCharacter()
    {
        for (int i = 0; i < character.Count; i++)
        {
            if(i!= _index)
            {
                character[i].SetActive(false);
            }
            else
            {
                character[i].SetActive(true);
            }
        }
    }
    void CloseAll()
    {
        for (int i = 0; i < character.Count; i++)
        {
            character[i].SetActive(false);
           
        }
    }

    protected void ChangeAnim(string animName)
    {
        if (currentAnimName != animName)
        {
            anim.ResetTrigger(animName);
            currentAnimName = animName;
            anim.SetTrigger(animName);
        }
    }
}
