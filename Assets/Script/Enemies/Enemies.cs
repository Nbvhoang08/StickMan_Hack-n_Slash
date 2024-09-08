using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor;
using UnityEditor.Tilemaps;
using UnityEngine;

public class Enemies : MonoBehaviour
{

    public IState currState;
    public Rigidbody2D rb;
    public  float moveSpeed;
    public float ActackRange;
    public Animator anim;
    public String currentAnimName;
    public float moveDistance = 2f; // Khoảng cách cố định 
    private Player target;
    public Player Target => target;
    private Vector3 startPosition;
    public bool IsRight;
    private bool isChangingDirection = false; // Biến này để kiểm tra xem đã đổi hướng hay chưa
    private float hp;
    private bool IsDead => hp <= 0;
    
    
    protected virtual void Start()
    {
         rb = GetComponent<Rigidbody2D>();
        OnInit();

    }

    public void Update()
    {
        if (currState != null)
        {
            currState.onExcute(this);
        }
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
            if (IsDead)
            {
                OnDeath();
            }
        }
    }


    public void changeState(IState newState)
    {
        if (currState != null) 
        {
            currState.onExit(this);
        }
        currState = newState;
        if (currState != null)
        {
            currState.onEnter(this);
        }
    }

    internal void SetTarget(Player player)
    {
        this.target = player;
        if (targetInRange())
        {
            changeState(new AtkState());
        }
        else
        if(Target != null)
        {
            changeState(new PatrolState());
        }
        else
        {
            changeState(new IdleState());
        }
    }

    public virtual void Moving() 
    {
        ChangeAnim("run");
        rb.velocity = transform.right * moveSpeed;
        float distanceTraveled = Vector3.Distance(startPosition, transform.position);

        // Nếu đã di chuyển hết khoảng cách cố định, đổi hướng và đặt lại vị trí bắt đầu
        if (Target == null && !isChangingDirection)
        {
            // Bắt đầu Coroutine đổi hướng
            StartCoroutine(ChangedDirection());
        }
        
    }
    IEnumerator ChangedDirection()
    {
        // Đặt cờ để tránh nhiều lần gọi Coroutine cùng lúc
        isChangingDirection = true;

        // Đợi 5 giây
        yield return new WaitForSeconds(2f);

        // Đổi hướng
        IsRight = !IsRight;
        ChangedDirection(IsRight);

        // Đặt lại cờ để cho phép đổi hướng lần sau
        isChangingDirection = false;

    }

    public void StopMoving() 
    {
        ChangeAnim("idle");
        rb.velocity = Vector2.zero;
      
    }

   
    public virtual void Atk()
    {
        ChangeAnim("atk");
       
    }
   
    public virtual void Cast()
    {
       
    }

    public void ChangedDirection(bool IsRight)
    {
        this.IsRight = IsRight;
        transform.rotation = IsRight ? Quaternion.Euler(Vector3.zero) : Quaternion.Euler(Vector3.up*180);
    }



    public bool targetInRange()
    {
        if(target != null && Vector2.Distance(target.transform.position, transform.position) <= ActackRange)
            return true;
        else
            return false;
    }
    



}
