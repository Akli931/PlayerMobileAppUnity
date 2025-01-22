using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneNavigator : MonoBehaviour
{
    public void GoToStateScene()
    {
        SceneManager.LoadScene("StateScene");
    }

    public void GoToCommentsScene()
    {
        SceneManager.LoadScene("CommentsScene");
    }
}
