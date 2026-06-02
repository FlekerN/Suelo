using TMPro;
using UnityEngine.Windows;
using UnityEngine;


public class Animaciones : MonoBehaviour
{
    public Animator anim;
    private IPlayerInput input;

    bool isrunning = false;

    void Start()
    {
        input = GetComponent<IPlayerInput>(); // DIP en acción
    }

    // Update is called once per frame
    void Update()
    {
        if (input == null) { return; }


        anim.SetFloat("walkX", input.MoveInput.x, 0.1f, Time.deltaTime);
        anim.SetFloat("walkY", input.MoveInput.y, 0.05f, Time.deltaTime);


        if (isrunning != input.isRunning)
        {
            isrunning = input.isRunning;
            if (input.isRunning)
            {
                anim.CrossFade("Run", 0.2f);
            }
            else
            {
                anim.CrossFade("Walk", 0.2f);
            }
        }


    }
}