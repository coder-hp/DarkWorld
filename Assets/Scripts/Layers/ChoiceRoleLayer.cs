using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChoiceRoleLayer : MonoBehaviour
{
    public Text zhanshiLevel;
    public Text fashiLevel;
    public Text huanshiLevel;

    float firstClickTime = 0;
    int firstClickRoleIndex = 0;

    void Start()
    {
        zhanshiLevel.text = "等级:" + UserData.zhanshiLevel;
        fashiLevel.text = "等级:" + UserData.fashiLevel;
        huanshiLevel.text = "等级:" + UserData.huanshiLevel;
    }

    public void onClickRole(int index)
    {
        if(firstClickTime == 0)
        {
            firstClickTime = CommonUtil.getTimeStamp_Millisecond();
            firstClickRoleIndex = index;
        }
        else
        {
            if(firstClickRoleIndex == index)
            {
                if((CommonUtil.getTimeStamp_Millisecond() - firstClickTime) < 300)
                {
                    choiceRole(index);
                }
            }
            else
            {
                firstClickTime = CommonUtil.getTimeStamp_Millisecond();
                firstClickRoleIndex = index;
            }
        }
    }

    void choiceRole(int index)
    {
        Destroy(gameObject);
        GameObject layer = LayerManager.showLayer(LayerManager.Layer.MainLayer);
    }
}
