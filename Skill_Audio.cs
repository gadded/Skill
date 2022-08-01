using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Audio : SkillBase
{
    public AudioClip clip;
    AudioSource anim;
    public PlayerEdi player;
    public Player player1;

    public Skill_Audio(PlayerEdi player)
    {
        this.player = player;
        anim = player.gameObject.GetComponent<AudioSource>();
    }
    public Skill_Audio(Player player)
    {
        this.player1 = player;
        anim = player.gameObject.GetComponent<AudioSource>();
    }

    public override void Init()
    {
        base.Init();
        anim.clip = clip;
    }

    public override void Stop()
    {
        base.Stop();
        anim.Stop();
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
        anim.Play();
    }

    public void SetGameClip(AudioClip clip)
    {
        this.clip = clip;
        name = this.clip.name;
        anim.clip = clip;
    }
}
