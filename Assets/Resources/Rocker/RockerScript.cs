using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockerScript : MonoBehaviour
{
    public static RockerScript s_instance = null;

    public Action<float> onMove = null;
    public Action onMoveEnd = null;

    public bool m_useMouse;

    public GameObject m_bg;
    public GameObject m_ball;

    Vector3 m_rockerInitPos;
    float m_bgRadius;                       // 大圆的半径，这是小圆的最大移动范围
    float m_triggerMoveLength = 20;         // 触发距离，小圆移动超过这个距离，即可生效
    bool m_isStartMove;

    private void Awake()
    {
        s_instance = this;
    }

    void Start()
    {
        m_bgRadius = m_bg.GetComponent<RectTransform>().sizeDelta.x / 2;
        m_rockerInitPos = gameObject.transform.position;
    }

    void Update()
    {
        if (m_useMouse)
        {
            Vector2 touchPos_ui = CommonUtil.getCurMousePosToUI(Global.s_instance.canvas);
            Vector2 touchPos_world = CommonUtil.TouchPosToWorldPos(Input.mousePosition);

            // 点击摇杆
            if (Input.GetMouseButtonDown(0))
            {
                if (CommonUtil.TwoPointDistance2D(touchPos_ui, transform.localPosition) <= m_bgRadius)
                {
                    m_isStartMove = true;
                    m_ball.transform.position = touchPos_world;
                }
                else if (CommonUtil.TwoPointDistance2D(touchPos_ui, transform.localPosition) <= 300)
                {
                    gameObject.transform.localPosition = touchPos_ui;

                    m_isStartMove = true;
                    m_ball.transform.localPosition = new Vector3(0,0,0);
                }
            }
            // 松开摇杆
            else if (Input.GetMouseButtonUp(0))
            {
                if (m_isStartMove)
                {
                    m_bg.transform.localPosition = new Vector2(0, 0);
                    m_ball.transform.localPosition = new Vector2(0, 0);
                    m_isStartMove = false;

                    gameObject.transform.position = m_rockerInitPos;

                    // 通知松开摇杆
                    if(onMoveEnd != null)
                    {
                        onMoveEnd();
                    }
                }
            }
            // 滑动摇杆
            else if (Input.GetMouseButton(0))
            {
                if (m_isStartMove)
                {
                    if (CommonUtil.TwoPointDistance2D(touchPos_ui, transform.localPosition) <= m_bgRadius)
                    {
                        m_ball.transform.position = touchPos_world;
                    }
                    else
                    {
                        float k = (touchPos_world.y - m_bg.transform.position.y) / (touchPos_world.x - m_bg.transform.position.x);
                        float x = Mathf.Cos(Mathf.Atan(k)) * m_bgRadius;
                        float y = Mathf.Sin(Mathf.Atan(k)) * m_bgRadius;

                        x = Mathf.Abs(x);
                        y = Mathf.Abs(y);

                        if ((touchPos_world.x < m_bg.transform.position.x) && (touchPos_world.y > m_bg.transform.position.y))
                        {
                            x = -x;
                        }
                        else if ((touchPos_world.x < m_bg.transform.position.x) && (touchPos_world.y < m_bg.transform.position.y))
                        {
                            x = -x;
                            y = -y;
                        }
                        else if ((touchPos_world.x > m_bg.transform.position.x) && (touchPos_world.y < m_bg.transform.position.y))
                        {
                            y = -y;
                        }

                        m_ball.transform.localPosition = new Vector2(x, y);
                    }

                    if(CommonUtil.TwoPointDistance2D(m_bg.transform.localPosition,m_ball.transform.localPosition) >= m_triggerMoveLength)
                    {
                        // 通知滑动摇杆
                        if (onMove != null)
                        {
                            float angle = getRockerAngle();
                            onMove(angle);
                        }
                    }
                }
            }

            return;
        }
    }

    public float getRockerAngle()
    {
        if (CommonUtil.TwoPointDistance2D(new Vector2(0, 0), m_ball.transform.localPosition) <= m_triggerMoveLength)
        {
            return 0;
        }

        return CommonUtil.TwoPointAngle(new Vector2(0, 0), m_ball.transform.localPosition);
    }
}
