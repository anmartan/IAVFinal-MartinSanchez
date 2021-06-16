using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuUpdater : MonoBehaviour
{
    [SerializeField] private Canvas canvas_;

    MainMenuFunctions callbacks_;
    private Animator animator_;

    private int creditsHash_ = Animator.StringToHash("Credits_");   // this is faster than comparing strings every update
    private int exitHash_ = Animator.StringToHash("Exit_");
    private int playHash_ = Animator.StringToHash("Play_");

    private void Start()
    {
        animator_ = GetComponent<Animator>();
        callbacks_ = canvas_.GetComponent<MainMenuFunctions>();
    }

    void Update()
    {
        animator_.SetBool(playHash_, callbacks_.IsAtPlayMenu());
        animator_.SetBool(exitHash_, callbacks_.IsAtQuitMenu());
        animator_.SetBool(creditsHash_, callbacks_.IsAtCreditsMenu());
    }
}
