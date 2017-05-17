#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class PreviewTest : MonoBehaviour {

    public List<GameObject> prefab = new List<GameObject>();

	// Use this for initialization
	void Start () {
        foreach (GameObject prefab in prefab)
        {
            print(prefab.name);
            Texture2D tex = AssetPreview.GetAssetPreview(prefab);
            byte[] bytes = tex.EncodeToPNG();
            File.WriteAllBytes(Application.dataPath + "/Images/Furniture/" + prefab.name + ".png", bytes);
            bytes = null;
        }
        AssetDatabase.Refresh();
	}
	
	// Update is called once per frame
	void Update () {
	    
	}
}
#endif