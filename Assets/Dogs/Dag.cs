using UnityEngine;
using System.Collections;

public class Dag : MonoBehaviour {

    public void resetMultiplier()
    {
        _scoreMultiplier = 0.0f;
    }
    public void incrementMultiplier()
    {
        _scoreMultiplier += _multiplierIncrement;
    }
    public void addScore(float score)
    {
        _score += score * (1.0f + _scoreMultiplier);
        EmitText(score.ToString());
    }

    public void EmitText( string str )
    {
        GameObject textInstance = Instantiate(textPrototype);
        //textInstance.transform.position = transform.position;
        //textInstance.GetComponent<Text>().Text = text;
    }

    // Use this for initialization
    void Start () {
        _score = 0;
        _scoreMultiplier = 0;
        _multiplierIncrement = 0.1f;
    }
	
	// Update is called once per frame
	void Update () {
        float horizImpulse = Time.deltaTime * horizontalImpulse;
	    if( Input.GetKey(KeyCode.LeftArrow) )
        {
            GetComponent<Rigidbody2D>().velocity -= new Vector2(horizImpulse,0);
        }
        if(Input.GetKey(KeyCode.RightArrow) )
        {
            GetComponent<Rigidbody2D>().velocity += new Vector2(horizImpulse,0);
        }

        if( Input.GetKeyDown(KeyCode.Space) )
        {
            if(timeSinceLastBork > timeBetweenBorks )
            {
                timeSinceLastBork = 0;
                Debug.Log("Bork!");
                Vector3 pos = this.GetComponentInChildren<Transform>().position;
                GameObject bork = GameObject.Instantiate(borkPrototype);
                bork.transform.position = pos;
            }
        }
        timeSinceLastBork += Time.deltaTime;

        //
        // Follow the dog
        //
        Vector3 cameraPosition = Camera.main.transform.position;
        float lockSpeed = nextBounceSpeed / 5.0f;
        cameraPosition = Vector3.Lerp(cameraPosition, transform.position, Time.deltaTime * lockSpeed);
        cameraPosition.z = -5; // always make it -5
        Camera.main.transform.position = cameraPosition;
        //
        // Zoom out as he goes higher
        //
        Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, 4 + Mathf.Log10(nextBounceSpeed), Time.deltaTime); 
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if( collision.gameObject.name == "Trampoline" )
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(0, nextBounceSpeed);
            if (!hadCollision)
            {
                nextBounceSpeed += bounceIncrement;
            }
            hadCollision = false;
        }
        if (collision.gameObject.CompareTag("Obstacle") )
        {
            hadCollision = true;
        }
    }

    

    // 
    private float _score;
    private float _scoreMultiplier;
    private float _multiplierIncrement;
    public GameObject textPrototype;

    //TODO: _ify
    //UNUSED private float cameraFollowSpeed = 3;

    public GameObject borkPrototype;
    //TODO: _ify
    private float timeBetweenBorks = 3;
    //TODO: _ify
    private float timeSinceLastBork = 0;

    public float nextBounceSpeed = 5;
    //TODO: _ify
    private float bounceIncrement = 1;
    //TODO: _ify
    private float horizontalImpulse = 5;
    //TODO: _ify
    private bool hadCollision = false;
}
