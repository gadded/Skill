using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Anim : SkillBase
{
    public AnimationClip clip;
    Animator anim;
    public PlayerEdi player;
    public Player player1;
    AnimatorOverrideController controller;

    public Skill_Anim(PlayerEdi player)
    {
        this.player = player;
        controller = player.controller;
        anim = player.gameObject.GetComponent<Animator>();
    }
    public Skill_Anim(Player player)
    {
        this.player1 = player;
        controller = player.controller;
        anim = player.gameObject.GetComponent<Animator>();
    }

    public override void Init()
    {
        base.Init();
        controller["Attack1"] = clip;
    }

    public override void Stop()
    {
        base.Stop();
        anim.StartPlayback();
    }

    public override void Play()
    {
        isBegin = true;
        starttime = Time.time;
    }
    public override void Play1()
    {
        isBegin = true;
        starttime = Time.time;
        Begin();
    }
    public override void UpDate(float time)
    {
        base.UpDate(time);
        if (isBegin && (time - starttime) > float.Parse(trigger))
        {
            isBegin = false;
            Begin();
        }
    }

    private void Begin()
    {
        anim.StopPlayback();
        AnimatorStateInfo info = anim.GetCurrentAnimatorStateInfo(0);
        if (info.IsName("Idle1"))
        {
            anim.SetTrigger("Play");
        }
    }

    public void SetGameClip(AnimationClip clip)
    {
        this.clip = clip;
        name = this.clip.name;
        controller["Attack1"] = clip;
    }
}
