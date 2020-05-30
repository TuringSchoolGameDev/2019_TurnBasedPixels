using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
	public float speed;
	private float distance;

	private Vector3 endPosition;
	private Action callback;

	public void Init(Vector3 startPosition, Vector3 endPosition, Action callback)
	{
		gameObject.transform.position = startPosition;

		Vector3 targetRotation;
		targetRotation.x = endPosition.x - startPosition.x;
		targetRotation.y = endPosition.y - startPosition.y;
		float angle = Mathf.Atan2(targetRotation.y, targetRotation.x) * Mathf.Rad2Deg;
		transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

		this.endPosition = endPosition;
		distance  = Vector3.Distance(endPosition, gameObject.transform.position);
		this.callback = callback;

		if (AudioManager.instance != null)
		{
			AudioManager.instance.PlayAudio(AudioManager.instance.shootAudio);
		}
	}

	private void Update()
	{
		gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, endPosition, speed * Time.deltaTime);

		float newDistance = Vector3.Distance(endPosition, gameObject.transform.position);
		if (newDistance >= distance)
		{
			if (AudioManager.instance != null)
			{
				AudioManager.instance.PlayAudio(AudioManager.instance.collisionAudio);
			}
			callback?.Invoke();
			Destroy(gameObject);
		}
		else
		{
			distance = newDistance;
		}
	}
}
