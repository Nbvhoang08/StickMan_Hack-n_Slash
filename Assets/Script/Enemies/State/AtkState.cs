using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class AtkState : IState
{

    private float timer;
    public void onEnter(Enemies enemies)
    {
        if (enemies.Target != null)
        {
            // doi huong theo player
            enemies.ChangedDirection(enemies.Target.transform.position.x > enemies.transform.position.x);
            if (enemies is Boss)
            {
                if(enemies.targetInRange())
                {
                    enemies.Atk(); // trong tam danh thi can chien  
                    enemies.StopMoving();
                   
                }
                
            }
            else if (enemies is Soldier)
            {
                // neu la soldier
                if(enemies.targetInRange())
                {
                    enemies.Atk(); // trong tam danh thi can chien   
                    
                }
                else
                {
                    enemies.Cast(); // ngoai tam thi ban cung
                }
            }else if(enemies is Orc)
            {
                // neu la soldier
                if (enemies.targetInRange())
                {
                   
                    enemies.Atk(); // trong tam danh thi can chien   
                }
                else
                {
                    enemies.Moving(); // ngoai tam thi ban cung
                }
            }else if(enemies is Orc)
            {
                  
            }



        }
        timer = 0;
    }

    public void onExcute(Enemies enemies)
    {
        timer += Time.deltaTime;
        if (enemies.Target != null)
        {
            if (enemies is Boss)
            {
                if (enemies.targetInRange())
                {
                    if(timer >= 1)
                    {
                        enemies.ChangedDirection(enemies.Target.transform.position.x > enemies.transform.position.x);
                        enemies.Atk(); // ngoai tam thi ban cung\
                        timer = 0f;
                    }
                }
                else
                {
                    if (timer >= 1)
                    {
                        enemies.Cast(); // ngoai tam thi ban cung\
                        timer = 0f;
                    }
                   
                }
            }
            else if (enemies is Soldier)
            {
                // neu la soldier
                if (enemies.targetInRange())
                {
                    if (timer >= 1f)
                    {
                        enemies.Atk(); // trong tam danh thi can chien
                        timer = 0f;
                    }
                }
                else
                {
                    if (timer >= 2f)
                    {
                        enemies.ChangedDirection(enemies.Target.transform.position.x > enemies.transform.position.x);
                        enemies.Cast(); // ngoai tam thi ban cung
                        timer = 0f;
                    }
                   
                }       
            }
            else if(enemies is Orc)
            {
                enemies.ChangedDirection(enemies.Target.transform.position.x > enemies.transform.position.x);
                if (enemies.targetInRange())
                {

                    enemies.Atk();
                }
                else
                {
                    if (timer >= 3)
                    {
                        enemies.Cast();
                        timer = 0;
                    }
                }
            }
        }
        else
        {
            if (timer >= 3f)
            {
                enemies.changeState(new PatrolState());

            }
            else
            {
                enemies.changeState(new IdleState());
            }
        }

        
    }

    public void onExit(Enemies enemies)
    {

    }

}
