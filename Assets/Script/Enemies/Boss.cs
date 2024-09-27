using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Boss : Enemies
{
 
    public GameObject thunderPrefab;
    public bool isAttack;
    
    protected override void Start()
    {
        base.Start();
        changeState(new PatrolState());
        
    }
   
    
    public override void OnInit()
    {
        base.OnInit();
        isAttack = false;
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
        isAttack = true;
        if (isAttack)
        {
            ChangeAnim("atk");
            hitbox.SetActive(true);
            Invoke(nameof(ResetAttack), 0.5f);
          
        }
       
        
    }
    public override void ResetAttack()
    {
        base.ResetAttack();
        isAttack = false;
        ChangeAnim("idle");
    }


    public override void Cast()
    {
        base.Cast();
        ChangeAnim("cast");
        StartCoroutine(Spell());
    }
    public override void StopMoving()
    {
        base.StopMoving();
        if (!isAttack)
        {
            ChangeAnim("idle");
        }
        
    }
    public override void Moving()
    {
        base.Moving();
        ChangeAnim("run");

    }


    IEnumerator Spell()
    {

        StopMoving();
        yield return new WaitForSeconds(1f);

        List<GameObject> spawnedThunders = new List<GameObject>();

        for (int i = 0; i <= 3; i++)
        {
            GameObject thunderR = Instantiate(thunderPrefab, new Vector2(transform.position.x + i * 3, transform.position.y + 1), Quaternion.identity);
            GameObject thunderL = Instantiate(thunderPrefab, new Vector2(transform.position.x - i * 3, transform.position.y + 1), Quaternion.identity);

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

   


}
   



