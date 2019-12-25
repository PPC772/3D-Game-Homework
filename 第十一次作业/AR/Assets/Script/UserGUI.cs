using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserGUI : MonoBehaviour{
    public GameObject player;
    private float player_speed = 0.005f;
    void Start(){
    }

    void Update(){
        float translationX = Input.GetAxis("Horizontal");
        float translationY = Input.GetAxis("Vertical");
        MovePlayer(translationX, translationY);
    }

	public void MovePlayer(float translationX, float translationY){
		if ((translationX == 0) && (translationY > 0)){
			player.transform.eulerAngles = new Vector3(270, 0, 0);
			player.transform.position += new Vector3(0, player_speed, 0);
		}
		if ((translationX > 0) && (translationY == 0)){
			player.transform.eulerAngles = new Vector3(0, 90, 270);
			player.transform.position += new Vector3(player_speed, 0, 0);
		}
		if ((translationX == 0) && (translationY < 0)){
			player.transform.eulerAngles = new Vector3(90, 0, 180);
			player.transform.position += new Vector3(0, -1 * player_speed, 0);
		}
		if ((translationX < 0) && (translationY == 0)){
			player.transform.eulerAngles = new Vector3(0, 270, 90);
			player.transform.position += new Vector3(-1 * player_speed, 0, 0);
		}
	}
}
