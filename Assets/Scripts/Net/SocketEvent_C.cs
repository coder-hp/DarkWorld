using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class SocketEvent_C
{
    public static void OnReceive(string data)
    {
        try
        {
            if (Socket_C.getInstance().isShowLog)
            {
                Debug.Log("收到服务端消息：" + data);
            }
            if (DebugLayer.s_instance)
            {
                DebugLayer.s_instance.addBackCount();
            }

            NetLoading.s_instance.close();
            try
            {
                S2CBaseData s2cBaseData = JsonConvert.DeserializeObject<S2CBaseData>(data);

                if (s2cBaseData.Tag == CSParam.NetTag.Login.ToString())
                {
                    NetEventManager.sendEvent(NetEventName.LoginLayer, data);
                }
                else if (s2cBaseData.Tag == CSParam.NetTag.Register.ToString())
                {
                    NetEventManager.sendEvent(NetEventName.LoginLayer, data);
                }
                else if (s2cBaseData.Tag == CSParam.NetTag.Bag.ToString())
                {
                    NetEventManager.sendEvent(NetEventName.MainLayer, data);
                }
                else if (s2cBaseData.Tag == CSParam.NetTag.EnterGameMode2.ToString())
                {
                    NetEventManager.sendEvent(NetEventName.MainLayer, data);
                }
                else if (s2cBaseData.Tag == CSParam.NetTag.CanEnterGameMode2.ToString())
                {
                    NetEventManager.sendEvent(NetEventName.MainLayer, data);
                }
                else if (s2cBaseData.Tag == CSParam.NetTag.GameMode2Start.ToString())
                {
                    NetEventManager.sendEvent(NetEventName.GameLayer, data);
                }
                else if (s2cBaseData.Tag == CSParam.NetTag.BroadcastState.ToString())
                {
                    NetEventManager.sendEvent(NetEventName.GameLayer, data);
                }
                else if (s2cBaseData.Tag == CSParam.NetTag.GetUserState.ToString())
                {
                    NetEventManager.sendEvent(NetEventName.GameLayer, data);
                }
                else
                {
                    Debug.Log("未知tag，不予处理：" + data);
                }
            }
            catch (Exception ex)
            {
                Debug.Log("服务端传的数据异常：" + ex + "内容：" + data);
            }
        }
        catch (Exception ex)
        {
            Debug.Log("OnReceive:" + ex);
        }
    }

    public static void OnConnect(bool result)
    {
        if (result)
        {
            ToastScript.show("连接服务器成功");
        }
        else
        {
            ToastScript.show("连接服务器失败");
        }
    }

    public static void OnClose()
    {
        if(LoginLayer.s_instance)
        {
            LoginLayer.s_instance.connectServer();
        }
    }
}