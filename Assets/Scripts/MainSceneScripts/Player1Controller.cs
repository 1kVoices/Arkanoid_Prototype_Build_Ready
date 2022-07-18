using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;


public class Player1Controller : MonoBehaviour
{
    private Rigidbody rb;

    [SerializeField]
    private float speed;

    [SerializeField]
    private GameObject _pauseMenu;

    [SerializeField]
    private Canvas _canvas;

    [SerializeField]
    private Transform _ball;

    public bool _autoPlay;

    [HideInInspector]
    public Canvas myCanvas;

    [SerializeField]
    private Reflect _reflect;

    [HideInInspector]
    public bool input = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        myCanvas = Instantiate(_canvas);

        myCanvas.enabled = false;
    }

    void Update()
    {
        var texts = myCanvas.GetComponentsInChildren<Text>();

        foreach (var t in texts)
        {
            t.text = _ball.GetComponent<Reflect>().Health.ToString();
        }

        if (_autoPlay)
        {
            Vector3 _ballPos = new Vector3(_ball.position.x, _ball.position.y, transform.position.z);

            transform.position = Vector3.Lerp(transform.position, _ballPos, Time.deltaTime * 20f);
        }

        if (input) return;

        if (!_autoPlay)
        {
            if (rb.velocity.magnitude > 6)
            {
                rb.velocity = Vector3.ClampMagnitude(rb.velocity, 6f);
            }
#if UNITY_STANDALONE
            if (Input.GetKey(KeyCode.W))
            {
                rb.AddForce(Vector3.up * speed, ForceMode.Acceleration);
            }

            if (Input.GetKey(KeyCode.S))
            {
                rb.AddForce(Vector3.down * speed, ForceMode.Acceleration);
            }

            if (Input.GetKey(KeyCode.D))
            {
                rb.AddForce(-Vector3.right * speed, ForceMode.Acceleration);
            }

            if (Input.GetKey(KeyCode.A))
            {
                rb.AddForce(Vector3.right * speed, ForceMode.Acceleration);
            }
#endif
#if UNITY_EDITOR
            if (Input.GetKey(KeyCode.UpArrow))
            {
                rb.AddForce(Vector3.up * speed, ForceMode.Acceleration);
            }

            if (Input.GetKey(KeyCode.DownArrow))
            {
                rb.AddForce(Vector3.down * speed, ForceMode.Acceleration);
            }

            if (Input.GetKey(KeyCode.RightArrow))
            {
                rb.AddForce(-Vector3.right * speed, ForceMode.Acceleration);
            }

            if (Input.GetKey(KeyCode.LeftArrow))
            {
                rb.AddForce(Vector3.right * speed, ForceMode.Acceleration);
            }
#endif
        }

        if (Input.GetKey(KeyCode.Escape))
        {
            _pauseMenu.SetActive(true);

            StartCoroutine(Lerp(_pauseMenu.transform, Vector3.one, 0.3f));

            Time.timeScale = 0;

            input = true;
        }
    }

    public void StartGame()
    {
        myCanvas.enabled = true;

        myCanvas.targetDisplay = 0;

        _autoPlay = false;

        _reflect.StartCoroutine(_reflect.Timer());
        _reflect.StartMoveCubes();
        input = false;
    }

    private IEnumerator Lerp(Transform obj, Vector3 target, float TravelTime)
    {
        Vector3 startPosition = obj.localScale;

        float t = 0;

        foreach (var child in _pauseMenu.GetComponentsInChildren<Button>())
        {
            child.interactable = false;
        }

        foreach (var child in _pauseMenu.GetComponentsInChildren<Toggle>())
        {
            child.interactable = false;
        }

        while (t < 1)
        {
            obj.localScale = Vector3.Lerp(startPosition, target, t * t);

            t += Time.unscaledDeltaTime / TravelTime;
           
            yield return null;
        }

        obj.localScale = target;

        foreach (var child in _pauseMenu.GetComponentsInChildren<Button>())
        {
            child.interactable = true;
        }

        foreach (var child in _pauseMenu.GetComponentsInChildren<Toggle>())
        {
            child.interactable = true;
        }
    }
}
