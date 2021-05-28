using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

class SkillState
{
    public SkillBtn skillBtn;
    public float cd;
    public float rest_cd = 0;
    public Image img_cd;

    public SkillState(SkillBtn _skillBtn, float _cd, Image _img_cd)
    {
        skillBtn = _skillBtn;
        cd = _cd;
        img_cd = _img_cd;
    }
}

public class GameUIScript : MonoBehaviour
{
    public static GameUIScript s_instance = null;

    public GameObject touchBlock;
    public Transform bloods_trans;
    List<SkillState> skillState_list = new List<SkillState>();

    private void Awake()
    {
        s_instance = this;
    }

    void Start()
    {
        skillState_list.Add(new SkillState(SkillBtn.ShanXian,5,transform.Find("mask_shanxian/img_cd").GetComponent<Image>()));
        skillState_list.Add(new SkillState(SkillBtn.JingHua, 5, transform.Find("mask_jinghua/img_cd").GetComponent<Image>()));
        skillState_list.Add(new SkillState(SkillBtn.Skill1, 3, transform.Find("mask_skill1/img_cd").GetComponent<Image>()));
        skillState_list.Add(new SkillState(SkillBtn.Skill2, 4, transform.Find("mask_skill2/img_cd").GetComponent<Image>()));
        skillState_list.Add(new SkillState(SkillBtn.Skill3, 5, transform.Find("mask_skill3/img_cd").GetComponent<Image>()));
    }
    
    void Update()
    {
        float time = Time.deltaTime;
        for(int i = 0; i < skillState_list.Count; i++)
        {
            if(skillState_list[i].rest_cd > 0)
            {
                skillState_list[i].rest_cd -= time;
                if(skillState_list[i].rest_cd <= 0)
                {
                    skillState_list[i].rest_cd = 0;
                    skillState_list[i].img_cd.transform.localScale = new Vector3(0,0,0);
                }
                skillState_list[i].img_cd.fillAmount = skillState_list[i].rest_cd / skillState_list[i].cd;
            }
        }
    }

    SkillState getSkillState(SkillBtn skill)
    {
        for(int i = 0; i < skillState_list.Count; i++)
        {
            if(skillState_list[i].skillBtn == skill)
            {
                return skillState_list[i];
            }
        }

        return null;
    }

    public void setTouchBlockIsShow(bool b)
    {
        touchBlock.SetActive(b);
    }

    public void onClickBack()
    {
        Destroy(gameObject);
        Destroy(GameScript.s_instance.gameObject);
        EnemyManager.list.Clear();
        LayerManager.showLayer(LayerManager.Layer.MainLayer);

        C2S_LeaveRoom c2s = new C2S_LeaveRoom();
        c2s.Tag = CSParam.NetTag.LeaveRoom.ToString();
        c2s.UserId = UserData.userId;
        Socket_C.getInstance().Send(c2s, false);
    }

    public void onClickJingHua()
    {
        //SkillState skillState = getSkillState(SkillBtn.JingHua);
        //if (skillState.rest_cd == 0)
        //{
        //    skillState.img_cd.transform.localScale = new Vector3(1, 1, 1);
        //    skillState.rest_cd = skillState.cd;
        //    HeroScript.s_instance.useSkill(SkillBtn.JingHua);
        //}
    }

    public void onClickShanXian()
    {
        //SkillState skillState = getSkillState(SkillBtn.ShanXian);
        //if (skillState.rest_cd == 0)
        //{
        //    skillState.img_cd.transform.localScale = new Vector3(1, 1, 1);
        //    skillState.rest_cd = skillState.cd;
        //    HeroScript.s_instance.useSkill(SkillBtn.ShanXian);
        //}
    }

    public void onClickAttack()
    {
        //HeroScript.s_instance.useSkill(SkillBtn.Attack);
    }

    public void onClickSkill(int index)
    {
        //SkillState skillState = getSkillState((SkillBtn)index);
        //if (skillState.rest_cd == 0)
        //{
        //    skillState.img_cd.transform.localScale = new Vector3(1, 1, 1);
        //    skillState.rest_cd = skillState.cd;
        //    HeroScript.s_instance.useSkill((SkillBtn)index);
        //}
    }

    //public void useAllSkill()
    //{
    //    for(int i = (int)SkillBtn.Skill1; i <= (int)SkillBtn.Skill4; i++ )
    //    {
    //        SkillState skillState = getSkillState((SkillBtn)i);
    //        if (skillState.rest_cd == 0)
    //        {
    //            skillState.img_cd.transform.localScale = new Vector3(1, 1, 1);
    //            skillState.rest_cd = skillState.cd;
    //            HeroScript.s_instance.useSkill((SkillBtn)i);
    //        }
    //    }
    //}
}
