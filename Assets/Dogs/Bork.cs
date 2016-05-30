using UnityEngine;
using System.Collections;

public class Bork : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
        _particleCount = 30;

        _lifeTime = 0.0f;
        _maxLifeTime = 1.0f;


        // Sanity check:

        // Make sure the particle system doesn't emit something
        // until after it is dead
        GetComponent<ParticleSystem>().Emit(_particleCount);
    }
	
	// Update is called once per frame
	void Update ()
    {
        //
        // Fade the text out as it dies
        //
        TextMesh tm = GetComponent<TextMesh>();
        Color newColour = tm.color;
        newColour.a = 1.0f - _lifeTime / _maxLifeTime;
        tm.color = newColour;


        //
        // When it's around long enough, kill it
        //
        _lifeTime += Time.deltaTime;
        if( _lifeTime > _maxLifeTime )
        {
            Destroy(gameObject);
        }
	}

    int _particleCount;

    float _lifeTime;
    float _maxLifeTime;
}
