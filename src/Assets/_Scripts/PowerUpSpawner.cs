using UnityEngine;
using System.Collections;

[System.Serializable]
public class PowerUpSetting{
	[Range(0,100)]
	public int BigPaddle;
	[Range(0,100)]
	public int SmallPaddle;
	[Range(0,100)]
	public int ShooterPaddle;
	[Range(0,100)]
	public int SpeedBall;
	[Range(0,100)]
	public int SlowBall;
	[Range(0,100)]
	public int MultiBall;
	[Range(0,100)]
	public int SweepBall;

	private int numberPUTypes = 7;
	private int nonNullNumberPUTypes = -1;
	private int[] PUTypes;

	public PowerUpType nextPowerUp(){
		return findCase ( (int)Mathf.Lerp(0, _nonNullNumberPUTypes * 100, Random.value) );
	}

	int[] _PUTypes{
		get{
			if (PUTypes == null) CreatePUTypes(); 
			return PUTypes;
		}
	}

	int _nonNullNumberPUTypes{
		get{
			if (nonNullNumberPUTypes == -1){
				nonNullNumberPUTypes = (_PUTypes[0] != 0)? 1 : 0;
				for (int i = 1; i<_PUTypes.Length;i++)
					if (_PUTypes[i] != _PUTypes[i-1])
						nonNullNumberPUTypes++;
			}
			return nonNullNumberPUTypes;
		}
	}

	void CreatePUTypes(){
		PUTypes = new int[numberPUTypes];
		PUTypes [0] = BigPaddle;
		PUTypes [1] = SmallPaddle;
		PUTypes [2] = ShooterPaddle;
		PUTypes [3] = SpeedBall;
		PUTypes [4] = SlowBall;
		PUTypes [5] = MultiBall;
		PUTypes [6] = SweepBall;

		for (int i = 1; i<numberPUTypes; i++) {
			PUTypes[i] += PUTypes[i-1];
		}
	}

	PowerUpType SwitchPUTypes(int i){
		switch (i) {
		case 0:
			return PowerUpType.BigPaddle;
		case 1:
			return PowerUpType.SmallPaddel;
		case 2:
			return PowerUpType.ShooterPaddle;
		case 3:
			return PowerUpType.SpeedBall;
		case 4:
			return PowerUpType.SlowBall;
		case 5:
			return PowerUpType.MultiBall;
		case 6:
			return PowerUpType.SweepBall;
		default:
			return PowerUpType.None;
		}
	}

	int findIndexCase(int i){
		for (int j = 0; j<_PUTypes.Length; j++)
			if (i < _PUTypes [j])
				return j;
		return -1;
	}

	PowerUpType findCase(int i){
		return SwitchPUTypes (findIndexCase (i));
	}
}

public enum PowerUpType {BigPaddle, SmallPaddel, ShooterPaddle, SpeedBall, SlowBall, MultiBall, SweepBall, None}

public class PowerUpSpawner : MonoBehaviour {

	public static PowerUpSpawner instance;

	void Awake(){
		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy (gameObject);
	}
	
	public PowerUpSetting spawner;

	public GameObject PUBig;
	public GameObject PUSmall;
	public GameObject PUShooter;
	public GameObject PUSpeed;
	public GameObject PUSlow;
	public GameObject PUSweep;
	public GameObject PUMulti;

	public static void instantiatePowerUp(Vector3 position){
		instance._instantiatePowerUp (position);
	}

	public static void instantiatePowerUp(Vector3 position, PowerUpType powerUp){
		instance._instantiatePowerUp (position, powerUp);
	}

	void _instantiatePowerUp(Vector3 position){
		_instantiatePowerUp (position, spawner.nextPowerUp ());
	}

	void _instantiatePowerUp(Vector3 position, PowerUpType powerUp){
		switch (powerUp) {
		case PowerUpType.None:
			return;
		case PowerUpType.BigPaddle:
			GameObject.Instantiate(PUBig,position,PUBig.transform.rotation);
			break;
		case PowerUpType.SmallPaddel:
			GameObject.Instantiate(PUSmall,position,PUSmall.transform.rotation);
			break;
		case PowerUpType.ShooterPaddle:
			GameObject.Instantiate(PUShooter,position,PUShooter.transform.rotation);
			break;
		case PowerUpType.SpeedBall:
			GameObject.Instantiate(PUSpeed,position,PUSpeed.transform.rotation);
			break;
		case PowerUpType.SlowBall:
			GameObject.Instantiate(PUSlow,position,PUSlow.transform.rotation);
			break;
		case PowerUpType.MultiBall:
			GameObject.Instantiate(PUMulti,position,PUMulti.transform.rotation);
			break;
		case PowerUpType.SweepBall:
			GameObject.Instantiate(PUSweep,position,PUSweep.transform.rotation);
			break;
		default:
			break;
		}
	}

	public static void makePowerUp (PowerUpType powerUpType, Paddle paddle, Ball ball, float durationTime, float powerUpIdentifier){
		instance._makePowerUp (powerUpType,paddle,ball,durationTime, powerUpIdentifier);
	}

	void _makePowerUp (PowerUpType powerUpType, Paddle paddle, Ball ball, float durationTime, float powerUpIdentifier){
		switch (powerUpType) {
		case PowerUpType.None:
			return;
		case PowerUpType.BigPaddle:
			paddle.EndPowerUp(PowerUpType.SmallPaddel);
			StartCoroutine( BigPaddle(powerUpType,paddle,durationTime,powerUpIdentifier));
			break;
		case PowerUpType.SmallPaddel:
			paddle.EndPowerUp(PowerUpType.BigPaddle);
			StartCoroutine( SmallPaddle(powerUpType,paddle,durationTime,powerUpIdentifier));
			break;
		case PowerUpType.ShooterPaddle:
			StartCoroutine( ShooterPaddle(powerUpType,paddle,durationTime,powerUpIdentifier));
			break;
		case PowerUpType.SpeedBall:
			StartCoroutine( SpeedBall(powerUpType,paddle, ball,durationTime,powerUpIdentifier));
			break;
		case PowerUpType.SlowBall:
			StartCoroutine( SlowBall(powerUpType,paddle, ball,durationTime,powerUpIdentifier));
			break;
		case PowerUpType.MultiBall:
			StartCoroutine( MultiBall(powerUpType,paddle, ball,durationTime,powerUpIdentifier));
			break;
		case PowerUpType.SweepBall:
			StartCoroutine( SweepBall(powerUpType,paddle, ball,durationTime,powerUpIdentifier));
			break;
		default:
			break;
		}
	}
	
	IEnumerator BigPaddle(PowerUpType powerUpType, Paddle paddle, float durationTime, float powerUpIdentifier){
		// Resize the paddle
		Vector3 paddleLossyScale = paddle.lossyScale;
		ResizePaddle(paddle, paddleLossyScale.x * 2,paddleLossyScale.y,paddleLossyScale.z);

		// If is an infinity power up, we are done
		if (durationTime == float.PositiveInfinity) {
			EndPowerUp ( powerUpType, paddle );
			yield break;
		}
		// Else wait till the end of the power up
		yield return new WaitForSeconds(durationTime);

		// Check if the paddle is still alive, and the powerUp identifiers matches
		if (paddle == null || paddle.GetPowerUpIdentifier(powerUpType) != powerUpIdentifier) yield break;

		// Resize the paddle to normal
		ResizePaddle ( paddle, paddleLossyScale);
		
		// End Power Up
		EndPowerUp ( powerUpType, paddle );
	}
	
	IEnumerator SmallPaddle(PowerUpType powerUpType, Paddle paddle, float durationTime, float powerUpIdentifier){
		// Resize the paddle
		Vector3 paddleLossyScale = paddle.lossyScale;
		ResizePaddle(paddle, paddleLossyScale.x * 0.5f,paddleLossyScale.y,paddleLossyScale.z);
		
		// If is an infinity power up, we are done
		if (durationTime == float.PositiveInfinity) {
			EndPowerUp ( powerUpType, paddle );
			yield break;
		}
		
		// Else wait till the end of the power up
		yield return new WaitForSeconds(durationTime);

		// Check if the paddle is still alive, and the powerUp identifiers matches
		if (paddle == null || paddle.GetPowerUpIdentifier(powerUpType) != powerUpIdentifier) yield break;

		// Resize the paddle to normal
		ResizePaddle (paddle, paddleLossyScale);

		// End Power Up 
		EndPowerUp ( powerUpType, paddle );
	}
	
	IEnumerator ShooterPaddle(PowerUpType powerUpType, Paddle paddle, float durationTime, float powerUpIdentifier){

		// If is an infinity power up, we are done
		if (durationTime == float.PositiveInfinity) {
			EndPowerUp ( powerUpType, paddle );
			yield break;
		}
		
		// Else wait till the end of the power up
		yield return new WaitForSeconds(durationTime);

		// Check if the paddle is still alive, and the powerUp identifiers matches
		if (paddle == null || paddle.GetPowerUpIdentifier(powerUpType) != powerUpIdentifier) yield break;
		
		// End Power Up
		EndPowerUp ( powerUpType, paddle );
	}
	
	IEnumerator SpeedBall(PowerUpType powerUpType, Paddle paddle, Ball ball, float durationTime, float powerUpIdentifier){

		// Make the changes
		float ballInitSpeed = ball.GetComponent<Rigidbody> ().velocity.magnitude;
		VelocityBall (ball, ballInitSpeed * 1.5f);

		// If is an infinity power up, we are done
		if (durationTime == float.PositiveInfinity) {
			EndPowerUp ( powerUpType, paddle );
			yield break;
		};
		
		// Else wait till the end of the power up
		yield return new WaitForSeconds(durationTime);

		// Check if the paddle is still alive, and the powerUp identifiers matches
		if (paddle == null || paddle.GetPowerUpIdentifier(powerUpType) != powerUpIdentifier) yield break;

		// Undo the changes
		VelocityBall (ball, ballInitSpeed);

		// End Power Up
		EndPowerUp ( powerUpType, paddle );
	}
	
	IEnumerator SlowBall(PowerUpType powerUpType, Paddle paddle, Ball ball, float durationTime, float powerUpIdentifier){
		
		// Make the changes
		
		// If is an infinity power up, we are done
		if (durationTime == float.PositiveInfinity) {
			EndPowerUp ( powerUpType, paddle );
			yield break;
		}
		
		// Else wait till the end of the power up
		yield return new WaitForSeconds(durationTime);
		
		// Check if the paddle is still alive, and the powerUp identifiers matches
		if (paddle == null || paddle.GetPowerUpIdentifier(powerUpType) != powerUpIdentifier) yield break;
		
		// Undo the changes
		
		// End Power Up
		EndPowerUp ( powerUpType, paddle );
	}
	
	IEnumerator MultiBall(PowerUpType powerUpType, Paddle paddle, Ball ball, float durationTime, float powerUpIdentifier){
		
		// Make the changes
		
		// If is an infinity power up, we are done
		if (durationTime == float.PositiveInfinity) {
			EndPowerUp ( powerUpType, paddle );
			yield break;
		}
		
		// Else wait till the end of the power up
		yield return new WaitForSeconds(durationTime);
		
		// Check if the paddle is still alive, and the powerUp identifiers matches
		if (paddle == null || paddle.GetPowerUpIdentifier(powerUpType) != powerUpIdentifier) yield break;
		
		// Undo the changes
		
		// End Power Up
		EndPowerUp ( powerUpType, paddle );
	}
	
	IEnumerator SweepBall(PowerUpType powerUpType, Paddle paddle, Ball ball, float durationTime, float powerUpIdentifier){
		
		// Make the changes
		
		// If is an infinity power up, we are done
		if (durationTime == float.PositiveInfinity) {
			EndPowerUp ( powerUpType, paddle );
			yield break;
		}
		
		// Else wait till the end of the power up
		yield return new WaitForSeconds(durationTime);
		
		// Check if the paddle is still alive, and the powerUp identifiers matches
		if (paddle == null || paddle.GetPowerUpIdentifier(powerUpType) != powerUpIdentifier) yield break;
		
		// Undo the changes
		
		// End Power Up
		EndPowerUp ( powerUpType, paddle );
	}

	/**************************************************************************************************/
	
	void ResizePaddle(Paddle paddle, Vector3 size){
		// If ball is attached
		Transform ballTransform = paddle.transform.FindChild ("Ball");
		Vector3 loosyBallScale = Vector3.zero;
		if (ballTransform != null)  loosyBallScale = ballTransform.lossyScale;

		paddle.transform.localScale = size;
		
		// If ball is attached, resize the ball
		if (ballTransform != null) {
			Transform ballParent = ballTransform.parent;
			ballTransform.parent = null;
			ballTransform.localScale = loosyBallScale;
			ballTransform.parent = ballParent;
		}
	}
	
	void ResizePaddle(Paddle paddle, float scaleX, float scaleY, float scaleZ){
		ResizePaddle(paddle,new Vector3(scaleX,scaleY,scaleZ));
	}

	void VelocityBall (Ball ball, float speed){
		Rigidbody ballRigidbody = ball.gameObject.GetComponent<Rigidbody> ();
		Vector3 velocity = ballRigidbody.velocity;
		float magnitude = velocity.magnitude;
		float factorX = velocity.x / magnitude;
		float factorY = velocity.y / magnitude;
		float factorZ = velocity.z / magnitude;

		ballRigidbody.velocity = new Vector3 (
			factorX * speed,
			factorY * speed,
			factorZ * speed
			);
	}
	
	void EndPowerUp(PowerUpType powerUpType, Paddle paddle){
		paddle.EndPowerUp (powerUpType);
	}
}

