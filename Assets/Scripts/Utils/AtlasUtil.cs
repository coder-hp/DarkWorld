using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class AtlasUtil
{
    static SpriteAtlas atlas_login = null;
    static SpriteAtlas atlas_main = null;

    public static SpriteAtlas getAtlas_login()
    {
        if(atlas_login == null)
        {
            atlas_login = Resources.Load("Atlas/login", typeof(SpriteAtlas)) as SpriteAtlas;
        }
        return atlas_login;
    }

    public static SpriteAtlas getAtlas_main()
    {
        if (atlas_main == null)
        {
            atlas_main = Resources.Load("Atlas/main", typeof(SpriteAtlas)) as SpriteAtlas;
        }
        return atlas_main;
    }
}
