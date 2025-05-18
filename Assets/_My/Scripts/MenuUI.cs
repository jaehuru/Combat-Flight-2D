using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuUI : MonoBehaviour
{
    public void OnClickStart()
    {
        Debug.Log("Start ��ư Ŭ����");
        SceneManager.LoadScene("PlayScene");
    }

    public void OnClickRestart()
    {
        Time.timeScale = 1f;  // ����� ���� �ð� �ٽ� �帣��
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);  // ���� �� ��ε�
    }

    public void OnClickExit()
    {
        Debug.Log("Exit ��ư Ŭ����");
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}