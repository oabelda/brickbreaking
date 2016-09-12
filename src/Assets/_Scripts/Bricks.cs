using UnityEngine;
using System.Collections;

public class Bricks : MonoBehaviour {

	public bool unbreakeable;
	public GameObject unbreakeableSound;
	public GameObject brickParticle;
	public Material[] mat;

	private int actualMat = 0;

	private Renderer rend;

	public PowerUpSetting powerUp;
	public bool useEnvironmentPowerUp = true;

	void OnCollisionEnter(Collision other){

		if (unbreakeable) {
			if (unbreakeableSound !=null) Instantiate (unbreakeableSound, transform.position, Quaternion.identity);
			return;
		}

		Instantiate (brickParticle, transform.position, Quaternion.identity);

		if (mat.Length == 0 || mat == null) {
			DestroyBrick();
		} else {
			if (actualMat >= mat.Length) {
				DestroyBrick();
			} else {
				if (rend == null) rend = GetComponent<Renderer> (); 

				rend.sharedMaterial = mat [actualMat];

				actualMat++;
			}
		}

		SpawnPowerUp ();

	}

	void DestroyBrick(){
		GM.instance.DestroyBrick ();
		Destroy (gameObject);
	}

	void SpawnPowerUp(){
		if (useEnvironmentPowerUp) {
			PowerUpSpawner.instantiatePowerUp (this.transform.position);
		} else {
			PowerUpSpawner.instantiatePowerUp ( this.transform.position, powerUp.nextPowerUp() );
		}
	}
}
