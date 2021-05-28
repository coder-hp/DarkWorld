using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class LoginLayer : MonoBehaviour
{
    public static LoginLayer s_instance = null;

    public GameObject loginPanel;
    public GameObject registerPanel;

    public InputField text_login_account;
    public InputField text_login_password;
    public InputField text_register_account;
    public InputField text_register_password;
    public InputField text_register_password2;

    public GameObject serverList;
    public Transform serverListContent;
    public Transform curServer;
    public GameObject demo_serverItem;

    SpriteAtlas atlas_login;

    ServerConfigData curServerConfigData;
    bool isTryConnectServer = false;

    void Start()
    {
        Application.targetFrameRate = 60;

        s_instance = this;
        atlas_login = AtlasUtil.getAtlas_login();

        NetEventManager.addEvent(NetEventName.LoginLayer, onNetEvent);

        // 自动填充上一次登录的账号密码
        {
            text_login_account.text = LocalDataManager.account;
            text_login_password.text = LocalDataManager.password;
        }

        // socket事件回调
        {
            Socket_C.getInstance().m_onSocketEvent_Receive = SocketEvent_C.OnReceive;
            Socket_C.getInstance().m_onSocketEvent_Connect = SocketEvent_C.OnConnect;
            Socket_C.getInstance().m_onSocketEvent_Close = SocketEvent_C.OnClose;
        }
        
        HttpReqUtil.getInstance().Get("http://hanxinyi.cn/ServerConfig.json", (string data) =>
         {
             ServerConfigEntity.getInstance().data_list = JsonConvert.DeserializeObject<List<ServerConfigData>>(data);
             for (int i = 0; i < ServerConfigEntity.getInstance().data_list.Count; i++)
             {
                 ServerConfigData serverConfigData = ServerConfigEntity.getInstance().data_list[i];
                 GameObject item = Instantiate(demo_serverItem, serverListContent);
                 item.transform.Find("img_state").GetComponent<Image>().sprite = atlas_login.GetSprite("state_" + serverConfigData.state);
                 item.transform.Find("text_name").GetComponent<Text>().text = serverConfigData.name;

                 item.GetComponent<Button>().onClick.AddListener(()=>
                 {
                     serverList.SetActive(false);
                     setCurServer(serverConfigData);
                 });
             }

             int serverIndex = 0;
             for(int i = 0; i < ServerConfigEntity.getInstance().data_list.Count; i++)
             {
                 if(ServerConfigEntity.getInstance().data_list[i].ip == LocalDataManager.beforeServer)
                 {
                     serverIndex = i;
                     break;
                 }
             }
             setCurServer(ServerConfigEntity.getInstance().data_list[serverIndex]);
         });
    }
    
    private void Update()
    {
    }

    void setCurServer(ServerConfigData serverConfigData)
    {
        curServerConfigData = serverConfigData;

        curServer.gameObject.SetActive(true);
        curServer.transform.Find("img_state").GetComponent<Image>().sprite = atlas_login.GetSprite("state_" + serverConfigData.state);
        curServer.transform.Find("text_name").GetComponent<Text>().text = serverConfigData.name;

        if (serverConfigData.state == 1)
        {
            curServer.transform.Find("text_state").GetComponent<Text>().text = "流畅";
            curServer.transform.Find("text_state").GetComponent<Text>().color = new Color(106, 184, 44);
        }
        else if (serverConfigData.state == 2)
        {
            curServer.transform.Find("text_state").GetComponent<Text>().text = "爆满";
            curServer.transform.Find("text_state").GetComponent<Text>().color = new Color(231, 32, 25);
        }
        else if (serverConfigData.state == 3)
        {
            curServer.transform.Find("text_state").GetComponent<Text>().text = "维护";
            curServer.transform.Find("text_state").GetComponent<Text>().color = new Color(130, 129, 129);
        }
        
        if(!Socket_C.getInstance().Start(serverConfigData.ip, serverConfigData.url, serverConfigData.port))
        {
            isTryConnectServer = true;
            Socket_C.getInstance().Stop();
        }
    }

    public void connectServer()
    {
        if (isTryConnectServer)
        {
            Socket_C.getInstance().Start(curServerConfigData.ip, curServerConfigData.url, curServerConfigData.port);
            isTryConnectServer = false;
        }
    }

    public void onClickChangeServer()
    {
        serverList.SetActive(true);
    }

    public void onClickOffline()
    {
        loginSuccess();
    }

    public void onClickLogin()
    {
        if (text_login_account.text == "")
        {
            ToastScript.show("请输入账号");
            return;
        }

        if (text_login_password.text == "")
        {
            ToastScript.show("请输入密码");
            return;
        }

        C2S_Login c2s = new C2S_Login();
        c2s.Tag = CSParam.NetTag.Login.ToString();
        c2s.Account = text_login_account.text;
        c2s.Password = text_login_password.text;
        Socket_C.getInstance().Send(c2s);
    }

    // 打开注册界面
    public void onClickToRegister()
    {
        loginPanel.SetActive(false);
        registerPanel.SetActive(true);
    }

    public void onClickRegister()
    {
        if(text_register_account.text == "")
        {
            ToastScript.show("请输入账号");
            return;
        }

        if (text_register_password.text == "")
        {
            ToastScript.show("请输入密码");
            return;
        }

        if (text_register_password2.text == "")
        {
            ToastScript.show("请输入密码");
            return;
        }

        if (text_register_password.text != text_register_password2.text)
        {
            ToastScript.show("密码不一致");
            return;
        }

        C2S_Register c2s = new C2S_Register();
        c2s.Tag = CSParam.NetTag.Register.ToString();
        c2s.Account = text_register_account.text;
        c2s.Password = text_register_password.text;
        Socket_C.getInstance().Send(c2s);
    }

    void loginSuccess()
    {
        loginPanel.SetActive(false);
        registerPanel.SetActive(false);

        //LayerManager.showLayer(LayerManager.Layer.ChoiceRoleLayer);
        //Destroy(gameObject);

        GameObject layer = LayerManager.showLayer(LayerManager.Layer.MainLayer);
        Destroy(gameObject);
    }

    //----------------------------网络事件回调----------------------------------------------

    void onNetEvent(string data)
    {
        S2CBaseData s2cBaseData = JsonConvert.DeserializeObject<S2CBaseData>(data);

        if (s2cBaseData.Tag == CSParam.NetTag.Login.ToString())
        {
            S2C_Login s2c = JsonConvert.DeserializeObject<S2C_Login>(data);
            if (s2c.Code == (int)CSParam.CodeType.Ok)
            {
                UserData.userId = s2c.UserId;
                UserData.zhanshiLevel = s2c.zhanshiLevel;
                UserData.fashiLevel = s2c.fashiLevel;
                UserData.huanshiLevel = s2c.huanshiLevel;

                LocalDataManager.setBeforeServer(curServerConfigData.ip);
                LocalDataManager.setAccountData(text_login_account.text, text_login_password.text);

                ToastScript.show("登录成功");
                loginSuccess();
            }
            else if (s2c.Code == (int)CSParam.CodeType.LoginFail)
            {
                ToastScript.show("账号或密码不正确");
            }
        }
        else if (s2cBaseData.Tag == CSParam.NetTag.Register.ToString())
        {
            S2C_Register s2c = JsonConvert.DeserializeObject<S2C_Register>(data);
            if (s2c.Code == (int)CSParam.CodeType.Ok)
            {
                UserData.userId = s2c.UserId;
                UserData.zhanshiLevel = s2c.zhanshiLevel;
                UserData.fashiLevel = s2c.fashiLevel;
                UserData.huanshiLevel = s2c.huanshiLevel;

                LocalDataManager.setBeforeServer(curServerConfigData.ip);
                LocalDataManager.setAccountData(text_register_account.text, text_register_password.text);

                ToastScript.show("注册成功");
                loginSuccess();
            }
            else if (s2c.Code == (int)CSParam.CodeType.RegisterFail_Exist)
            {
                ToastScript.show("注册失败：账号已存在");
            }
            else
            {
                ToastScript.show("注册失败：" + s2c.Code);
            }
        }
        else
        {
            Debug.Log("未知tag，不予处理：" + data);
        }
    }
}
