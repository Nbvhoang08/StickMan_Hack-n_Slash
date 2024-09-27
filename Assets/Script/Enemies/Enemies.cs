using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
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
    public bool hurt;

    private bool IsDead => hp <= 0;
    public GameObject hitbox;
    [SerializeField] private bool isFrozen;
    public CinemachineImpulseSource _imPulse; 
    protected virtual void Start()
    {
         rb = GetComponent<Rigidbody2D>();
        OnInit();

    }

    public void Update()
    {
        if (currState != null && !IsDead && !isFrozen)
        {
            currState.onExcute(this);
        }
       if(IsDead)
        {
            OnDeath();
        }
        
    }

    public virtual void OnInit()
    {
        hp = 10;
        hitbox.SetActive(false);
        hurt = false;
        isFrozen = false;
        _imPulse = GetComponent<CinemachineImpulseSource>();
    }

    public virtual void DesSpawn()
    {

        Destroy(this.gameObject);
    }
    protected virtual void OnDeath()
    {
        changeState(null);
        ChangeAnim("die");
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
            Hurt();
            hp -= damage;
            if (IsDead)
            {
                OnDeath();
            }
        }
        else
        {
            ChangeAnim("die");
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
    public virtual void ResetAttack()
    {
        hitbox.SetActive(false);
    }
   
    public virtual void Atk()
    {
      
       
    }
   
    public virtual void Cast()
    {
       
    }
    public virtual void Hurt()
    {
        hurt = true;
        ChangeAnim("hurt");

    }
    public void Freeze()
    {
        // Dừng Animator (đóng băng animation hiện tại)
        anim.speed = 0;
        isFrozen = true;
        StartCoroutine(unFreeze());
    }

    public void Unfreeze()
    {
        // Tiếp tục Animator
        anim.speed = 1;
        isFrozen = false;
    }
    IEnumerator unFreeze() 
    {
        yield return new WaitForSeconds(2f);
        Unfreeze();
        yield return new WaitForSeconds(0.5f);
        ResetState();
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
            
        }if (collision.CompareTag("playerBullet"))
        {
            Freeze();
            
        }
    }
    public void ResetState()
    {
/*        changeState(new IdleState());*/
        hurt = false;
       
    }

    private void OnDrawGizmosSelected()
    {
        // Màu sắc của Gizmo khi vẽ phạm vi tấn công
        Gizmos.color = Color.red;

        // Vẽ một vòng tròn để thể hiện phạm vi tấn công
        Gizmos.DrawWireSphere(transform.position, AttackRange);
    }

   
}
