using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Global : MonoBehaviour
{
    public static Global s_instance = null;

    public Camera camera_ui = null;
    public Camera camera_world = null;

    public Transform canvas;
    public Transform canvas_high;

    public Text Text_fps;
    public Text Text_ping;

    int fpsFlag = 0;

    void Awake()
    {
        s_instance = this;

        LocalDataManager.init();
    }

    private void Update()
    {
        if (Text_fps)
        {
            if (fpsFlag == 0)
            {
                int curFps = (int)(1.0f / Time.deltaTime);
                Text_fps.text = "fps:" + curFps;
                fpsFlag = 8;
            }
            else
            {
                --fpsFlag;
            }
        }
    }
}
