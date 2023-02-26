using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    Animation anim;
    public AnimationClip firstAnim;
    public GameObject leftArm;
    float timer = 0f;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animation>();
        firstAnim.name = "mixamo";
        firstAnim.legacy = true;
        anim.AddClip(firstAnim, firstAnim.name);
        anim.clip = firstAnim;
    }

    // Update is called once per frame
    void Update()
    {
        timer = timer + 1.0f;
    }

    public void playAnimation()
    {
        anim.Play(firstAnim.name);
    }

    public void pauseAnimation()
    {
        anim.Stop();
        //leftArm.transform.rotation = Quaternion.Euler(0, 5, 0);
    }
}
