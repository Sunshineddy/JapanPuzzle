/***********************************************************************

オブジェクトを透明にする方法
http://tsubakit1.hateblo.jp/entry/2015/09/07/222755
オブジェクトのシェーダ設定で"Rendering Mode"を"Fade"にすると
GetComponent<Renderer>().material.colorのアルファ値に従って透明になる

スクリプトでテクスチャを貼る
http://dennou-note.blogspot.jp/2014/01/unity_20.html
(1)  "Assets/Resources/"以下にテクスチャを置く(Resourcesフォルダがない場合は作成)
(2)  Resources.Loadの引数にテクスチャのResources/以降のパスを文字列として与え、戻り値をTexture2D型変数に格納
(3)  GetComponent<Renderer>().material.mainTextureに(2)の変数を代入

************************************************************************/


using UnityEngine;
using System.Collections;

public class changeTexture : MonoBehaviour {

    const string texname_creeper = "Texture/creeper";
    const string texname_Xplogo = "Texture/Xplogo";

    Texture2D tex_creeper;
    Texture2D tex_Xplogo;
    
	// Use this for initialization
	void Start () {
        // 初期状態の色
        GetComponent<Renderer>().material.color = Color.white;
        
        tex_creeper = Resources.Load<Texture2D>(texname_creeper);
        tex_Xplogo = Resources.Load<Texture2D>(texname_Xplogo);
        //GetComponent<Renderer>().material.mainTexture = tex_creeper;

        changeTex2("Texture/moai");
    }
	
	// Update is called once per frame
	void Update () {
        //changeTex();
        clearTexture();
    }
    

    // そのパスにあるテクスチャに貼りかえる
    public void changeTex2(string tex_path) {
        GetComponent<Renderer>().material.mainTexture = Resources.Load<Texture2D>(tex_path);
    }

    // spaceを押している間オブジェクトのテクスチャが変わる
    void changeTex()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            GetComponent<Renderer>().material.mainTexture = tex_creeper;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            GetComponent<Renderer>().material.mainTexture = tex_Xplogo;
        }
    }

    // spaceを押すとオブジェクトを透明化する
    void clearTexture() {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            // 白にするとテクスチャが見える (テクスチャの色と乗算になるから)
            GetComponent<Renderer>().material.color = Color.white;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        { 
            // オブジェクトを透明にする
            GetComponent<Renderer>().material.color = Color.clear;
        }
    }
    
}
