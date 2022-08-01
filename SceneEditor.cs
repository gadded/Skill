using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using Newtonsoft.Json;

public class SceneEditor : EditorWindow
{
    static SceneEditor editor;
    static List<Json_Data> json_s = new List<Json_Data>();
    static List<MonsterValue> monst_type = new List<MonsterValue>();
    int a = 0;
    static List<string> characterList = new List<string>();

    [MenuItem("Tools/³¡¾°±à¼­Æ÷")]
    public static void Init()
    {
        if (Application.isPlaying)
        {
            editor = EditorWindow.GetWindow<SceneEditor>("³¡¾°±à¼­Æ÷");
            if (editor != null)
            {
                editor.Show();
            }
        }
    }
    static void AddMonst(Json_Data item)
    {
        MonsterValue monster = new MonsterValue();
        monster.game = Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/aaa/" + item.path + ".prefab"));
        monster.name = item.name;
        monster.game.transform.position = new Vector3(item.x, item.y, item.z);
        monster.isactive = item.isActive;
        monst_type.Add(monster);
    }
   
    private void OnEnable()
    {
        OnSelecterCharacter();
        string str = File.ReadAllText(Application.dataPath + "/Skill/Monst.json");
        if (!string.IsNullOrEmpty(str))
        {
            json_s = JsonConvert.DeserializeObject<List<Json_Data>>(str);
        }
        foreach (var item in json_s)
        {
            AddMonst(item);
        }

    }

    private void OnSelecterCharacter()
    {
        characterList.Clear();
        string[] files = Directory.GetFiles(GetPath(), "*.prefab", SearchOption.AllDirectories);
        foreach (var item in files)
        {
            characterList.Add(Path.GetFileNameWithoutExtension(item));
        }
        characterList.Sort();
        characterList.Insert(0, "null");
    }

    private string GetPath()
    {
        return Application.dataPath + "/aaa";
    }

    private void OnGUI()
    {
        for (int i = 0; i < monst_type.Count; i++)
        {
            GUILayout.BeginHorizontal();
            monst_type[i].isactive = GUILayout.Toggle(monst_type[i].isactive, monst_type[i].name);
            if (monst_type[i].isactive)
            {
                monst_type[i].game.SetActive(true);
            }
            else
            {
                monst_type[i].game.SetActive(false);
            }
            monst_type[i].game.transform.position = EditorGUILayout.Vector3Field("pos:", monst_type[i].game.transform.position);
            int index = i;
            if (GUILayout.Button("É¾³ý"))
            {
                GameObject.Destroy(monst_type[i].game);
                monst_type.RemoveAt(index);
            }
            GUILayout.EndHorizontal();
        }
        a = EditorGUILayout.Popup(a, characterList.ToArray());
        if (GUILayout.Button("Ìí¼Ó"))
        {
            if (a != 0)
            {
                Json_Data _Data = new Json_Data();
                _Data.name = characterList[a];
                _Data.type = "Monst";
                _Data.x = Random.Range(0, 10);
                _Data.y = 0;
                _Data.z = Random.Range(0, 10);
                _Data.path = characterList[a];
                _Data.isActive = true;
                AddMonst(_Data);
            }
        }
        if (GUILayout.Button("±£´æ"))
        {
            json_s.Clear();
            foreach (var item in monst_type)
            {
                Json_Data _Data = new Json_Data();
                _Data.name = item.name;
                _Data.x = item.game.transform.position.x;
                _Data.y = item.game.transform.position.y;
                _Data.z = item.game.transform.position.z;
                _Data.path = item.name;
                _Data.isActive = item.isactive;
                json_s.Add(_Data);
            }
            string str = JsonConvert.SerializeObject(json_s);
            File.WriteAllText(Application.dataPath + "/Skill/Monst.json", str);
        }
    }
}
public class Json_Data
{
    public string name;
    public string type;
    public float x;
    public float y;
    public float z;
    public string path;
    public bool isActive;
}
public class MonsterValue
{
    public Transform parent;
    public GameObject game;
    public string name;
    public bool isactive = true;
}