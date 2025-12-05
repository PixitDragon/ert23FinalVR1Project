using UnityEngine;

public class aaaaaaa : MonoBehaviour
{
    private Animation anim;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anim = GetComponent<Animation>();
        //PlayAttack();
    }

    public void PlayAttack()
    {
        anim.Play("taunt");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
