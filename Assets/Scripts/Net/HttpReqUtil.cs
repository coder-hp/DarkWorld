using System.Collections;
using UnityEngine;
using UnityEngine.Networking;


public class HttpReqUtil:MonoBehaviour
{
    public static HttpReqUtil s_instance;

    public delegate void CallBack(string data);

    public static HttpReqUtil getInstance()
    {
        if (s_instance == null)
        {
            GameObject obj = new GameObject();
            obj.name = "HttpReqUtil";
            s_instance = obj.AddComponent<HttpReqUtil>();
            DontDestroyOnLoad(obj);
        }

        return s_instance;
    }

    public void Get(string url, CallBack callback)
    {
        // 防止缓存
        url += ("?" + CommonUtil.getCurTime());

        StartCoroutine(DoGet(url, callback));
    }

    IEnumerator DoGet(string url, CallBack callback)
    {
        using (UnityWebRequest www = UnityWebRequest.Get(url))
        {
            yield return www.Send();

            if (www.isNetworkError)
            {
                callback(www.error);
            }
            else
            {
                if (callback != null)
                {
                    callback(www.downloadHandler.text.TrimStart());
                }
            }
        }
    }

    public void Post(string url, WWWForm form, CallBack callback)
    {
        StartCoroutine(DoPost(url, form, callback));
    }

    IEnumerator DoPost(string url, WWWForm form, CallBack callback)
    {
        using (UnityWebRequest www = UnityWebRequest.Post(url, form))
        {
            yield return www.Send();

            if (www.isNetworkError)
            {
                callback(www.error);
            }
            else
            {
                if (callback != null)
                {
                    callback(www.downloadHandler.text.TrimStart());
                }
            }
        }
    }
}