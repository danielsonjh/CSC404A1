﻿using UnityEngine;

public class Raindrop : MonoBehaviour
{
    public const float AdhesionK = 20f;
    public const float CohesionK = 1f;
    public const float MergeThreshold = 0.1f;

    private const float ScaleScaler = 4;

    private Rigidbody _rigidbody;
    private Transform _cloth;

	void Start ()
	{
	    _rigidbody = GetComponent<Rigidbody>();
	    _cloth = GetComponentInChildren<Transform>();
	}

    void Update()
    {
        if (transform.position.y < -20f)
        {
            Destroy(gameObject);
        }
    }

	void FixedUpdate ()
	{
		_rigidbody.AddForce(new Vector3(0, Mathf.Min(GetAdhesionForce() + _rigidbody.mass * Physics.gravity.y, 0), 0));
	}

    void OnTriggerStay(Collider other)
    {
        if (other.attachedRigidbody && other.CompareTag("Raindrop"))
        {
            var dir = transform.position - other.transform.position;
            if (Controller.Instance.AttachedRaindrop == gameObject || 
                transform.localScale.x >= other.transform.localScale.x && 
                0.8 * (dir.magnitude + other.transform.localScale.x) <= transform.localScale.x && 
                Controller.Instance.AttachedRaindrop != other.gameObject)
            {
                transform.localScale += other.transform.localScale;
                _cloth.localScale = new Vector3(transform.localScale.x, _cloth.localScale.y, transform.localScale.z);
                _rigidbody.mass += other.attachedRigidbody.mass;
                Destroy(other.gameObject);
                _rigidbody.velocity = Vector3.zero;
            }
            else
            {
                other.attachedRigidbody.AddForce(dir * CohesionK / dir.sqrMagnitude);
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Window"))
        {
            _rigidbody.velocity = Vector3.zero;
            transform.position = new Vector3(transform.position.x, transform.position.y, other.transform.position.z);
        }
    }

    private float GetAdhesionForce()
    {
        return Mathf.Sqrt(_rigidbody.mass) * AdhesionK;
    }
}
