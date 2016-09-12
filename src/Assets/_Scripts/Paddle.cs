using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Paddle : MonoBehaviour {

	public float paddleSpeed = 1f;

	private Vector3 playerPos = new Vector3 (0,-9.5f,0);

	public float boundarySize = 5f;

	private GameObject ball;
	
	public Vector3 lossyScale;

	private Dictionary<PowerUpType,float> powerUps = new Dictionary<PowerUpType, float>();

	void Start(){
		ball = gameObject.transform.FindChild ("Ball").gameObject;
		lossyScale = transform.lossyScale;
	}

	// Update is called once per frame
	void Update () 
	{
		float xPos = transform.position.x + (Input.GetAxis("Horizontal") * paddleSpeed);
		playerPos = new Vector3 (Mathf.Clamp (xPos,-boundarySize,boundarySize),-9.5f,0.0f);
		transform.position = playerPos;
	}

	void OnDestroy(){
		Destroy (ball);
	}

	void OnTriggerEnter (Collider col){
		if (col.tag == "PowerUp") {
			Destroy(col.gameObject);
			ManagePowerUp (col.gameObject.GetComponent<PowerUp>());
		}
	}

	void ManagePowerUp(PowerUp powerUp){
		if (powerUps.ContainsKey(powerUp.powerUpType)) return;
		float powerUpIdentifier = Random.value;
		powerUps.Add (powerUp.powerUpType,powerUpIdentifier);
		powerUp.makePowerUp(this,ball.GetComponent<Ball>(),powerUpIdentifier);
	}

	public void EndPowerUp (PowerUpType powerUp){
		powerUps.Remove (powerUp);
	}

	public float GetPowerUpIdentifier (PowerUpType powerUp){
		float powerUpIdentifier;
		if (powerUps.TryGetValue (powerUp, out powerUpIdentifier)) {
			return powerUpIdentifier;
		}
		return float.NaN;

	}
}
