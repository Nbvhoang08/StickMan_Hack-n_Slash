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
    public float AttackRange;
    public Animator anim;
    public String currentAnimName;
    public float moveDistance = 2f; // Khoảng cách cố định 
    public   Player target;
    public Player Target => target;
    private Vector3 startPosition;
    public bool IsRight;
    private bool isChangingDirection = false; // Biến này để kiểm tra xem đã đổi hướng hay chưa
    public float hp;
    private bool IsDead => hp <= 0;
    public GameObject hitbox;
    
    protected virtual void Start()
    {
         rb = GetComponent<Rigidbody2D>();
        OnInit();

    }

    public void Update()
    {
        if (currState != null && !IsDead)
        {
            currState.onExcute(this);
        }
        
    }

    public virtual void OnInit()
    {
        hp = 10;
        hitbox.SetActive(false);
    }

    public virtual void DesSpawn()
    {

    }
    protected virtual void OnDeath()
    {
        Debug.Log("die");
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

    public virtual void StopMoving() 
    {
        rb.velocity = Vector2.zero;
      
    }
    public void ResetAttack()
    {
        hitbox.SetActive(false);
    }
   
    public virtual void Atk()
    {
      
       
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
        if(target != null && Vector2.Distance(target.transform.position, transform.position) <= AttackRange)
            return true;
        else
            return false;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("hitbox"))
        {
            OnHit(collision.GetComponent<hitbox>().damage);
            ChangeAnim("hurt");
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Màu sắc của Gizmo khi vẽ phạm vi tấn công
        Gizmos.color = Color.red;

        // Vẽ một vòng tròn để thể hiện phạm vi tấn công
        Gizmos.DrawWireSphere(transform.position, AttackRange);
    }



}
