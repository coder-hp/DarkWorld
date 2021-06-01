using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class CommonUtil
{
    static Material ImageGray = null;
    static long time_start = 0;

    // 13位时间戳：毫秒
    public static long getTimeStamp_Millisecond()
    {
        DateTime original = new DateTime(1970, 1, 1, 0, 0, 0);
        return (long)(DateTime.Now.ToUniversalTime() - original).TotalMilliseconds;
    }

    // 10位时间戳：秒
    public static long getTimeStamp_Second()
    {
        DateTime original = new DateTime(1970, 1, 1, 0, 0, 0);
        return (long)(DateTime.Now.ToUniversalTime() - original).TotalSeconds;
    }

    // 计时开始
    public static long jishi_start()
    {
        time_start = getTimeStamp_Millisecond();
        return time_start;
    }

    // 计时结束
    public static long jishi_end()
    {
        return getTimeStamp_Millisecond() - time_start;
    }
    
    // 格式2017/7/12 15:05:03
    public static string getCurTime()
    {
        return DateTime.Now.ToString();
    }

    public static DateTime StampToDateTime(long timeStamp)
    {
        timeStamp = 1606294609;

        DateTime dateTimeStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
        long lTime = long.Parse(timeStamp + "0000000");
        TimeSpan toNow = new TimeSpan(lTime);

        return dateTimeStart.Add(toNow);
    }

    /// <summary>
    /// 32位MD5加密
    /// </summary>
    /// <param name="password"></param>
    /// <returns></returns>
    public static string GetMD5(string password)
    {
        string cl = password;
        string pwd = "";
        MD5 md5 = MD5.Create(); //实例化一个md5对像
        // 加密后是一个字节类型的数组，这里要注意编码UTF8/Unicode等的选择　
        byte[] s = md5.ComputeHash(Encoding.UTF8.GetBytes(cl));
        // 通过使用循环，将字节类型的数组转换为字符串，此字符串是常规字符格式化所得
        for (int i = 0; i < s.Length; i++)
        {
            // 将得到的字符串使用十六进制类型格式。格式后的字符是小写的字母，如果使用大写（X）则格式后的字符是大写字符 
            pwd = pwd + s[i].ToString("x2");        // 字母小写
            // pwd = pwd + s[i].ToString("x2");     // 字母大写
        }
        return pwd;
    }

    // 格式2017/7
    static public string getCurYearMonth()
    {
        return DateTime.Now.Year + "/" + DateTime.Now.Month;
    }

    // 格式2017/7/10
    static public string getCurYearMonthDay()
    {
        return DateTime.Now.Year + "/" + DateTime.Now.Month + "/" + DateTime.Now.Day;
    }

    static public int getCurYear()
    {
        return DateTime.Now.Year;
    }

    static public int getCurMonth()
    {
        return DateTime.Now.Month;
    }

    static public int getCurDay()
    {
        return DateTime.Now.Day;
    }

    static public int getCurHour()
    {
        return DateTime.Now.Hour;
    }

    static public int getCurMinute()
    {
        return DateTime.Now.Minute;
    }

    static public int getCurSecond()
    {
        return DateTime.Now.Second;
    }

    static public int getCurMonthAllDays()
    {
        return DateTime.DaysInMonth(getCurYear(), getCurMonth());
    }

    static public bool splitStrIsPerfect(string str, List<string> list, char c)
    {
        bool b = false;
        {
            if (str[str.Length - 1] == c)
            {
                b = true;
            }
        }

        splitStr(str, list, c);

        return b;
    }

    /*
     * 裁剪字符串：1.2.3.3.5
     * str：源字符串
     * list：裁剪后存放的地方
     * c：裁剪规则
     * 如：splitStr("1.2.3.4.5",list,'.');
     */
    static public void splitStr(string str, List<string> list, char c)
    {
        string temp = "";
        for (int i = 0; i < str.Length; i++)
        {
            if (str[i] != c)
            {
                temp += str[i];
            }
            else
            {
                list.Add(temp);
                temp = "";
            }

            if ((str[i] != c) && (i == (str.Length - 1)))
            {
                list.Add(temp);
                temp = "";
            }
        }
    }

    /*
     * 裁剪字符串：1.2.3.3.5
     * str：源字符串
     * c：裁剪规则
     * 返回1
     */
    static public int splitStr_Start(string str, char c)
    {
        List<string> list = new List<string>();
        splitStr(str, list, c);

        return int.Parse(list[0]);
    }

    /*
     * 裁剪字符串：1.2.3.3.5
     * str：源字符串
     * c：裁剪规则
     * 返回200
     */
    static public int splitStr_End(string str, char c)
    {
        List<string> list = new List<string>();
        splitStr(str, list, c);

        return int.Parse(list[1]);
    }

    /*
     * subStringEndByChar("1/2/3/4/5/6",'/')
     * 返回6
     */
    static public string subStringEndByChar(string str, char c)
    {
        return str.Substring(str.LastIndexOf(c) + 1);
    }

    // size：物品数量
    // jiange：物品间隔
    // index：物品序号（从0开始）
    // centerPosX：居中位置坐标
    static public int getPosX(int size, int jiange, int index, int centerPosX)
    {
        int firstX;
        if (size % 2 == 0)
        {
            firstX = (centerPosX - jiange / 2) - (size / 2 - 1) * jiange;
        }
        else
        {
            firstX = centerPosX - (size / 2) * jiange;
        }

        return firstX + jiange * index;
    }

    static public bool isStrContain(string sourceStr, string containStr)
    {
        if (containStr.CompareTo("") == 0)
        {
            return false;
        }

        if (sourceStr.Length < containStr.Length)
        {
            return false;
        }

        for (int i = 0; i <= sourceStr.Length - containStr.Length; i++)
        {
            string temp = "";
            for (int j = i; j < (i + containStr.Length); j++)
            {
                temp += sourceStr[j];
            }

            if (temp.CompareTo(containStr) == 0)
            {
                return true;
            }
        }

        return false;
    }

    static public void setImageSprite(Image image, string path)
    {
        image.sprite = Resources.Load(path, typeof(Sprite)) as Sprite;
    }

    static public void setImageColor(Image image, float r, float g, float b)
    {
        image.color = new Color(r / 255.0f, g / 255.0f, b / 255.0f);
    }

    static public void setImageColor(Image image, float r, float g, float b, float a)
    {
        image.color = new Color(r / 255.0f, g / 255.0f, b / 255.0f, a / 255.0f);
    }

    static public void setFontColor(Text text, float r, float g, float b)
    {
        text.color = new Color(r / 255.0f, g / 255.0f, b / 255.0f);
    }

    static public void setButtonEnable(Button btn, bool enable)
    {
        if (enable)
        {
            btn.interactable = true;
            setImageColor(btn.transform.Find("Image").GetComponent<Image>(), 255, 255, 255, 255);
        }
        else
        {
            btn.interactable = false;
            setImageColor(btn.transform.Find("Image").GetComponent<Image>(), 255, 255, 255, 125);
        }
    }

    //字符转ASCII码：
    //character长度只能为1
    static public int charToAsc(string character)
    {
        System.Text.ASCIIEncoding asciiEncoding = new System.Text.ASCIIEncoding();
        int intAsciiCode = (int)asciiEncoding.GetBytes(character)[0];

        return intAsciiCode;
    }

    // 天数差
    // data_old:xxxx-xx-xx xx:xx:xx
    // data_new:xxxx-xx-xx xx:xx:xx
    static public int tianshucha(string data_old, string data_new)
    {
        DateTime d1 = Convert.ToDateTime(data_old);
        DateTime d2 = Convert.ToDateTime(data_new);
        DateTime d3 = Convert.ToDateTime(string.Format("{0}-{1}-{2}", d1.Year, d1.Month, d1.Day));
        DateTime d4 = Convert.ToDateTime(string.Format("{0}-{1}-{2}", d2.Year, d2.Month, d2.Day));
        int days = (d4 - d3).Days;

        return days;
    }

    // 秒数差
    // data_old:xxxx-xx-xx xx:xx:xx
    // data_new:xxxx-xx-xx xx:xx:xx
    static public int miaoshucha(string data_old, string data_new)
    {
        DateTime d1 = Convert.ToDateTime(data_old);
        DateTime d2 = Convert.ToDateTime(data_new);
        DateTime d3 = Convert.ToDateTime(string.Format("{0}-{1}-{2} {3}:{4}:{5}", d1.Year, d1.Month, d1.Day, d1.Hour, d1.Minute, d1.Second));
        DateTime d4 = Convert.ToDateTime(string.Format("{0}-{1}-{2} {3}:{4}:{5}", d2.Year, d2.Month, d2.Day, d2.Hour, d2.Minute, d2.Second));

        int days = (d4 - d3).Days;
        int hours = (d4 - d3).Hours;
        int minutes = (d4 - d3).Minutes;
        int seconds = (d4 - d3).Seconds;

        TimeSpan t1 = new TimeSpan(days, hours, minutes, seconds);
        int i = (int)t1.TotalSeconds;

        return i;
    }

    static public void setGray(Image img, bool isGray)
    {
        if (isGray)
        {
            if (ImageGray == null)
            {
                ImageGray = Resources.Load("materials/ImageGray", typeof(Material)) as Material;
            }

            img.material = ImageGray;
        }
        else
        {
            img.material = null;
        }
    }

    static public Sprite getSprite(string path)
    {
        return Resources.Load(path, typeof(Sprite)) as Sprite;
    }

    static public Texture GetTexture(string fullPath)
    {
        //return AssetDatabase.LoadAssetAtPath(fullPath, typeof(Texture)) as Texture;
        return Resources.Load(fullPath, typeof(Texture)) as Texture;
    }

    static public Material GetMaterial(string fullPath)
    {
        return Resources.Load(fullPath, typeof(Material)) as Material;
    }

    // 鼠标坐标转UI坐标(Canvas使用Screen Space-Camera模式)
    static public Vector2 getCurMousePosToUI(Transform canvas)
    {
        Vector2 pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas as RectTransform,
                    Input.mousePosition, canvas.GetComponent<Canvas>().worldCamera, out pos);
        return pos;
    }

    // 判断鼠标是否点击在某个UI上
    static public bool mousePosIsInContent(Transform canvas,Transform content)
    {
        Vector2 vec2 = getCurMousePosToUI(canvas);

        float x = content.transform.localPosition.x;
        float y = content.transform.localPosition.y;

        float width = content.GetComponent<RectTransform>().rect.width;
        float height = content.GetComponent<RectTransform>().rect.height;

        if ((vec2.x >= (x - width / 2)) &&
            (vec2.x <= (x + width / 2)) &&
            (vec2.y >= (y - height / 2)) &&
            (vec2.y <= (y + height / 2)))
        {
            return true;
        }

        return false;
    }

    static public Vector3 TouchPosToWorldPos(Vector2 vec2)
    {
        // 相机是世界的，世界到屏幕
        Vector3 camera = Camera.main.WorldToScreenPoint(GameObject.Find("UICamera").transform.position);
        Vector3 pos = new Vector3(vec2.x, vec2.y, camera.z);
        Vector3 WorldPoint = Camera.main.ScreenToWorldPoint(pos);
        return new Vector3(WorldPoint.x, WorldPoint.y, 0);
    }

    //static public bool mousePosIsInContent(Vector2 vec2,Transform content)
    //{
    //    vec2 = TouchPosToWorldPos(vec2);

    //    float x = content.transform.position.x;
    //    float y = content.transform.position.y;

    //    float width = content.GetComponent<RectTransform>().sizeDelta.x / 100;
    //    float height = content.GetComponent<RectTransform>().sizeDelta.y / 100;
    //    if ((vec2.x >= (x - width / 2)) &&
    //        (vec2.x <= (x + width / 2)) &&
    //        (vec2.y >= (y - height / 2)) &&
    //        (vec2.y <= (y + height / 2)))
    //    {
    //        return true;
    //    }

    //    return false;
    //}

    //static public bool uiPosIsInContent(Vector2 vec2, Transform content, object size = null)
    //{
    //    float pivot_y = content.GetComponent<RectTransform>().pivot.y;

    //    float x = content.transform.position.x;
    //    float y = content.transform.position.y;

    //    float width = content.GetComponent<RectTransform>().sizeDelta.x;
    //    float height = content.GetComponent<RectTransform>().sizeDelta.y;
    //    if(size != null)
    //    {
    //        width = ((Vector2)size).x;
    //        height = ((Vector2)size).y;
    //    }
    //    if ((vec2.x >= (x - width / 2)) &&
    //        (vec2.x <= (x + width / 2)) &&
    //        (vec2.y >= (y - height * pivot_y)) &&
    //        (vec2.y <= (y + height * (1 - pivot_y))))
    //    {
    //        return true;
    //    }

    //    return false;
    //}

    static public void setScrollViewEnable(Transform scrollView, bool enable)
    {
        scrollView.GetComponent<ScrollRect>().enabled = enable;
    }

    static public void setVisible(Transform obj , bool visible)
    {
        obj.localScale = new Vector3(visible ? 1 : 0, 1,1);
    }

    static public float twoObjDistance_3D(GameObject obj1,GameObject obj2)
    {
        return (obj1.transform.position - obj2.transform.position).magnitude;
    }

    static public float twoObjDistance_2D(GameObject obj1, GameObject obj2)
    {
        Vector2 pos1 = new Vector2(obj1.transform.position.x, obj1.transform.position.z);
        Vector2 pos2 = new Vector2(obj2.transform.position.x, obj2.transform.position.z);

        return (pos1 - pos2).magnitude;
    }

    static public float twoObjDistance_2D(Vector2 vec1, Vector2 vec2)
    {
        Vector2 pos1 = new Vector2(vec1.x, vec1.y);
        Vector2 pos2 = new Vector2(vec2.x, vec2.y);

        return (pos1 - pos2).magnitude;
    }

    static public float twoObjDistance_2D_2(GameObject obj1, GameObject obj2)
    {
        Vector2 pos1 = new Vector2(obj1.transform.position.y, obj1.transform.position.z);
        Vector2 pos2 = new Vector2(obj2.transform.position.y, obj2.transform.position.z);

        return (pos1 - pos2).magnitude;
    }

    // 3D中两点之间的距离
    static public float TwoPointDistance3D(Vector3 point1, Vector3 point2)
    {
        float i = Mathf.Sqrt((point1.x - point2.x) * (point1.x - point2.x)
                           + (point1.y - point2.y) * (point1.y - point2.y)
                           + (point1.z - point2.z) * (point1.z - point2.z));

        return i;
    }

    // 2D中两点之间的距离
    static public float TwoPointDistance2D(Vector3 point1, Vector3 point2)
    {
        float i = Mathf.Sqrt((point1.x - point2.x) * (point1.x - point2.x)
                           + (point1.y - point2.y) * (point1.y - point2.y));

        return i;
    }

    // 返回值：0~360,从正Y轴开始
    static public float TwoPointAngle(Vector2 point1, Vector2 point2)
    {
        float angle = 0;
        float k = (point2.y - point1.y) / (point2.x - point1.x);
        float tempAngle = Mathf.Atan(k) * Mathf.Rad2Deg;

        // 第一象限
        if ((point2.x > point1.x) && (point2.y > point1.y))
        {
            angle = 90 - tempAngle;
        }
        // 第二象限
        else if ((point2.x < point1.x) && (point2.y > point1.y))
        {
            angle = 360 - (90 + tempAngle);
        }
        // 第三象限
        else if ((point2.x < point1.x) && (point2.y < point1.y))
        {
            angle = 180 + (90 - tempAngle);
        }
        // 第四象限
        else if ((point2.x > point1.x) && (point2.y < point1.y))
        {
            angle = 90 + (-tempAngle);
        }

        return angle;
    }

    static public bool CheckPointInGameObject(GameObject obj, Vector3 vec3)
    {
        Vector3 pos = obj.transform.position;
        float width = obj.GetComponent<RectTransform>().sizeDelta.x;
        float height = obj.GetComponent<RectTransform>().sizeDelta.y;

        if ((vec3.x >= (pos.x - width / 2)) &&
            (vec3.x <= (pos.x + width / 2)) &&
            (vec3.y >= (pos.y - height / 2)) &&
            (vec3.y <= (pos.y + height / 2)))
        {
            return true;
        }

        return false;
    }

    static public GameObject minDistance(GameObject obj, List<GameObject> listObj)
    {
        GameObject backObj = null;
        float distance = 10000;

        for (int i = 0; i < listObj.Count; i++)
        {
            float temp = TwoPointDistance3D(obj.transform.position, listObj[i].transform.position);
            if (temp < distance)
            {
                distance = temp;
                backObj = listObj[i];
            }
        }

        return backObj;
    }

    static public float angleToRadian(float angle)
    {
        return angle * Mathf.Deg2Rad;
    }

    static public GameObject createObjFromPrefab(Transform parent,string path)
    {
        GameObject pre = Resources.Load(path, typeof(GameObject)) as GameObject;
        if (pre != null)
        {
            return GameObject.Instantiate(pre, parent);
        }

        return null;
    }

    static public void Log(string str)
    {
        Debug.Log(str);

        //if (HttpUtil.s_instance)
        //{
        //    HttpUtil.s_instance.reqGet("http://47.251.34.72:9999/log?param=" + str);
        //}

        //if (DebugPanelScript.s_instance != null)
        //{
        //    DebugPanelScript.s_instance.addLog(str);
        //}
    }

    static public Vector2 WorldPosToUI(Camera worldCamera ,Vector3 worldPos)
    {
        // Canvas Render Mode = Screen Space-OverLay时，使用下面的方法
        // return worldCamera.WorldToScreenPoint(worldPos);

        // Canvas Render Mode = Screen Space-Camera时，使用下面的方法
        Vector2 pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(Global.s_instance.canvas.transform as RectTransform,
            worldCamera.WorldToScreenPoint(worldPos), Global.s_instance.canvas.GetComponent<Canvas>().worldCamera, out pos);
        return pos;
    }

    static public string formatTime(string time)
    {
        string[] array = time.Split('/');
        if (array.Length >= 3)
        {
            time = "";
            time += (array[0] + "/");
            if (array[1].Length == 1)
            {
                time += ("0" + array[1] + "/");
            }
            else
            {
                time += (array[1] + "/");
            }

            if (array[2].Length == 1)
            {
                time += ("0" + array[2]);
            }
            else
            {
                time += (array[2]);
            }
        }

        return time;
    }
}