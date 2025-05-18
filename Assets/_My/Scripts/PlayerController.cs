using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Player player;
    private Vector2 moveInput;
    private bool isMobileLeftPressed = false;
    private bool isMobileRightPressed = false;

    private void Awake()
    {
        player = GetComponent<Player>();
    }

    public void OnMove(InputValue inputValue)
    {
        moveInput = inputValue.Get<Vector2>();
    }

    public void OnShoot(InputValue inputValue)
    {
        if (inputValue.isPressed)
            player.TryShoot();
    }

    void Update()
    {
#if UNITY_EDITOR || UNITY_STANDALONE
        float input = moveInput.x;
#else
        float input = 0;
        if (isMobileLeftPressed) input = -1;
        else if (isMobileRightPressed) input = 1;
#endif

        player.Move(input);
        player.TryShoot();  // 자동 공격
    }

    // 모바일 UI 버튼용 함수
    public void OnLeftDown() => isMobileLeftPressed = true;
    public void OnLeftUp() => isMobileLeftPressed = false;

    public void OnRightDown() => isMobileRightPressed = true;
    public void OnRightUp() => isMobileRightPressed = false;
}