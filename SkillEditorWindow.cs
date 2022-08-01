using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.IO;

public class SkillEditorWindow : EditorWindow
{
    class PlayerEditor
    {
        public string characterName = string.Empty;
        public string folderName = string.Empty;
        public int folderIndex = 0;
        public int characterIndex = 0;
        public List<string> list = new List<string>();
        public PlayerEdi player = new PlayerEdi();
    }
    PlayerEditor player = new PlayerEditor();
    List<string> characterList = new List<string>();
    List<string> folderList = new List<string>();
    Dictionary<string, List<string>> dic = new Dictionary<string, List<string>>();
    string newSkillName = string.Empty;
    SkillWindow skillWindow;

    [MenuItem("Tools/技能编辑器")]
    public static void Init()
    {
        if (Application.isPlaying)
        {
            SkillEditorWindow skillEditor = EditorWindow.GetWindow<SkillEditorWindow>("技能编辑器");
            if (skillEditor != null)
            {
                skillEditor.Show();
            }
        }
    }

    private void OnEnable()
    {
        OnSelectFolder();
        OnSelectCharacter();
    }
    private void OnSelectFolder()
    {
        folderList.Clear();
        folderList.Add("All");
        string[] strs = Directory.GetDirectories(GetPath());
        foreach (var item in strs)
        {
            folderList.Add(Path.GetFileName(item));
        }
    }

    private void OnSelectCharacter()
    {
        characterList.Clear();
        string[] strs = Directory.GetFiles(GetPath(), "*.prefab", SearchOption.AllDirectories);
        foreach (var item in strs)
        {
            characterList.Add(Path.GetFileNameWithoutExtension(item));
        }
        characterList.Sort();
        characterList.Insert(0, "null");
        player.list = characterList;
    }

    private string GetPath()
    {
        return Application.dataPath + "/GameDate/Model";
    }

    private void OnGUI()
    {
        int folderIndex = EditorGUILayout.Popup(player.folderIndex, folderList.ToArray());
        if (folderIndex != player.folderIndex)
        {
            player.folderIndex = folderIndex;
            player.characterIndex = -1;
            player.folderName = folderList[folderIndex];
            List<string> list;
            if (!dic.TryGetValue(player.folderName, out list))
            {
                string[] str = Directory.GetFiles(GetPath() + "/" + player.folderName, "*.prefab", SearchOption.AllDirectories);
                foreach (var item in str)
                {
                    list.Add(Path.GetFileNameWithoutExtension(item));
                }
            }
            dic.Add(player.folderName, list);
            player.list = list;
        }
        int characterIndex = EditorGUILayout.Popup(player.characterIndex, player.list.ToArray());
        if (characterIndex != player.characterIndex)
        {
            player.characterIndex = characterIndex;
            player.characterName = player.list[characterIndex];
            if (!string.IsNullOrEmpty(player.characterName) && player.player != null)
            {
                player.player.OnDestoy();
            }
            player.player = PlayerEdi.Init(player.characterName);
        }
        newSkillName = EditorGUILayout.TextField(newSkillName);
        if (GUILayout.Button("创建新技能"))
        {
            if (!string.IsNullOrEmpty(newSkillName) && player.player != null)
            {
                List<SkillBase> skills = player.player.AddSkill(newSkillName);
                OpenWindow(newSkillName, skills);
            }
        }
        foreach (var item in player.player.dic)
        {
            if (GUILayout.Button(item.Key))
            {
                OpenWindow(item.Key, item.Value);
            }
            if (GUILayout.Button("删除"))
            {
                player.player.RevSkill(item.Key);
                break;
            }
        }
    }

    private void OpenWindow(string newSkillName, List<SkillBase> skills)
    {
        if (!string.IsNullOrEmpty(newSkillName) && skills != null)
        {
            if (skillWindow == null)
            {
                skillWindow = EditorWindow.GetWindow<SkillWindow>(newSkillName);
            }
            skillWindow.Init(skills, player.player);
            skillWindow.Show();
            skillWindow.Repaint();
        }
    }
}
