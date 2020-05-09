using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyParticles : MonoBehaviour
{
	public ParticleSystem particles;

	void Start()
	{
		StartCoroutine(DestroyIn(particles.main.duration));
	}

	private IEnumerator DestroyIn(float seconds)
	{
		yield return new WaitForSeconds(seconds);

		Destroy(gameObject);
	}
}