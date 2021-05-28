using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerManager
{
    public enum Layer
    {
        LoginLayer,
        ChoiceRoleLayer,
        MainLayer,
        BagLayer,
        GameUILayer,
        MatchLayer,
    }

    public static GameObject showLayer(Layer layer)
    {
        GameObject pre = Resources.Load("Prefabs/Layers/" + layer) as GameObject;
        GameObject obj = GameObject.Instantiate(pre, Global.s_instance.canvas);

        return obj;
    }
}
