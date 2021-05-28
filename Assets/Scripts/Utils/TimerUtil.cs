using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerUtil : MonoBehaviour
{
    public static TimerUtil s_instance = null;

    public delegate void OnCallBack();

    List<TimerData> timerDataList = new List<TimerData>();
    List<string> formList = new List<string>();

    public static bool isDestroy = false;

    class TimerData
    {
        public float curTime = 0;
        
        public float endTime;
        public OnCallBack onCallBack;

        public TimerData(float _endTime, OnCallBack _onCallBack)
        {
            endTime = _endTime;
            onCallBack = _onCallBack;
        }
    }

    public static TimerUtil getInstance()
    {
        if(s_instance == null)
        {
            GameObject obj = new GameObject();
            s_instance = obj.AddComponent<TimerUtil>();
            obj.name = "TimerUtil";
            DontDestroyOnLoad(obj);
        }

        return s_instance;
    }

    public void registerEvent(string _event)
    {
        Debug.Log("注册定时器事件：" + _event);
        for(int i = 0; i < formList.Count; i++)
        {
            if(formList[i] == _event)
            {
                return;
            }
        }

        formList.Add(_event);
    }

    public void cancelEvent(string _event)
    {
        Debug.Log("取消定时器事件：" + _event);
        for (int i = formList.Count - 1; i >= 0; i--)
        {
            if (formList[i] == _event)
            {
                formList.RemoveAt(i);
            }
        }
    }

    public void delayTime(float timeSeconds,OnCallBack onCallBack)
    {
        timerDataList.Add(new TimerData(timeSeconds, onCallBack));
    }
    
    void Update()
    {
        for(int i = 0; i < timerDataList.Count; i++)
        {
            timerDataList[i].curTime += Time.deltaTime;
        }

        for (int i = timerDataList.Count - 1; i >= 0 ; i--)
        {
            if(timerDataList[i].curTime >= timerDataList[i].endTime)
            {
                if (timerDataList[i].onCallBack != null)
                {
                    timerDataList[i].onCallBack();
                }
                timerDataList.RemoveAt(i);
            }
        }
    }

    void OnDestroy()
    {
        isDestroy = true;
    }
}
