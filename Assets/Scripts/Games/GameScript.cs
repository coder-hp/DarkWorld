using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameMode
{
    Single,
    Double,
}

public class GameScript : MonoBehaviour
{
    public static GameScript s_instance = null;
    public Camera camera_world;

    public List<GameObject> heroList = new List<GameObject>();
    public List<HeroScript> heroScriptList = new List<HeroScript>();

    public GameMode gameMode = GameMode.Single;

    public bool isStart = false;

    long ping = 0;

    private void Awake()
    {
        s_instance = this;
        Global.s_instance.camera_world = camera_world;
        NetEventManager.addEvent(NetEventName.GameLayer, onNetEvent);
    }

    void Start()
    {
        C2S_UserReady c2s = new C2S_UserReady();
        c2s.Tag = CSParam.NetTag.UserReady.ToString();
        c2s.UserId = UserData.userId;
        Socket_C.getInstance().Send(c2s, false);

        InvokeRepeating("refreshPing", 0.2f, 0.2f);
    }

    public void init(List<int> userList)
    {
        if(userList.Count > 1)
        {
            gameMode = GameMode.Double;

            GameUIScript.s_instance.setTouchBlockIsShow(true);
        }
        for(int i = 0; i < userList.Count; i++)
        {
            GameObject prefab = Resources.Load("Prefabs/Game/Hero") as GameObject;
            GameObject hero = Instantiate(prefab, transform);
            hero.GetComponent<HeroScript>().init(userList[i]);

            isStart = true;
        }
    }

    void refreshPing()
    {
        Global.s_instance.Text_ping.text = "ping:" + ping;
    }

    HeroScript getHeroById(int userId)
    {
        for(int i = 0; i < heroScriptList.Count; i++)
        {
            if(heroScriptList[i].userId == userId)
            {
                return heroScriptList[i];
            }
        }

        return null;
    }

    //----------------------------网络事件回调----------------------------------------------

    long beforeBroadcastTime = 0;
    void onNetEvent(string data)
    {
        S2CBaseData s2cBaseData = JsonConvert.DeserializeObject<S2CBaseData>(data);

        if (s2cBaseData.Tag == CSParam.NetTag.BroadcastState.ToString())
        {
            if (beforeBroadcastTime != 0)
            {
                ping = CommonUtil.jishi_end();
                beforeBroadcastTime = CommonUtil.jishi_start();
            }
            else
            {
                beforeBroadcastTime = CommonUtil.jishi_start();
            }
            S2C_BroadcastState s2c = JsonConvert.DeserializeObject<S2C_BroadcastState>(data);

            for (int i = 0; i < s2c.list.Count; i++)
            {
                HeroScript heroScript = getHeroById(s2c.list[i].UserId);
                if (heroScript != null && heroScript.userId != UserData.userId)
                {
                    heroScript.broadcastState(s2c.list[i]);
                }
            }
        }
        else if (s2cBaseData.Tag == CSParam.NetTag.GameMode2Start.ToString())
        {
            GameUIScript.s_instance.setTouchBlockIsShow(false);
            isStart = true;
        }
        else if (s2cBaseData.Tag == CSParam.NetTag.GetUserState.ToString())
        {
            S2C_BroadcastState s2c = JsonConvert.DeserializeObject<S2C_BroadcastState>(data);

            for (int i = 0; i < s2c.list.Count; i++)
            {
                HeroScript heroScript = getHeroById(s2c.list[i].UserId);
                if (heroScript)
                {
                    heroScript.broadcastState(s2c.list[i]);
                }
            }
        }
        else
        {
            Debug.Log("未知tag，不予处理：" + data);
        }
    }
}
