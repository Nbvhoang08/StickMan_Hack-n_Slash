using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orc : Enemies
{
    // Start is called before the first frame update
    public bool isAttacking;
    public float gap;
    protected override void Start()
    {
        base.Start();
        changeState(new PatrolState());

    }


    public override void OnInit()
    {
        base.OnInit();
        isAttacking = false;
        
    }

    public override void DesSpawn()
    {
        base.DesSpawn();
    }

    protected override void OnDeath()
    {
        base.OnDeath();
        ChangeAnim("die");

    }
    public override void Atk()
    {
        base.Atk();
        isAttacking = true;
        ChangeAnim("atk");

    }

    public override void Cast()
    {
        base.Cast();
        skill();
        Debug.Log("skill");
     
    }
    public override void Moving()
    {
        base.Moving();
        if (!isAttacking)
        {
            ChangeAnim("run");
        }
       
    }
    public override void StopMoving()
    {
        base.StopMoving();
        ChangeAnim("idle");


    }
    private void skill()
    {
        Vector2 des = new Vector2(Target.transform.position.x - gap, Target.transform.position.y);
        transform.position = des;
        if (Vector2.Distance(Target.transform.position, des) < 0.1f)
        {
            ChangeAnim("atk1");
            Debug.Log("hehe");
        }
    }
    



}
