using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NetEventName
{
    LoginLayer,
    MainLayer,
    GameLayer,
}

class NetEventData
{
    public NetEventName netEventName;
    public Action<string> callback = null;

    public NetEventData(NetEventName _netEventName, Action<string> _callback)
    {
        netEventName = _netEventName;
        callback = _callback;
    }
}

public class NetEventManager
{
    static List<NetEventData> eventList = new List<NetEventData>();

    public static void addEvent(NetEventName _netEventName, Action<string> _callback)
    {
        for(int i = 0; i < eventList.Count; i++)
        {
            if(_netEventName == eventList[i].netEventName)
            {
                eventList[i].callback = _callback;
                return;
            }
        }

        eventList.Add(new NetEventData(_netEventName, _callback));
    }

    public static void sendEvent(NetEventName _netEventName,string data)
    {
        for (int i = 0; i < eventList.Count; i++)
        {
            if (_netEventName == eventList[i].netEventName)
            {
                eventList[i].callback(data);
                return;
            }
        }
    }
}
