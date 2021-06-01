using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalDataManager
{
    public static string account;
    public static string password;
    public static int beforeServer;

    public static void init()
    {
        account = PlayerPrefs.GetString("account","");
        password = PlayerPrefs.GetString("password", "");
        beforeServer = PlayerPrefs.GetInt("beforeServer", 1);
    }

    public static void setAccountData(string _account,string _password)
    {
        account = _account;
        password = _password;

        PlayerPrefs.SetString("account", account);
        PlayerPrefs.SetString("password", password);
    }

    public static void setBeforeServer(int id)
    {
        beforeServer = id;

        PlayerPrefs.SetInt("beforeServer", beforeServer);
    }
}
