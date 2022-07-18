using System.Collections;
using UnityEngine;

public class Reflect : MonoBehaviour
{
    private Rigidbody rb;

    [SerializeField] 
    private float _speed;

    [SerializeField] 
    private Transform _player1;

    [SerializeField]
    private Transform _ball;

    [SerializeField]
    public float Health;

    private GameObject[] obstacles;

    [HideInInspector]
    public Vector3 start;

    [HideInInspector]
    public float startHealth;

    [HideInInspector]
    public bool timer = true;

    [SerializeField]
    private PauseController _pauseController;

    [HideInInspector]
    public bool firstTime = true;

    void Start()
    {
        startHealth = Health;

        rb = GetComponent<Rigidbody>();

        _ball.position = _player1.transform.position + new Vector3(0, 0, -3f);

        start = -transform.forward;

        obstacles = GameObject.FindGameObjectsWithTag("Obstacle");
    }

    void Update()
    {
        if (timer) return;

        _ball.Translate(start * _speed * Time.deltaTime);

        if (_speed > 12f)
        {
            _speed = 12f;
        }

        if (Health == 0)
        {
            if (!firstTime) return;
            firstTime = false;

            _pauseController.YouDied();

            Debug.Log("Game Over");
#if UNITY_EDITOR
            //UnityEditor.EditorApplication.isPaused = true;
#endif
        }

        obstacles = GameObject.FindGameObjectsWithTag("Obstacle");

        if (obstacles.Length == 0)
        {
            if (!firstTime) return;
            firstTime = false;

            _pauseController.YouWin();
            Debug.Log("Game Over");
#if UNITY_EDITOR
            //UnityEditor.EditorApplication.isPaused = true;
#endif
        }
    }

    public void StartMoveCubes()
    {
        foreach (var obstacle in obstacles)
        {
            StartCoroutine(StartRandomLerp(obstacle.transform));
        }
    }

    void OnCollisionEnter(Collision collider)
    {
        //var speed = lastVel.magnitude;

        //var direction = Vector3.Reflect(lastVel.normalized, collider.contacts[0].normal);

        //rb.velocity = direction * Mathf.Max(speed, 0f);

        start = Vector3.Reflect(start, collider.contacts[0].normal);

        if (collider.transform.tag == "Obstacle")
        {
            _speed++;

            collider.gameObject.SetActive(false);
        }

        if(collider.transform.tag == "Dead")
        {
            Health--;

            Debug.Log("Ouch! Health = " + Health);
            
            StartCoroutine(PauseStart());
        }
    }

    private IEnumerator StartRandomLerp(Transform unit)
    {
        yield return new WaitForSeconds(1.6f);

        while (true)
        {
            yield return StartCoroutine(RandomLerp(unit.transform, new Vector3(Random.Range(11, 1), Random.Range(-3, 3), Random.Range(3, -3)), 1.4f));

            yield return new WaitForSeconds(0.1f);
        }
    }

    private IEnumerator RandomLerp(Transform obj, Vector3 target, float TravelTime)
    {
        Vector3 startPosition = obj.position;

        float t = 0f;

        while (t < 1)
        {
            obj.position = Vector3.Lerp(startPosition, target, t * t);

            t += Time.deltaTime / TravelTime;

            yield return null;
        }
    }
    
    public IEnumerator PauseStart()
    {
        _speed = 7f;

        _ball.position = _player1.transform.position + new Vector3(0, 0, -3f);

        start = Vector3.zero;

        yield return new WaitForSeconds(1.5f);

        start = -transform.forward;
    }

    public IEnumerator Timer()
    {
        yield return new WaitForSecondsRealtime(1.3f);

        timer = false;
    }
}
