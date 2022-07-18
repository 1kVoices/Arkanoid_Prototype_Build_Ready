using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;


public class Player2Controller : MonoBehaviour
{
    [SerializeField] private float speed;

    [SerializeField]
    private Canvas _canvas;

    [SerializeField]
    private Transform _ball;

    private Canvas myCanvas;

    void Start()
    {
        myCanvas = Instantiate(_canvas);

        myCanvas.targetDisplay = 1;
    }

    void Update()
    {
        var texts = myCanvas.GetComponentsInChildren<Text>();

        foreach (var t in texts)
        {
            t.text = _ball.GetComponent<Reflect>().Health.ToString();
        }

        Vector3 _ballPos = new Vector3(_ball.position.x, _ball.position.y, transform.position.z);

        transform.position = Vector3.Lerp(transform.position, _ballPos, Time.deltaTime * 20f);
    }
}
