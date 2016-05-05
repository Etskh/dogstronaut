using UnityEngine;
using System.Collections;

public class ScoreText : MonoBehaviour
{

	// Use this for initialization
	void Start ()
    {
        _upSpeed = 1.0f;
        _scaleSpeed = 0.5f;
        _timeTillDeath = 1.0f;
        _aliveTime = 0.0f;

        _fadeOutColour = new Color(1, 1, 1, 0);
    }
	
	// Update is called once per frame
	void Update ()
    {
        float deltaScale = _scaleSpeed * Time.deltaTime;
        transform.position += new Vector3(0.0f, _upSpeed * Time.deltaTime, 0.0f);
        transform.localScale += new Vector3( deltaScale, deltaScale, deltaScale);

        TextMesh tm = GetComponent<TextMesh>();
        tm.color = Color.Lerp(tm.color, _fadeOutColour, Time.deltaTime * _timeTillDeath);
        _aliveTime += Time.deltaTime;

        if (_aliveTime > _timeTillDeath )
        {
            Destroy(gameObject);
        }
	}

    private float _upSpeed;
    private float _scaleSpeed;
    private float _timeTillDeath;
    private float _aliveTime;
    private Color _fadeOutColour;
}
