using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor;
using UnityEditor.Tilemaps;
using UnityEngine;

public class Enemies : MonoBehaviour
{

    [SerializeField] private IState currState;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float ActackRange;
    public Animator anim;
    public String currentAnimName;
    private Player target;
    public bool IsRight;
    public Player Target => target;
    public GameObject thunderPrefab;
    public float moveDistance = 2f; // Khoảng cách cố định
    private Vector3 startPosition;
    private bool isChangingDirection = false; // Biến này để kiểm tra xem đã đổi hướng hay chưa
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        changeState(new IdleState());
    }

    public void Update()
    {
        if (currState != null)
        {
            currState.onExcute(this);
        }
    }
    public  void OnInit()
    {

    }

    public void DesSpawn()
    {

    }

    public void OnDeath()
    {
        ChangeAnim("die");
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

    public void Moving() 
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

   
    public void Atk()
    {
        
        ChangeAnim("atk");
       
    }
   

    IEnumerator Spell()
    {

        StopMoving();
        yield return new WaitForSeconds(1f);

        List<GameObject> spawnedThunders = new List<GameObject>();

        for (int i = 0; i <= 3; i++)
        {
            GameObject thunderR = Instantiate(thunderPrefab, new Vector2(transform.position.x + i * 3, transform.position.y+1), Quaternion.identity);
            GameObject thunderL = Instantiate(thunderPrefab, new Vector2(transform.position.x - i * 3, transform.position.y+1), Quaternion.identity);

            // Thêm vào danh sách để quản lý và hủy sau này
            spawnedThunders.Add(thunderR);
            spawnedThunders.Add(thunderL);
        }

        yield return new WaitForSeconds(1f);

        // Hủy các object thunder sau 1 giây
        foreach (GameObject thunder in spawnedThunders)
        {
            Destroy(thunder);
        }

        ChangeAnim("done");
    }


    public void Cast()
    {
        ChangeAnim("cast");
        StartCoroutine(Spell());    
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
    public void ChangeAnim(string animName)
    {
        if (currentAnimName != animName)
        {
            anim.ResetTrigger(animName);
            currentAnimName = animName;
            anim.SetTrigger(animName);

        }
    }



}
