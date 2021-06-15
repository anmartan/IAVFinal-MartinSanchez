using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

/// <summary>
/// Callbacks for the main menu.
/// </summary>
public class MainMenuFunctions : MonoBehaviour
{
    private enum Menus { MainMenu = 0, PlayMenu, CreditsMenu, QuitMenu };

    [SerializeField] private GameObject main_;
    [SerializeField] private GameObject play_;
    [SerializeField] private GameObject credits_;
    [SerializeField] private GameObject quit_;
    [SerializeField] private Dropdown dropdown_;

    
    [HideInInspector] public bool playBool_ = false;
    [HideInInspector] public bool creditsBool_ = false;
    [HideInInspector] public bool quitBool_ = false;

    private GameManager.Solvers solver_;
    
    /// <summary>
    /// Deactivates all the menus and only activates the right one
    /// </summary>
    /// <param name="id"></param>
    private void ChooseMenu(Menus id)
    {
        main_.SetActive(false);
        play_.SetActive(false);
        credits_.SetActive(false);
        quit_.SetActive(false);

        switch(id)
        {
            case Menus.PlayMenu:
                play_.SetActive(true);
                playBool_ = true;
                break;
            case Menus.CreditsMenu:
                credits_.SetActive(true);
                creditsBool_ = true;
                break;
            case Menus.QuitMenu:
                quit_.SetActive(true);
                quitBool_ = true;
                break;
            default:
                main_.SetActive(true);
                playBool_ = false;
                creditsBool_ = false;
                quitBool_ = false;
                break;
        }
    }


    //------------------ Button callbacks ------------------//

    public void ToPlayMenu() { ChooseMenu(Menus.PlayMenu); }
    public void ToCreditsMenu() { ChooseMenu(Menus.CreditsMenu); }
    public void ToQuitMenu() { ChooseMenu(Menus.QuitMenu); }
    public void ToMainMenu() { ChooseMenu(Menus.MainMenu); }
    public void ExitGame() { Application.Quit(); }

    public void PlayGame()
    {
        solver_ = (GameManager.Solvers)dropdown_.value;
        // writes a file with the config

    }
}
