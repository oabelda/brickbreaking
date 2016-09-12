using UnityEngine;
using System.Collections;

public class PowerUp : MonoBehaviour {

	public PowerUpType powerUpType;

	[Range(1,float.PositiveInfinity)]
	public float durationTime = 10.0f;

	public void makePowerUp (Paddle paddle, Ball ball, float powerUpIdentifier){
		PowerUpSpawner.makePowerUp (powerUpType,paddle,ball,durationTime, powerUpIdentifier);
	}

}
