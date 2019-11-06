using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Change : MonoBehaviour{
    public ParticleSystem star, kernel, aperture;
    Color red = new Color(255, 0, 0), bule = new Color(0, 0, 255), green = new Color(0, 255, 0);

    void Start(){ }

    void OnGUI(){
        if (GUI.Button(new Rect(0, 0, 80, 40), "red")){
        	var main = star.main;
        	main.startColor = red;
      		main = kernel.main;
        	main.startColor = red;
        	main = aperture.main;
        	main.startColor = red;
        }
        if (GUI.Button(new Rect(80, 0, 80, 40), "bule")){
        	var main = star.main;
        	main.startColor = bule;
      		main = kernel.main;
        	main.startColor = bule;
        	main = aperture.main;
        	main.startColor = bule;
        }
        if (GUI.Button(new Rect(160, 0, 80, 40), "green")){
        	var main = star.main;
        	main.startColor = green;
      		main = kernel.main;
        	main.startColor = green;
        	main = aperture.main;
        	main.startColor = green;
        }
    }
    // Update is called once per frame
    void Update(){ }
}