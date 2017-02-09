using UnityEngine;
using System.Collections;


public class AnimationTest : MonoBehaviour {

    const string clipDirectory = "Animation/";
    const string clipName = "Falling";
    Animation anim;
    AnimationClip clip;
    [SerializeField] int tick = 0;
    
	// Use this for initialization
	void Start () {
        clip = (AnimationClip)AnimationClip.Instantiate(Resources.Load(clipDirectory+clipName));
        anim = GetComponent<Animation>();
        anim.AddClip(clip, clipName);
	}
	
	// Update is called once per frame
	void Update () {

        if (tick++ % 200 == 0)
        {
            anim.Play(clipName);
        }
        /*
        if ((tick - 30) % 200 == 0)
            anim.Stop();
        */
    }
}
