using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerConfigData
{
    public int state;
    public string name;
    public string url;
    public string ip;
    public int port;
}

public class ServerConfigEntity
{
    public static ServerConfigEntity s_instance = null;

    public List<ServerConfigData> data_list = new List<ServerConfigData>();

    public static ServerConfigEntity getInstance()
    {
        if(s_instance == null)
        {
            s_instance = new ServerConfigEntity();
        }

        return s_instance;
    }
}
