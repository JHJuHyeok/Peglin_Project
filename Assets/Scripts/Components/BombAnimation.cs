using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombAnimation : MonoBehaviour
{
    private static readonly int BC = Animator.StringToHash("breakCount");
    private Animator anim;
    private PegBase peg;

    private void Start()
    {
        anim = GetComponent<Animator>();
        peg = GetComponent<PegBase>();
    }

    private void Update()
    {
        anim.SetInteger(BC, peg.count);
    }
}
