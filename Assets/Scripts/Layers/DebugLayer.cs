using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugLayer : MonoBehaviour
{
    public static DebugLayer s_instance = null;

    public Text text_reqCount;
    public Text text_backCount;

    int reqCount = 0;
    int backCount = 0;

    void Start()
    {
        s_instance = this;
    }

    public void addReqCount()
    {
        ++reqCount;
        text_reqCount.text = "请求:"+reqCount;
    }

    public void addBackCount()
    {
        ++backCount;
        text_backCount.text = "返回:" + backCount;
    }

    public void onClickFps()
    {
        if (transform.Find("btn_fps/Text").GetComponent<Text>().text == "帧率60")
        {
            transform.Find("btn_fps/Text").GetComponent<Text>().text = "帧率10";
            Application.targetFrameRate = 10;
        }
        else if (transform.Find("btn_fps/Text").GetComponent<Text>().text == "帧率10")
        {
            transform.Find("btn_fps/Text").GetComponent<Text>().text = "帧率30";
            Application.targetFrameRate = 30;
        }
        else if (transform.Find("btn_fps/Text").GetComponent<Text>().text == "帧率30")
        {
            transform.Find("btn_fps/Text").GetComponent<Text>().text = "帧率60";
            Application.targetFrameRate = 60;
        }
    }
}
