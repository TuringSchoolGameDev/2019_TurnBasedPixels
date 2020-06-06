using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthHandler : MonoBehaviour
{
	public RectTransform healthBar;
	private float startingHealthBarWidth;
	private TileObject tileObject;
	public Text healthText;

	public Vector3 offset;
	private void Start()
	{
		startingHealthBarWidth = healthBar.sizeDelta.x;
	}

	public void Init(TileObject tileObject)
	{
		this.tileObject = tileObject;
		gameObject.transform.position = tileObject.transform.position + offset;
	}
	private void Update()//kiekvienam kadre mums atlieka kazkoki koda
	{
		if (tileObject == null)
		{
			Destroy(gameObject);
			return;
		}

		Vector2 laikinasKintamasis = healthBar.sizeDelta;
		laikinasKintamasis.x = startingHealthBarWidth * ((float)tileObject.health / tileObject.startingHealth);
		healthBar.sizeDelta = laikinasKintamasis;

		gameObject.transform.position = tileObject.transform.position + offset;

		healthText.text = tileObject.health.ToString();
	}
}
