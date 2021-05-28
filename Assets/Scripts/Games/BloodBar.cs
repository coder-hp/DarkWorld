using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BloodBar : MonoBehaviour
{
    public Image img_blood;

    void Start()
    {
        
    }

    public void init(bool isEnemy)
    {
        if(isEnemy)
        {
            img_blood.color = new Color(243f / 255f, 36f / 255f, 15f/255f);
            transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
        }
    }

    public void setIsTeam()
    {
        img_blood.color = new Color(0 / 255f, 203 / 255f, 204 / 255f);
    }

    public void setProgress(float progress)
    {
        img_blood.transform.localScale = new Vector3(progress,1,1);
    }
}
