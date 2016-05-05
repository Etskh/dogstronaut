using UnityEngine;

public class Dag : MonoBehaviour
{
    // Use this for initialization
    void Start()
    {
        _score = 0.0f;
        _scoreMultiplier = 0.0f;
        _multiplierIncrement = 1f;

        _timeBetweenBorks = 1.0f;
        _timeSinceLastBork = 0.0f;

        _nextBounceSpeed = 5.0f;
        _bounceIncrement = 1.0f;
        _horizontalImpulse = 5.0f;
        _hadCollision = false;

        _timeSinceHit = float.MaxValue;
        _timeToHit = 0.5f;
    }

    // Update is called once per frame
    void Update()
    {
        float horizImpulse = Time.deltaTime * _horizontalImpulse;
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            GetComponent<Rigidbody2D>().velocity -= new Vector2(horizImpulse, 0);
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            GetComponent<Rigidbody2D>().velocity += new Vector2(horizImpulse, 0);
        }

        if (Input.GetKey(KeyCode.Space))
        {
            if (_timeSinceLastBork > _timeBetweenBorks)
            {
                _timeSinceLastBork = 0;
                Vector3 pos = this.GetComponentInChildren<Transform>().position;

                //GameObject bork = GameObject.Instantiate(borkPrototype);
                //bork.transform.position = pos;

                addScore(100);
            }
        }
        _timeSinceLastBork += Time.deltaTime;

        //
        // Hit the jump right before landing to increase the multiplier
        //
        if( Input.GetButtonDown("Vertical"))
        {
            _timeSinceHit = 0.0f;
        }
        _timeSinceHit += Time.deltaTime;

        //
        // Follow the dog
        //
        Vector3 cameraPosition = Camera.main.transform.position;
        float lockSpeed = _nextBounceSpeed / 5.0f;
        cameraPosition = Vector3.Lerp(cameraPosition, transform.position, Time.deltaTime * lockSpeed);
        cameraPosition.z = -5; // always make it -5
        Camera.main.transform.position = cameraPosition;

        //
        // Zoom out as he goes higher
        //
        Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, 4 + Mathf.Log10(_nextBounceSpeed), Time.deltaTime);


        //
        // Update the score
        //
        GameObject.Find("Score").GetComponent<TextMesh>().text = _score.ToString() + "\n<size=48>x</size> " + _scoreMultiplier.ToString();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Trampoline")
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(0, _nextBounceSpeed);
            if ( _timeSinceHit < _timeToHit )
            {
                incrementMultiplier();
                _nextBounceSpeed += _bounceIncrement;
            }
            _hadCollision = false;
        }
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            _hadCollision = true;
        }
    }

    public void resetMultiplier()
    {
        _scoreMultiplier = 0.0f;
    }
    public void incrementMultiplier()
    {
        _scoreMultiplier += _multiplierIncrement;
        EmitText("<size=48>x</size> " + _scoreMultiplier.ToString(), transform.position + new Vector3(1, -1));
    }
    public void addScore(float score)
    {
        float multipliedScore = score * _scoreMultiplier ;
        _score += multipliedScore;
        EmitText(multipliedScore.ToString(), transform.position + new Vector3(1,1));
    }

    public void EmitText(string str, Vector3 pos )
    {
        pos.z -= 0.2f;
        GameObject textInstance = Instantiate(textPrototype);
        textInstance.transform.position = pos;
        textInstance.GetComponent<TextMesh>().text = str;
    }

    private float _score;
    private float _scoreMultiplier;
    private float _multiplierIncrement;

    public GameObject textPrototype;
    public GameObject borkPrototype;

    private float _timeBetweenBorks;
    private float _timeSinceLastBork;

    public float _nextBounceSpeed;
    private float _bounceIncrement;
    private float _horizontalImpulse;
    private bool _hadCollision;

    private float _timeToHit;
    private float _timeSinceHit;
}
