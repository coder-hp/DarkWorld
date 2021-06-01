using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetLoading : MonoBehaviour
{
    public static NetLoading s_instance = null;

    void Start()
    {
        s_instance = this;

        close();
    }

    public void show()
    {
        gameObject.SetActive(true);
        Invoke("close",3);
    }

    public void close()
    {
        gameObject.SetActive(false);
    }
}
