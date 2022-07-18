using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    [SerializeField]
    private Button _startGame;

    [SerializeField]
    private Button _settings;

    [SerializeField]
    private Button _exit;

    [SerializeField]
    private Image _notWoking;

    private Coroutine _cor;

    [SerializeField]
    private CamController _camController;

    [HideInInspector]
    public AsyncOperation scene1;

    void Start()
    {
        scene1 = SceneManager.LoadSceneAsync(1);
        scene1.allowSceneActivation = false;
    }
    public void StartGame_EditorEvent()
    {
        StartCoroutine(Lerp(transform, Vector3.zero, 0.3f));
        _camController.StartFastAnim();
    }

    public void Settings_EditorEvent()
    {
        if (_cor != null) StopCoroutine(_cor);
        _cor = StartCoroutine(ColorLerp(_notWoking, new Color32(255, 255, 255, 255), 1f));
    }

    public void Exit_EditorEvent()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    public IEnumerator Lerp(Transform obj, Vector3 target, float TravelTime)
    {
        foreach (var child in this.GetComponentsInChildren<Button>())
        {
            child.interactable = false;
        }

        foreach (var child in this.GetComponentsInChildren<Toggle>())
        {
            child.interactable = false;
        }

        Vector3 startPosition = obj.localScale;

        float t = 0;

        while (t < 1)
        {
            obj.localScale = Vector3.Lerp(startPosition, target, t * t);

            t += Time.unscaledDeltaTime / TravelTime;

            yield return null;
        }

        obj.localScale = target;

        foreach (var child in GetComponentsInChildren<Button>())
        {
            child.interactable = true;
        }

        foreach (var child in GetComponentsInChildren<Toggle>())
        {
            child.interactable = true;
        }
    }

    private IEnumerator ColorLerp(Image img, Color target, float travelTime) 
    {
        var time = 0f;

        while (time < 1f)
        {
            img.color = Color.Lerp(img.color, target, time * time);
            time += Time.deltaTime / travelTime;
            yield return null;
        }
        img.color = target;

        yield return new WaitForSeconds(1f);

        time = 0f;

        while (time < 1f)
        {
            img.color = Color.Lerp(img.color, new Color32(255, 255, 255, 0), time * time);
            time += Time.deltaTime / travelTime;
            yield return null;
        }
    }
}
