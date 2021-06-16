using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorUpdater : MonoBehaviour
{
    Animator animator_;
    Rigidbody rigidbody_;

    int velHash_ = Animator.StringToHash("Velocity_");      // this is faster than comparing strings every update

    private void Start()
    {
        animator_ = GetComponent<Animator>();
        rigidbody_ = GetComponent<Rigidbody>();
    }

    void Update()
    {
        float vel = rigidbody_.velocity.magnitude;

        animator_.SetFloat(velHash_, vel);
        
    }
}
