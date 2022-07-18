using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class PauseController : MonoBehaviour
{
    [SerializeField]
    private Button _resume;

    [SerializeField]
    private Transform _ball;

    [SerializeField]
    private Transform _player1;

    [SerializeField]
    private Transform _player2;

    [SerializeField]
    private List<GameObject> _obstacles;

    [SerializeField]
    private Image _died;

    [SerializeField]
    private Image _win;

    [SerializeField]
    private Reflect _reflect;

    private StreamWriter _writer;

    private string _address;

    private string _fileName = "Log";

    private void Start()
    {
        _address = Directory.GetCurrentDirectory().ToString() + @"\" + _fileName + ".txt";

        GameEvents.singleton.onLog += Write;
    }

    void Write(string toWrite)
    {
        if (_writer == null)
        {
            _writer = new StreamWriter(_address);
        }

        var line = $"{toWrite} {DateTime.Now}";

        _writer.WriteLine(line);
    }

    void OnDestroy()
    {
        if (_writer != null) _writer.Close();
    }

    private void Reset()
    {
        Time.timeScale = 1f;

        _reflect.firstTime = true;

        _player1.GetComponent<Player1Controller>().myCanvas.enabled = true;

        _died.gameObject.SetActive(false);

        _win.gameObject.SetActive(false);

        _player1.position = new Vector3(6, 0, 14.8f);

        _player1.GetComponent<Rigidbody>().velocity = Vector3.zero;

        _player2.position = new Vector3(6, 0, -14.8f);

        _player2.GetComponent<Rigidbody>().velocity = Vector3.zero;

        _ball.position = new Vector3(6, 0, 11.8f);

        _ball.GetComponent<Reflect>().Health = _ball.GetComponent<Reflect>().startHealth;

        _ball.GetComponent<Reflect>().timer = true;

        StartCoroutine(_ball.GetComponent<Reflect>().Timer());

        _ball.GetComponent<Reflect>().start = -transform.forward;

        _player1.GetComponent<Rigidbody>().velocity = Vector3.zero;

        foreach (var obstacle in _obstacles)
        {
            obstacle.SetActive(true);
        }
    }

    public void NewGame_EditorEvent()
    {
        StartCoroutine(Lerp(transform, Vector3.zero, 0.3f, false));
        Reset();
    }

    public void Resume_EditorEvent()
    {
        StartCoroutine(Lerp(this.transform, Vector3.zero, 0.1f, false));

        Time.timeScale = 1f;
    }

    public void AutoPlay_EditorEvent(bool b)
    {
        GameEvents.singleton.Log(b? "Player enabled AutoPlay at " : "Player disabled AutoPlay at ");
        _player1.GetComponent<Player1Controller>()._autoPlay = b;
    }

    public void YouDied()
    {
        _died.color = new Color32(255, 255, 255, 0);
        _died.gameObject.SetActive(true);
        Time.timeScale = 0;
        _player1.GetComponent<Player1Controller>().myCanvas.enabled = false;
        _player1.GetComponent<Player1Controller>().input = true;
        StartCoroutine(ColorLerp(_died, new Color32(255, 255, 255, 255), 1f));
    }

    public void YouWin()
    {
        _win.color = new Color32(255, 255, 255, 0);
        _win.gameObject.SetActive(true);
        Time.timeScale = 0f;
        _player1.GetComponent<Player1Controller>().myCanvas.enabled = false;
        _player1.GetComponent<Player1Controller>().input = true;
        StartCoroutine(ColorLerp(_win, new Color32(255, 255, 255, 255), 1f));
    }

    public void Exit_EditorEvent()
    {
        GameEvents.singleton.Log("Exit Game at ");
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
#if UNITY_STANDALONE
        Application.Quit();
#endif
    }

    private IEnumerator ColorLerp(Image img, Color target, float travelTime)
    {
        var time = 0f;

        while (time < 1f)
        {
            img.color = Color.Lerp(img.color, target, time * time);
            time += Time.unscaledDeltaTime / travelTime;
            yield return null;
        }
        img.color = target;

        yield return new WaitForSecondsRealtime(1f);

        yield return StartCoroutine(Lerp(transform, Vector3.one, 0.3f, true));
    }

    private IEnumerator Lerp(Transform obj, Vector3 target, float TravelTime, bool isDied)
    {
        _player1.GetComponent<Player1Controller>().input = false;

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
            if (isDied) if (child == _resume) continue;
            child.interactable = true;
        }

        foreach (var child in GetComponentsInChildren<Toggle>())
        {
            child.interactable = true;
        }
    }
}
