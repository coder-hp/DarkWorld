using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SkillBtn
{
    ShanXian,
    JingHua,
    Attack,
    Skill1,
    Skill2,
    Skill3,
}

public enum HeroAction
{
    Idle,
    Run,
}

public class HeroScript : MonoBehaviour
{
    Animator animator;
    CharacterController character;

    float walkSpeed = 6f;
    float atkRange = 3f;
    float shanxianDis = 4;

    public Transform blood_trans;
    public BloodBar bloodBar;

    int all_hp = 30;
    int cur_hp = 30;
    HeroAction heroAction = HeroAction.Idle;

    public int userId;
    public bool isSelf = false;
    Tween tween_move = null;
    Tween tween_rotate = null;

    private void Awake()
    {
    }

    void Start()
    {
        animator = GetComponent<Animator>();

        // 增加血条
        {
            GameObject pre = Resources.Load("Prefabs/Game/blood") as GameObject;
            blood_trans = Instantiate(pre, GameUIScript.s_instance.bloods_trans).transform;
            bloodBar = blood_trans.GetComponent<BloodBar>();

            if(!isSelf)
            {
                bloodBar.setIsTeam();
            }
        }

        //if (isSelf && (GameScript.s_instance.gameMode == GameMode.Double) && GameScript.s_instance.isStart)
        //{
        //    InvokeRepeating("getState", 1f / 30f, 1f / 30f);
        //}
    }

    public void init(int _userId)
    {
        userId = _userId;

        GameScript.s_instance.heroList.Add(gameObject);
        GameScript.s_instance.heroScriptList.Add(this);

        character = GetComponent<CharacterController>();

        if (UserData.userId == _userId)
        {
            isSelf = true;

            RockerScript.s_instance.onMove = onRockerMove;
            RockerScript.s_instance.onMoveEnd = onRockerMoveEnd;
            CameraTrack.s_instance.setTarget(transform);
            
            //if (GameScript.s_instance.gameMode == GameMode.Double)
            //{
            //    InvokeRepeating("submitState", 1f, 0.016f);
            //}
        }
        else
        {
            // 不是自己的话拿掉，否则不能用代码直接修改postion
            Destroy(GetComponent<CharacterController>());
        }
    }

    void submitState()
    {
        C2S_SubmitState c2s = new C2S_SubmitState();
        c2s.Tag = CSParam.NetTag.SubmitState.ToString();
        c2s.UserId = UserData.userId;
        c2s.action = (int)heroAction;
        c2s.pos_x = transform.position.x;
        c2s.pos_y = transform.position.y;
        c2s.pos_z = transform.position.z;
        c2s.rotate_y = transform.rotation.eulerAngles.y;
        Socket_C.getInstance().Send(c2s, false);
    }

    void getState()
    {
        //CommonUtil.jishi_start();
        C2S_GetUserState c2s = new C2S_GetUserState();
        c2s.Tag = CSParam.NetTag.GetUserState.ToString();
        for(int i = 0; i < GameScript.s_instance.heroScriptList.Count; i++)
        {
            if(userId != GameScript.s_instance.heroScriptList[i].userId)
            {
                c2s.userId_list.Add(GameScript.s_instance.heroScriptList[i].userId);
            }
        }
        Socket_C.getInstance().Send(c2s, false);
    }

    void Update()
    {
        if (blood_trans && Global.s_instance != null && Global.s_instance.camera_world != null)
        {
            blood_trans.transform.localPosition = CommonUtil.WorldPosToUI(Global.s_instance.camera_world, gameObject.transform.position) + new Vector2(0, 100);
        }
    }

    public void hit(int damage)
    {
        cur_hp -= damage;
        if(cur_hp < 0)
        {
            cur_hp = 0;
        }
        bloodBar.setProgress((float)cur_hp / (float)all_hp);
    }

    public void onAtk()
    {
        GameObject enemy = CommonUtil.minDistance(gameObject, EnemyManager.list);
        if (enemy != null && CommonUtil.TwoPointDistance3D(transform.position, enemy.transform.position) <= atkRange)
        {
            enemy.GetComponent<EnemyScript>().hit(2);
        }
    }

    public void onAtkEnd()
    {
        onRockerMoveEnd();
    }

    public void broadcastState(S2C_BroadcastState.BroadcastStateData s2c)
    {
        float juli = CommonUtil.TwoPointDistance3D(transform.position, new Vector3(s2c.pos_x, s2c.pos_y, s2c.pos_z));

        //transform.rotation = Quaternion.Euler(0, s2c.rotate_y, 0);

        if(tween_move != null)
        {
            tween_move.Pause();
        }
        if (tween_rotate != null)
        {
            tween_rotate.Pause();
        }
        float time = juli * 0.2f;
        if(time < 0.1f)
        {
            time = 0.1f;
        }
        else if (time > 0.3f)
        {
            time = 0.3f;
        }

        tween_rotate = transform.DORotate(new Vector3(0, s2c.rotate_y, 0), time).SetEase(Ease.Linear).OnComplete(() =>
        //tween_rotate = transform.DORotate(new Vector3(0, s2c.rotate_y, 0), 0.1f).SetEase(Ease.Linear).OnComplete(() =>
        {
            tween_rotate.Pause();
        });

        tween_move = transform.DOMove(new Vector3(s2c.pos_x, s2c.pos_y, s2c.pos_z), time).SetEase(Ease.Linear).OnComplete(() =>
        //tween_move = transform.DOMove(new Vector3(s2c.pos_x, s2c.pos_y, s2c.pos_z), 0.1f).SetEase(Ease.Linear).OnComplete(() =>
        {
            tween_move.Pause();
            if (s2c.action == (int)HeroAction.Idle)
            {
                checkAtk();
            }
        });

        animator.Play("run");

        //switch (s2c.action)
        //{
        //    case (int)HeroAction.Idle:
        //        {
        //            checkAtk();
        //            break;
        //        }

        //    case (int)HeroAction.Run:
        //        {
        //            animator.Play("run");
        //            break;
        //        }
        //}

        //switch (s2c.action)
        //{
        //    case (int)HeroAction.Idle:
        //        {
        //            character.enabled = false;
        //            transform.position = new Vector3(s2c.pos_x, s2c.pos_y, s2c.pos_z);
        //            character.enabled = true;
        //            idle();
        //            break;
        //        }

        //    case (int)HeroAction.Run:
        //        {
        //            run(s2c.rotate_y);
        //            break;
        //        }
        //}
    }

    void idle()
    {
        checkAtk();

        heroAction = HeroAction.Idle;
    }

    void run(float angle)
    {
        transform.rotation = Quaternion.Euler(0, angle, 0);
        animator.Play("run");
        character.Move(transform.forward * walkSpeed * Time.deltaTime);

        heroAction = HeroAction.Run;
    }

    void onRockerMove(float angle)
    {
        if (isSelf)
        {
            run(angle);

            if (GameScript.s_instance.gameMode == GameMode.Double)
            {
                submitState();
            }
        }
    }

    void onRockerMoveEnd()
    {
        if (isSelf)
        {
            idle();

            if (GameScript.s_instance.gameMode == GameMode.Double)
            {
                submitState();
            }
        }
    }

    void checkAtk()
    {
        GameObject enemy = CommonUtil.minDistance(gameObject, EnemyManager.list);
        if (enemy != null && CommonUtil.TwoPointDistance3D(transform.position, enemy.transform.position) <= atkRange)
        {
            animator.Play("attack" + RandomUtil.getRandom(1, 2), 0, 0);
        }
        else
        {
            animator.Play("idle");
        }
    }
}
