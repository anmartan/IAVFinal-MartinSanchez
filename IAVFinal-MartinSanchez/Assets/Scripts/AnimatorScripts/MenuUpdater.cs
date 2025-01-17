﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Updates the variables the animator needs (in the main menu)
/// </summary>
public class MenuUpdater : MonoBehaviour
{
    [SerializeField] private Canvas canvas_;

    MenuFunctions callbacks_;
    private Animator animator_;

    private int creditsHash_ = Animator.StringToHash("Credits_");   // this is faster than comparing strings every update
    private int exitHash_ = Animator.StringToHash("Exit_");
    private int playHash_ = Animator.StringToHash("Play_");

    private void Start()
    {
        animator_ = GetComponent<Animator>();
        callbacks_ = canvas_.GetComponent<MenuFunctions>();
    }

    void Update()
    {
        animator_.SetBool(playHash_, callbacks_.IsAtPlayMenu());
        animator_.SetBool(exitHash_, callbacks_.IsAtQuitMenu());
        animator_.SetBool(creditsHash_, callbacks_.IsAtCreditsMenu());
    }
}
