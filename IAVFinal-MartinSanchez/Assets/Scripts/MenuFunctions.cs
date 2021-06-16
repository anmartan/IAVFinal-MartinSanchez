using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;
using System.IO;

/// <summary>
/// Callbacks for the main menu.
/// </summary>
public class MenuFunctions : MonoBehaviour
{
    private enum Menus { MainMenu = 0, PlayMenu, CreditsMenu, QuitMenu };

    [SerializeField] private GameObject credits_;   // the menus that will be activated and deactivated
    [SerializeField] private GameObject main_;      // the menus that will be activated and deactivated
    [SerializeField] private GameObject play_;      // the menus that will be activated and deactivated
    [SerializeField] private GameObject quit_;      // the menus that will be activated and deactivated

    [SerializeField] private Dropdown algorithm_;   // the dropdown with the algorithm options
    [SerializeField] private Dropdown map_;         // the dropdown with the map options

    private bool creditsBool_ = false;              // whether the player is seeing the credits
    private bool playBool_ = false;                 // whether the player is seeing the play menu
    private bool quitBool_ = false;                 // whether the player is seeing the quit menu
    private string filename_;                       // the file that will be loaded later on
    private int solver_;                            // the solver algorithm that will be used
    
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

    /// <summary>
    /// Saves the configuration chosen and changes the scene.
    /// </summary>
    public void PlayGame()
    {
        solver_ =   algorithm_.value;                   // loads the values the player set
        filename_ = Configuration.MAZES_[map_.value];   // and saves them in the file

        // writes a file with the config
        StreamWriter file = new StreamWriter(Application.dataPath + "/Maps/" + Configuration.CONFIG_FILE_);
        file.WriteLine(solver_);
        file.WriteLine(filename_);
        file.Close();

        SceneManager.LoadScene("IAVFinalScene");  // loads the scene with the maze
    }


    //----------------------- Getters ----------------------//
    public bool IsAtPlayMenu() { return playBool_; }
    public bool IsAtCreditsMenu() { return creditsBool_; }
    public bool IsAtQuitMenu() { return quitBool_; }
    
}
