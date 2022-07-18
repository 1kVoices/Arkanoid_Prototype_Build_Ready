using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class CamController : MonoBehaviour
{
    [SerializeField]
    private Animator animator;

    [SerializeField]
    private StartMenu _startMenu;

    private void Start()
    {
        GameEvents.singleton.onTrigger += OnTrigger;
    }

    private void OnTrigger()
    {
        animator.SetTrigger("Back");
        Invoke(nameof(StartAnotherAnim), 2.6f);
    }

    void StartAnotherAnim()
    {
        animator.SetTrigger("Forward");
    }

    void StartMenuAnim()
    {
        _startMenu.StartCoroutine(_startMenu.Lerp(_startMenu.transform, Vector3.one, 0.3f));
    }

    public void StartFastAnim()
    {
        animator.SetTrigger("Fast");
    }

    public void SwitchScene()
    {
        _startMenu.scene1.allowSceneActivation = true;
    }
}
