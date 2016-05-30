using UnityEngine;

public class Dag : MonoBehaviour
{
    public GameObject textPrototype;
    public GameObject borkPrototype;

    // Use this for initialization
    void Start()
    {
        //
        // Score
        //
        _score = 0.0f;
        _scoreMultiplier = 0.0f;
        _multiplierIncrement = 1f;

        //
        // BOrking
        //
        _timeBetweenBorks = 1.0f;
        _timeSinceLastBork = 0.0f;

        //
        // Bouncing and collision
        //
        _nextBounceSpeed = 5.0f;
        _bounceIncrement = 1.0f;
        _horizontalImpulse = 5.0f;
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

                GameObject bork = GameObject.Instantiate(borkPrototype);
                bork.transform.position = pos;

                addScore(100, transform.position + new Vector3(1, 1));
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
        GameObject.Find("Score").GetComponent<TextMesh>().text =
            _score.ToString() + "\n" +
            GetMultiplierString();
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
            else
            {
                resetMultiplier();
            }
        }
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            // Do something when you hit an obstacle
        }
        if (collision.gameObject.CompareTag("Pickup"))
        {
            ParticleSystem ps = collision.gameObject.GetComponent<ParticleSystem>();
            ps.Emit(40);
            EmitText("Collected!", collision.gameObject.transform.position);
        }
    }

    void OnTriggerEnter2D( Collider2D other)
    {
        addScore( 100, other.transform.position );
        Destroy(other.gameObject);
    }


    //
    // Score manipulation
    //
    public void resetMultiplier()
    {
        _scoreMultiplier = 1.0f;
    }
    public void incrementMultiplier()
    {
        _scoreMultiplier += _multiplierIncrement;
        EmitText( GetMultiplierString(), transform.position + new Vector3(1, -1));
    }
    public void addScore(float score, Vector3 where )
    {
        float multipliedScore = score * _scoreMultiplier ;
        _score += multipliedScore;
        EmitText(multipliedScore.ToString(), where );
    }

    /// <summary>
    /// Emits a string with the text at pos
    /// </summary>
    /// <param name="str">The string to output</param>
    /// <param name="pos">Where to emit the text</param>
    public void EmitText(string str, Vector3 pos )
    {
        pos.z -= 0.2f;
        GameObject textInstance = Instantiate(textPrototype);
        textInstance.transform.position = pos;
        textInstance.GetComponent<TextMesh>().text = str;
    }

    private string GetMultiplierString()
    {
        return "<size=32>x</size>" + _scoreMultiplier.ToString();
    }

    float _score;
    float _scoreMultiplier;
    float _multiplierIncrement;

    float _timeBetweenBorks;
    float _timeSinceLastBork;

    float _nextBounceSpeed;
    float _bounceIncrement;
    float _horizontalImpulse;
    //bool _hadCollision;

    float _timeToHit;
    float _timeSinceHit;
}
