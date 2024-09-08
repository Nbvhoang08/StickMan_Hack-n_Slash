using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Animator anim;
    public String currentAnimName;
    private float hp;
    private bool IsDead => hp <= 0;

    private void Start()
    {
        OnInit();
    }

    public virtual void OnInit()
    {
        hp = 10;
    }

    public virtual void DesSpawn()
    {

    }
    protected virtual void OnDeath()
    {

    }
    protected virtual void SpecialSkill()
    {

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

    


    public void OnHit(float damage)
    {
        if (!IsDead)
        {
            hp -= damage;
            if(IsDead)
            {
                OnDeath();
            }
        }
    }
    
}
