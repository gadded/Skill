using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;
using System;
using Newtonsoft.Json;

public class PlayerEdi : MonoBehaviour
{
    public Dictionary<string, List<SkillBase>> dic = new Dictionary<string, List<SkillBase>>();
    public AnimatorOverrideController controller;
    RuntimeAnimatorController runtime;
    public List<SkillBase> skills = new List<SkillBase>();

    public Transform effect;
    Animator anim;
    AudioSource source;


    private void Awake()
    {
        anim = gameObject.GetComponent<Animator>();
    }

    public static PlayerEdi Init(string path)
    {
        if (File.Exists("Assets/aaa/" + path + ".prefab"))
        {
            GameObject game = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/aaa/" + path + ".prefab");
            if (game != null)
            {
                Debug.Log("找到" + path);
                PlayerEdi player = GameObject.Instantiate(game).AddComponent<PlayerEdi>();
                player.controller = new AnimatorOverrideController();
                player.runtime = Resources.Load<RuntimeAnimatorController>("player1");
                player.controller.runtimeAnimatorController = player.runtime;
                player.anim.runtimeAnimatorController = player.controller;
                player.source = player.gameObject.AddComponent<AudioSource>();
                player.effect = player.transform.Find("Effect");
                player.gameObject.name = path;
                player.LoadAllSkill();
                return player;
            }
        }
        return null;
    }

    private void LoadAllSkill()
    {
        if (File.Exists("Assets/Skill/" + gameObject.name + ".json"))
        {
            string str = File.ReadAllText("Assets/Skill/" + gameObject.name + ".json");
            if (!string.IsNullOrEmpty(str))
            {
                List<SkillXML> list = JsonConvert.DeserializeObject<List<SkillXML>>(str);
                foreach (var item in list)
                {
                    dic.Add(item.name, new List<SkillBase>());
                    foreach (var ite in item.dic)
                    {
                        foreach (var it in ite.Value)
                        {
                            switch (ite.Key)
                            {
                                case "动画":
                                    Skill_Anim _Anim = new Skill_Anim(this);
                                    AnimationClip clip = AssetDatabase.LoadAssetAtPath<AnimationClip>("Assets/GameDate/Anim/" + it.SkillName + ".anim");
                                    _Anim.SetGameClip(clip);
                                    _Anim.Trigger(it.Trigger);
                                    dic[item.name].Add(_Anim);
                                    break;
                                case "声音":
                                    Skill_Audio _Audio = new Skill_Audio(this);
                                    AudioClip audio = AssetDatabase.LoadAssetAtPath<AudioClip>("Assets/GameDate/Audio/" + it.SkillName + ".mp3");
                                    _Audio.SetGameClip(audio);
                                    _Audio.Trigger(it.Trigger);
                                    dic[item.name].Add(_Audio);
                                    break;
                                case "特效":
                                    Skill_Effect _Effect = new Skill_Effect(this);
                                    GameObject ga = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/GameDate/Effect/Nova/" + it.SkillName + ".prefab");
                                    _Effect.SetGameClip(ga);
                                    _Effect.Trigger(it.Trigger);
                                    dic[item.name].Add(_Effect);
                                    break;
                            }
                        }
                    }
                }
            }
        }
    }
    public void Play()
    {
        foreach (var item in skills)
        {
            item.Play();
        }
    }

    // Update is called once per frame
    void Update()
    {
        foreach (var item in skills)
        {
            item.UpDate(Time.time);
        }
    }
    public List<SkillBase> AddSkill(string name)
    {
        if (dic.ContainsKey(name))
        {
            return dic[name];
        }
        dic.Add(name, new List<SkillBase>());
        return dic[name];
    }
    public List<SkillBase> GetSkill(string name)
    {
        if (dic.ContainsKey(name))
        {
            return dic[name];
        }
       
        return null;
    }
    public void RevSkill(string name)
    {
        if (dic.ContainsKey(name))
        {
            dic.Remove(name);
        }
    }
    public void OnDestoy()
    {
        Destroy(gameObject);
    }
}
public class SkillXML
{
    public string name;
    public Dictionary<string, List<Skill>> dic = new Dictionary<string, List<Skill>>();
}

public class Skill
{
    public string SkillName;
    public string Trigger;

    public Skill(string skillName, string trigger)
    {
        SkillName = skillName;
        Trigger = trigger;
    }
}