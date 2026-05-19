using UnityEngine;

public class RagdollAnimator : MonoBehaviour
{
    Animator anim;
    Rigidbody[] rbs;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        rbs = GetComponentsInChildren<Rigidbody>(true);

        EnableAnimator();

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void EnableRagdoll() 
    {
        anim.enabled = false;
        foreach (var rigidbody in rbs) 
        {
            rigidbody.isKinematic = false;
        }
    }
    public void EnableAnimator()
    {
        foreach (var rigidbody in rbs)
        {
            rigidbody.isKinematic = true;
        }
        anim.enabled = true;
    }
}
