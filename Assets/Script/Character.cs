using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Animator anim;
    public String currentAnimName;
   
    public delegate void OnHealthChange();
    [HideInInspector] public OnHealthChange onHealthChangeCallBack;
    public int maxHp=4;
    public bool IsDead => hp <= 0;

    public int hp;
    public virtual void Awake()
    {
        OnInit();
        
    }

    protected virtual void Start()
    {
        
    }

    public virtual void OnInit()
    {
        hp = maxHp;
        if (onHealthChangeCallBack != null)
        {
            onHealthChangeCallBack.Invoke();
        }
        anim = GetComponent<Animator>();    
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

    


    public void OnHit(int damage)
    {
        if (!IsDead)
        {
            hp -= damage;
            if (onHealthChangeCallBack != null)
            {
                onHealthChangeCallBack.Invoke();
            }
            if (IsDead)
            {
                OnDeath();
            }
        }
    }
    
}
