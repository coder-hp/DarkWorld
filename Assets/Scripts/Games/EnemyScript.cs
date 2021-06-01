using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    public Transform trans_atkRange;

    Animator animator;
    CharacterController character;

    float atkRange = 3f;        // 攻击范围
    float chaseRange = 8f;      // 追击半径范围
    float walkSpeed = 3f;

    public Transform blood_trans;
    public BloodBar bloodBar; 
    
    int all_hp = 10;
    int cur_hp = 10;
    bool isAttacking = false;

    void Start()
    {
        EnemyManager.addEnemy(gameObject);
        animator = GetComponent<Animator>();
        character = GetComponent<CharacterController>();

        trans_atkRange.localScale = new Vector3(chaseRange / transform.localScale.x * 2, trans_atkRange.localScale.y, chaseRange / transform.localScale.x * 2);

        // 增加血条
        {
            GameObject pre = Resources.Load("Prefabs/Game/blood") as GameObject;
            blood_trans = Instantiate(pre, GameUIScript.s_instance.bloods_trans).transform;
            blood_trans.GetComponent<BloodBar>().init(true);
            bloodBar = blood_trans.GetComponent<BloodBar>();
        }
    }

    void Update()
    {
        if (blood_trans && Global.s_instance != null && Global.s_instance.camera_world != null)
        {
            blood_trans.transform.localPosition = CommonUtil.WorldPosToUI(Global.s_instance.camera_world, gameObject.transform.position) + new Vector2(0, 100);
        }

        GameObject enemy = CommonUtil.minDistance(gameObject, GameScript.s_instance.heroList);
        if (enemy != null && CommonUtil.TwoPointDistance3D(transform.position, enemy.transform.position) <= atkRange)
        {
            isAttacking = true;
            animator.Play("attack");
        }
        else
        {
            if (!isAttacking && enemy != null && CommonUtil.TwoPointDistance3D(transform.position, enemy.transform.position) <= chaseRange)
            {
                float angle = CommonUtil.TwoPointAngle(new Vector2(transform.position.x, transform.position.z), new Vector2(enemy.transform.position.x, enemy.transform.position.z));
                transform.rotation = Quaternion.Euler(0, angle, 0);
                animator.Play("run");
                character.Move(transform.forward * walkSpeed * Time.deltaTime);
            }
            else
            {
                if (!isAttacking)
                {
                    animator.Play("idle");
                }
            }
        }
    }

    public void hit(int damage)
    {
        cur_hp -= damage;
        if (cur_hp < 0)
        {
            cur_hp = 0;
        }
        bloodBar.setProgress((float)cur_hp / (float)all_hp);

        if(cur_hp <= 0)
        {
            destroy();
        }
    }

    public void onAtk()
    {
        GameObject enemy = CommonUtil.minDistance(gameObject, GameScript.s_instance.heroList);
        if (enemy != null && CommonUtil.TwoPointDistance3D(transform.position, enemy.transform.position) <= atkRange)
        {
            enemy.GetComponent<HeroScript>().hit(1);
        }
    }

    public void onAtkEnd()
    {
        GameObject enemy = CommonUtil.minDistance(gameObject, GameScript.s_instance.heroList);
        if (enemy == null || CommonUtil.TwoPointDistance3D(transform.position, enemy.transform.position) > atkRange)
        {
            animator.Play("wait");
        }

        isAttacking = false;
    }

    private void destroy()
    {
        if (blood_trans)
        {
            Destroy(blood_trans.gameObject);
        }

        EnemyManager.removeEnemy(gameObject);
        Destroy(gameObject);
    }
}
