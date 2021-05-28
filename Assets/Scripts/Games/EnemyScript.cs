using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    Animator animator;

    float atkRange = 3f;

    public Transform blood_trans;
    public BloodBar bloodBar; 
    
    int all_hp = 10;
    int cur_hp = 10;

    void Start()
    {
        EnemyManager.addEnemy(gameObject);
        animator = GetComponent<Animator>();

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
            animator.Play("attack");
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
            Destroy(gameObject);
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
    }

    private void OnDestroy()
    {
        if (blood_trans)
        {
            Destroy(blood_trans.gameObject);
        }
        EnemyManager.removeEnemy(gameObject);
    }
}
