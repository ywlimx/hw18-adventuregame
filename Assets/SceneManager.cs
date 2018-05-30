﻿using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class SceneManager : MonoBehaviour
{
    public string m_outputPath = "Assets/ScenePrefabs";

	// Use this for initialization
	void Start ()
	{
	    ReloadScenePrefabs();
	}

    // Update is called once per frame
    void Update () {
		
	}

    // replace all children with the associated prefabs
    public void ReloadScenePrefabs()
    {
#if UNITY_EDITOR
        List<GameObject> deleteGameObjects = new List<GameObject>();
        List<Object> prefabObjects = new List<Object>();
        foreach (Transform child in transform)
        {
            // create a new prefab
            string scenePrefabName = string.Format("{0}/{1}.prefab", m_outputPath, child.name);
            Object prefab = AssetDatabase.LoadAssetAtPath<Object>(scenePrefabName);
            if (prefab != null)
            {
                deleteGameObjects.Add(child.gameObject);
                prefabObjects.Add(prefab);
            }
            else
            {
                Debug.LogWarningFormat("Could not find sceneprefab at {0} to replace child gameobject!", scenePrefabName);
            }
        }

        for (int i = 0; i < deleteGameObjects.Count; ++i)
        {
            GameObject go = (GameObject)Instantiate(prefabObjects[i], transform);
            go.name = deleteGameObjects[i].name;

            // Relink the prefab
            PrefabUtility.ConnectGameObjectToPrefab(go, (GameObject)prefabObjects[i]);

            deleteGameObjects[i].transform.parent = null;
            Destroy(deleteGameObjects[i]);
        }
#endif
    }
}