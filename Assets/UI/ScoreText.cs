using UnityEngine;
using System.Collections;

public class ScoreText : MonoBehaviour
{

	// Use this for initialization
	void Start ()
    {
        _upSpeed = 2.0f;
        _scaleSpeed = 0.1f;
        _timeTillDeath = 1.0f;
        _aliveTime = 0.0f;
    }
	
	// Update is called once per frame
	void Update ()
    {
        //
        // Scale it and move it up a little bit
        //
        float deltaScale = _scaleSpeed * Time.deltaTime;
        transform.position += new Vector3(0.0f, _upSpeed * Time.deltaTime, 0.0f);
        transform.localScale += new Vector3( deltaScale, deltaScale, deltaScale);
        
        //
        // Fade out the text
        //
        TextMesh tm = GetComponent<TextMesh>();
        Color newColour = tm.color;
        newColour.a = 1.0f - _aliveTime / _timeTillDeath;
        tm.color = newColour;

        //
        // Make it more alive until it dies
        //
        _aliveTime += Time.deltaTime;
        if (_aliveTime > _timeTillDeath )
        {
            Destroy(gameObject);
        }
	}

    float _upSpeed;
    float _scaleSpeed;
    float _timeTillDeath;
    float _aliveTime;
}
