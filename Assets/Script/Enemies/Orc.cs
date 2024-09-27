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
        Destroy(this.gameObject);
    }

    protected override void OnDeath()
    {
        base.OnDeath();



    }
    public override void Atk()
    {
        base.Atk();
        isAttacking = true;
        if (!hurt)
        {
            ChangeAnim("atk");
            if (isAttacking)
            {
                ChangeAnim("atk");
                hitbox.SetActive(true);
                Invoke(nameof(ResetAttack), 0.8f);

            }
        }
        


    }

    public override void Cast()
    {
        base.Cast();
        skill();

     
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
         
        }
    }
    public override void ResetAttack()
    {
        base.ResetAttack();
        isAttacking = false;
        ChangeAnim("idle");

    }




}
