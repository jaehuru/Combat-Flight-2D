using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuUI : MonoBehaviour
{
    public void OnClickStart()
    {
        Debug.Log("Start 버튼 클릭됨");
        SceneManager.LoadScene("PlayScene");
    }

    public void OnClickRestart()
    {
        Time.timeScale = 1f;  // 멈췄던 게임 시간 다시 흐르게
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);  // 현재 씬 재로드
    }

    public void OnClickExit()
    {
        Debug.Log("Exit 버튼 클릭됨");
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}