using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillBase
{
    public string name = string.Empty;
    public string trigger = "0";
    public bool isBegin = false;
    public float starttime = 0;

    public virtual void Init()
    {

    }
    public virtual void Trigger(string str)
    {
        trigger = str;
    }
    public virtual void Stop()
    {

    }
    public virtual void Play()
    {

    }
    public virtual void Play1()
    {

    }
    public virtual void UpDate(float time)
    {

    }
}
