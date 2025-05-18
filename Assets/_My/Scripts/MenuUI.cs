using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuUI : MonoBehaviour
{
    public void OnClickStart()
    {
        Debug.Log("Start ¹öÆ° Å¬¸¯µÊ");
        SceneManager.LoadScene("PlayScene");
    }

    public void OnClickRestart()
    {
        Time.timeScale = 1f;  
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); 
    }

    public void OnClickExit()
    {
        Debug.Log("Exit ¹öÆ° Å¬¸¯µÊ");
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}