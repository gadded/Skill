using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using Newtonsoft.Json;
using System.IO;

public class SkillWindow : EditorWindow
{
    PlayerEdi player;
    List<SkillBase> skill;
    public void Init(List<SkillBase> skills, PlayerEdi player)
    {
        this.player = player;
        this.skill = skills;
        this.player.skills = this.skill;
    }
    string[] strs = { "", "����", "����", "��Ч" };
    int index = 0;
    private void OnGUI()
    {
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("����"))
        {
            foreach (var item in skill)
            {
                item.Play();
            }
        }
        if (GUILayout.Button("ֹͣ"))
        {
            foreach (var item in skill)
            {
                item.Stop();
            }
        }
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        index = EditorGUILayout.Popup(index, strs);
        if (GUILayout.Button("���"))
        {
            switch (index)
            {
                case 1:
                    skill.Add(new Skill_Anim(player));
                    break;
                case 2:
                    skill.Add(new Skill_Audio(player));
                    break;
                case 3:
                    skill.Add(new Skill_Effect(player));
                    break;
            }
        }
        GUILayout.EndHorizontal();
        foreach (var item in skill)
        {
            if (GUILayout.Button("ɾ��"))
            {
                skill.Remove(item);
                break;
            }
            if (item is Skill_Anim)
            {
                Skill_Anim anim = item as Skill_Anim;
                string str = EditorGUILayout.TextField(anim.trigger);
                AnimationClip a = EditorGUILayout.ObjectField(anim.clip, typeof(AnimationClip), false) as AnimationClip;
                if (a != anim.clip)
                {
                    anim.SetGameClip(a);
                }
            }
            else if (item is Skill_Audio)
            {
                Skill_Audio anim = item as Skill_Audio;
                string str = EditorGUILayout.TextField(anim.trigger);
                AudioClip a = EditorGUILayout.ObjectField(anim.clip, typeof(AudioClip), false) as AudioClip;
                if (a != anim.clip)
                {
                    anim.SetGameClip(a);
                }
            }
            else if (item is Skill_Effect)
            {
                Skill_Effect anim = item as Skill_Effect;
                string str = EditorGUILayout.TextField(anim.trigger);
                GameObject a = EditorGUILayout.ObjectField(anim.game, typeof(GameObject), false) as GameObject;
                if (a != anim.game)
                {
                    anim.SetGameClip(a);
                }
            }
        }
        if (GUILayout.Button("����"))
        {
            List<SkillXML> list = new List<SkillXML>();
            foreach (var item in player.dic)
            {
                SkillXML skillXML = new SkillXML();
                skillXML.name = item.Key;
                foreach (var ite in item.Value)
                {
                    if (ite is Skill_Anim)
                    {
                        if (!skillXML.dic.ContainsKey("����"))
                        {
                            skillXML.dic.Add("����", new List<Skill>());
                        }
                        skillXML.dic["����"].Add(new Skill(ite.name, ite.trigger));
                    }
                    else if (ite is Skill_Audio)
                    {
                        if (!skillXML.dic.ContainsKey("����"))
                        {
                            skillXML.dic.Add("����", new List<Skill>());
                        }
                        skillXML.dic["����"].Add(new Skill(ite.name, ite.trigger));
                    }
                    else if (ite is Skill_Effect)
                    {
                        if (!skillXML.dic.ContainsKey("��Ч"))
                        {
                            skillXML.dic.Add("��Ч", new List<Skill>());
                        }
                        skillXML.dic["��Ч"].Add(new Skill(ite.name, ite.trigger));
                    }
                }
                list.Add(skillXML); 
            }
            string str = JsonConvert.SerializeObject(list);
            File.WriteAllText("Assets/Skill/" + player.name + ".json", str);
        }
    }
}
