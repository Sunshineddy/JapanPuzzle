/*
インスタンス生成時に引数を渡す
http://udasankoubou.blogspot.jp/2013/01/unity.html
 
http://qiita.com/2dgames_jp/items/495dc59c78930e284707

 */

using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

    // 忘れずにアタッチする
    public GameObject blockPrefab;

    changeTexture c;
    int tick = 0;
    string texPath = "Texture/moai";

	// Use this for initialization
	void Start () {
        GameObject ct = Instantiate(blockPrefab, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
        c = ct.GetComponent<changeTexture>();

	}
	
	// Update is called once per frame
	void Update () {
        tick++;
        if (tick == 100)
        {
            texPath = "Texture/creeper";
        }
        else if (tick == 200)
        {
            texPath = "Texture/Xplogo";
        }
        else if (tick == 300) {
            tick = 0;
            texPath = "Texture/moai";
        }
        c.changeTex2(texPath);
	}

    public int get_tick() {
        return tick;
    }

    public string get_texPath() {
        return texPath;
    }
}
