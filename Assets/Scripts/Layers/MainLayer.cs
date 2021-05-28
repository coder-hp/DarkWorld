using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainLayer : MonoBehaviour
{
    public Text text_name;
    public Text text_level;

    void Awake()
    {
    }

    void Start()
    {
        NetEventManager.addEvent(NetEventName.MainLayer, onNetEvent);

        //C2S_Bag c2s = new C2S_Bag();
        //c2s.Tag = (int)CSParam.NetTag.Bag;
        //c2s.UserId = UserData.userId;
        //Socket_C.getInstance().Send(c2s);
    }

    void Update()
    {
        
    }

    public void onClickEnterGame(int mode)
    {
        if (mode == 1)
        {
            LayerManager.showLayer(LayerManager.Layer.GameUILayer);

            GameObject pre = Resources.Load("Prefabs/Game/GameWorld") as GameObject;
            GameObject gameLayer = Instantiate(pre);
            gameLayer.GetComponent<GameScript>().init(new List<int>() { UserData.userId});

            Destroy(gameObject);
        }
        else if (mode == 2)
        {
            LayerManager.showLayer(LayerManager.Layer.MatchLayer);

            C2S_EnterGameMode2 c2s = new C2S_EnterGameMode2();
            c2s.Tag = CSParam.NetTag.EnterGameMode2.ToString();
            c2s.UserId = UserData.userId;
            Socket_C.getInstance().Send(c2s);
        }
    }

    //----------------------------网络事件回调----------------------------------------------

    void onNetEvent(string data)
    {
        S2CBaseData s2cBaseData = JsonConvert.DeserializeObject<S2CBaseData>(data);

        if (s2cBaseData.Tag == CSParam.NetTag.Bag.ToString())
        {
            S2C_Bag s2c = JsonConvert.DeserializeObject<S2C_Bag>(data);
            if (s2c.Code == (int)CSParam.CodeType.Ok)
            {
                Debug.Log("背包数据:" + s2c.data);
            }
            else
            {
                ToastScript.show("获取背包错误：" + s2c.Code);
            }
        }
        else if (s2cBaseData.Tag == CSParam.NetTag.EnterGameMode2.ToString())
        {
            Debug.Log("收到EnterGameMode2回复");
        }
        else if (s2cBaseData.Tag == CSParam.NetTag.CanEnterGameMode2.ToString())
        {
            S2C_CanEnterGameMode2 s2c = JsonConvert.DeserializeObject<S2C_CanEnterGameMode2>(data);

            MatchLayer.s_instance.close();

            // 进入游戏
            {
                LayerManager.showLayer(LayerManager.Layer.GameUILayer);

                GameObject pre = Resources.Load("Prefabs/Game/GameWorld") as GameObject;
                GameObject gameLayer = Instantiate(pre);
                gameLayer.GetComponent<GameScript>().init(s2c.allUserId);

                Destroy(gameObject);
            }
        }
        else
        {
            Debug.Log("未知tag，不予处理：" + data);
        }
    }
}
