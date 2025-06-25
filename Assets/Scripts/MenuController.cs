using UnityEngine;
using UnityEngine.SceneManagement;


public enum PanelType
{
    None,
    Main,
    Option,
    Credit,
}
public class MenuController : MonoBehaviour
{
    private GameManager manager;

    void Start()
    {
        this.manager = GameManager.instance;
    }

    public void OpenPanel()
    {

    }
    public void ChangeScene(string _sceneName)
    {
        print("Change Scene");
        manager.ChangeScene(_sceneName);
    }

    public void Quit()
    {
        manager.Quit();
    }
}
