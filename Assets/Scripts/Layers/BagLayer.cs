using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BagLayer : MonoBehaviour
{
    public Transform listContent;
    public GameObject demo_bag_item;
    public Text text_level;

    void Start()
    {
        switch(UserData.curOccupation)
        {
            case Occupation.Zhanshi:
                {
                    text_level.text = "等级:" + UserData.zhanshiLevel;
                    break;
                }

            case Occupation.FaShi:
                {
                    text_level.text = "等级:" + UserData.fashiLevel;
                    break;
                }

            case Occupation.HuanShi:
                {
                    text_level.text = "等级:" + UserData.huanshiLevel;
                    break;
                }
        }

        for(int i = 0; i < 79; i++)
        {
            GameObject obj = Instantiate(demo_bag_item,listContent);
        }
    }

    public void onClickBack()
    {
        Destroy(gameObject);
    }

    public void onClickEquip(int index)
    {
        switch(index)
        {
            // 武器
            case 1:
                {
                    ToastScript.show("武器");
                    break;
                }

            // 衣服
            case 2:
                {
                    ToastScript.show("衣服");
                    break;
                }

            // 裤子
            case 3:
                {
                    ToastScript.show("裤子");
                    break;
                }

            // 鞋子
            case 4:
                {
                    ToastScript.show("鞋子");
                    break;
                }

            // 项链
            case 5:
                {
                    ToastScript.show("项链");
                    break;
                }

            // 戒指
            case 6:
                {
                    ToastScript.show("戒指");
                    break;
                }
        }
    }

    public void onClickLookMore()
    {
        ToastScript.show("查看更多");
    }
}
