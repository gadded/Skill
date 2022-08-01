using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Effect : SkillBase
{
    public GameObject game;
    GameObject obj;
    ParticleSystem system;
    public PlayerEdi player;
    public Player player1;

    public Skill_Effect(PlayerEdi player)
    {
        this.player = player;
    }
    public Skill_Effect(Player player)
    {
        this.player1 = player;
    }

    public override void Init()
    {
        base.Init();
        system = obj.GetComponent<ParticleSystem>();
        name = obj.name;
        system.Stop();
    }

    public override void Stop()
    {
        base.Stop();
        system.Stop();
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
        system.Play();
    }

    public void SetGameClip(GameObject clip)
    {
        this.game = clip;
        if (game != null)
        {
            obj = GameObject.Instantiate(game, player.effect);
            system = obj.GetComponent<ParticleSystem>();
            name = obj.name;
            system.Stop();
        }
    }
    public void SetGameClip1(GameObject clip)
    {
        this.game = clip;
        if (this.game != null)
        {
            obj = GameObject.Instantiate(this.game, player1.effect);
            system = obj.GetComponent<ParticleSystem>();
            name = obj.name;
            system.Stop();
        }
    }
}
