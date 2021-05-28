//#define IN_UNITY_EDITOR

using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Collections;
using System;
using UnityEngine.Networking;

public class AudioScript : MonoBehaviour {

    public static GameObject s_audioObj = null;
    public static AudioScript s_audioScript;

    
    public AudioSource m_musicPlayer;           // 背景音乐只会有一个
    public List<AudioSource> m_soundPlayer;     // 音效会同时播放多个，所以用List

    public float m_musicVolume = 1.0f;
    public float m_soundVolume = 1.0f;

    static public AudioScript getAudioScript ()
    {
        if(!s_audioObj)
        {
            s_audioObj = new GameObject();
            s_audioObj.transform.name = "Audio";
            MonoBehaviour.DontDestroyOnLoad(s_audioObj);
            s_audioScript = s_audioObj.AddComponent<AudioScript>();
            s_audioScript.init();
        }

        return s_audioScript;
    }

    public void init ()
    {
        initMusicPlayer();
        initSoundPlayer();
    }

    public void initMusicPlayer ()
    {
        m_musicVolume = PlayerPrefs.GetFloat("MusicVolume",1.0f);

        GameObject go = new GameObject("musicPlayer");
        go.transform.SetParent(transform, false);
        AudioSource player = go.AddComponent<AudioSource>();
        m_musicPlayer = player;

        player.loop = true;
        player.mute = false;
        player.volume = m_musicVolume;
        player.pitch = 1.0f;
        player.playOnAwake = false;
    }

    public void initSoundPlayer ()
    {
        m_soundVolume = PlayerPrefs.GetFloat("SoundVolume", 1.0f);

        m_soundPlayer = new List<AudioSource>();

        GameObject go = new GameObject("soundPlayer");
        go.transform.SetParent(transform, false);
        AudioSource player = go.AddComponent<AudioSource>();
        m_soundPlayer.Add(player);

        player.loop = false;
        player.mute = false;
        player.volume = m_soundVolume;
        player.pitch = 1.0f;
        player.playOnAwake = false;
    }

    public void playMusic (string audioPath,bool isLoop)
    {
        if(m_musicPlayer.isPlaying)
        {
            m_musicPlayer.Stop();
        }
        m_musicPlayer.clip = Resources.Load(audioPath, typeof(AudioClip)) as AudioClip;
        m_musicPlayer.Play();
        m_musicPlayer.loop = isLoop;
        m_musicPlayer.volume = m_musicVolume;
    }

    public void playMusicByAssets(string audioPath, bool isLoop)
    {
#if IN_UNITY_EDITOR
        if (m_musicPlayer.isPlaying)
        {
            m_musicPlayer.Stop();
        }
        m_musicPlayer.clip = AssetDatabase.LoadAssetAtPath(audioPath, typeof(AudioClip)) as AudioClip;
        m_musicPlayer.Play();
        m_musicPlayer.loop = isLoop;
        m_musicPlayer.volume = m_musicVolume;
#endif
    }

    public void playMusicByDownFile(string audioPath,bool isLoop,Action loadCallback = null)
    {
        if (m_musicPlayer.isPlaying)
        {
            m_musicPlayer.Stop();
        }

        Debug.Log("AudioScript.playMusicByDownFile:" + audioPath);
        if (File.Exists(audioPath))
        {
            StartCoroutine(LoadAudio(audioPath, (audioClip) =>
            {
                m_musicPlayer.clip = audioClip;
                m_musicPlayer.Play();
                m_musicPlayer.loop = isLoop;
                m_musicPlayer.volume = m_musicVolume;

                if (loadCallback != null)
                {
                    loadCallback();
                }
            }));
        }
        else
        {
            Debug.Log("File not Exists:" + audioPath);
        }
    }

    IEnumerator LoadAudio(string filePath, Action<AudioClip> loadFinish)
    {

        //#if UNITY_ANDROID
        //        filePath = "file://" + filePath;
        //#elif UNITY_IOS
        //        filePath = "file://" + filePath;
        //#elif UNITY_STANDALONE_WIN
        //        filePath = "file://" + filePath;
        //#endif

        filePath = "file://" + filePath;

        using (var uwr = UnityWebRequestMultimedia.GetAudioClip(filePath, AudioType.MPEG))
        {
            yield return uwr.SendWebRequest();
            if (uwr.isNetworkError)
            {
                Debug.Log("LoadAudio error:" + uwr.error);
            }
            else
            {
                AudioClip audioClip = DownloadHandlerAudioClip.GetContent(uwr);
                loadFinish(audioClip);
            }
        }
    }

    public void playSound (string audioPath)
    {
        for(int i = 0; i < m_soundPlayer.Count; i++)
        {
            if(!m_soundPlayer[i].isPlaying)
            {
                m_soundPlayer[i].clip = Resources.Load(audioPath, typeof(AudioClip)) as AudioClip;
                m_soundPlayer[i].Play();

                return;
            }
        }

        // 如果执行到这里，说明暂时没有空余的音效组件使用，需要再新建一个
        {
            GameObject go = new GameObject("soundPlayer");
            go.transform.SetParent(transform, false);
            AudioSource player = go.AddComponent<AudioSource>();
            m_soundPlayer.Add(player);

            player.loop = false;
            player.mute = false;
            player.volume = m_soundVolume;
            player.pitch = 1.0f;
            player.playOnAwake = false;

            player.clip = (AudioClip)Resources.Load(audioPath, typeof(AudioClip));
            player.Play();
        }
    }

    public float getMusicVolume()
    {
        return m_musicVolume;
    }

    public void setMusicVolume(float volume)
    {
        m_musicVolume = volume;
        m_musicPlayer.volume = m_musicVolume;

       PlayerPrefs.SetFloat("MusicVolume", m_musicVolume);
    }

    public float getSoundVolume()
    {
        return m_soundVolume;
    }

    public void setSoundVolume(float volume)
    {
        m_soundVolume = volume;
        for (int i = 0; i < m_soundPlayer.Count; i++)
        {
            m_soundPlayer[i].volume = m_soundVolume;
        }

        PlayerPrefs.SetFloat("SoundVolume", m_soundVolume);
    }

    public void pauseMusic()
    {
        if (m_musicPlayer.isPlaying)
        {
            m_musicPlayer.Pause();
        }
    }

    public void resumeMusic()
    {
        if (m_musicPlayer)
        {
            m_musicPlayer.Play();
        }
    }

    public void stopMusic ()
    {
        if(m_musicPlayer.isPlaying)
        {
            m_musicPlayer.Stop();
            m_musicPlayer.clip.UnloadAudioData();
        }
    }

    public void stopSound ()
    {
        for(int i = 0; i < m_soundPlayer.Count; i++)
        {
            if(m_soundPlayer[i].isPlaying)
            {
                m_soundPlayer[i].Stop();
            }
        }
    }
    
    
    public void playSound_btn()
    {
        playSound("Audios/btn");
    }
    
}
