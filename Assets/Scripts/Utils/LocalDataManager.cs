using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalDataManager
{
    public static string account;
    public static string password;
    public static string beforeServer;

    public static void init()
    {
        account = PlayerPrefs.GetString("account","");
        password = PlayerPrefs.GetString("password", "");
        beforeServer = PlayerPrefs.GetString("beforeServer", "");
    }

    public static void setAccountData(string _account,string _password)
    {
        account = _account;
        password = _password;

        PlayerPrefs.SetString("account", account);
        PlayerPrefs.SetString("password", password);
    }

    public static void setBeforeServer(string ip)
    {
        beforeServer = ip;

        PlayerPrefs.SetString("beforeServer", beforeServer);
    }
}
