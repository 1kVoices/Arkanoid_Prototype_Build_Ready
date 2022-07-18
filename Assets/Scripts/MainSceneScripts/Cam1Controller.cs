using UnityEngine;

public class Cam1Controller : MonoBehaviour
{
    [SerializeField]
    private Animator _animator;

    [SerializeField]
    private GameObject _cube;

    [SerializeField]
    private Player1Controller _controller;

    void Start()
    {
        _animator.SetTrigger("Start");
        _controller.input = true;
    }

    void DeactivateFirstCube()
    {
        _cube.SetActive(false);
        GameEvents.singleton.Log("Start Game at ");
    }

    void OnEndAnimation()
    {
        _controller.StartGame();
    }
}
