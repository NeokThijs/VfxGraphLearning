using UnityEngine;
using UnityEngine.SceneManagement;

public class sceneSwitch : MonoBehaviour
{
    public void ToMain()
    {
        SceneManager.LoadScene("Main");
    }

    public void ToSpace()
    {
        SceneManager.LoadScene("SpaceShip");
    }
}
