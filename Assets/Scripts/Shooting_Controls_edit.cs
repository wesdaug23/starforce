using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Shooting_Controls_edit: MonoBehaviour
{
	public GameObject projectile;
	public float lifetime = 2.0f;
	public float projForce = 10000.0f;
	public int starPointNum = 5;
	public List<GameObject> starpoints = new List<GameObject>();
	public float reloadTime;
	private List<SpriteRenderer> spri = new List<SpriteRenderer>();
	public List<bool> canShoot = new List<bool> (5);
	public List<bool> autoShoot = new List<bool> ();
	public bool autoShootAll;
	
	//Direction Vectors for projectiles
	private List<Vector2> pointVectList = new List<Vector2>(12); 
	private List<float> pointAngles = new List<float>(12);




	//Real-time update. Put conditions you always want to check for here
	void Update( )
	{
		redrawStar(transform.rotation, starPointNum);

		if (Input.GetKeyDown (KeyCode.F))
		{
			if (autoShootAll)
				autoShootAll = false;
			else {
				autoShootAll = true;
			}
		}

		if (Input.GetKeyDown (KeyCode.Y))
		{
			starPointNum = starPointNum - 1;
		}

		if (Input.GetKeyDown (KeyCode.U))
		{
			starPointNum = starPointNum + 1;
		}

		if (Input.GetKeyDown (KeyCode.Alpha1) && Input.GetKeyDown (KeyCode.E))
		{
			if (autoShoot [0])
				autoShoot [0] = false;
			else {
				autoShoot [0] = true;
			}
		}

		if ((Input.GetKeyDown (KeyCode.Alpha1) || autoShoot [0] || autoShootAll) && canShoot [0] == true)
			
			Shoot (1); //fires "top" point
		
		if( (Input.GetKeyDown( KeyCode.Alpha2 ) || autoShootAll) && canShoot [1] == true)
			
			Shoot (2); //fires "right top" point
		
		if ((Input.GetKeyDown (KeyCode.Alpha3) || autoShootAll) && canShoot [2] == true)
			
			Shoot (3); //fires "right bottom" point
		
		if( (Input.GetKeyDown( KeyCode.Alpha4 ) || autoShootAll) && canShoot [3] == true)
			
			Shoot (4); //fires "left bottom" point
		
		if( (Input.GetKeyDown( KeyCode.Alpha5 ) || autoShootAll) && canShoot [4] == true)
			
			Shoot (5); //fires "left top" point
	}
	
	
	//Creates projectile and shoots it in appropriate direction
	void Shoot(int point) {
		
		//clones existing projectile gameobject
		GameObject proj = Instantiate(projectile, transform.position, Quaternion.identity) as GameObject;
		SpriteRenderer spr = starpoints [point - 1].GetComponent<SpriteRenderer> (); 
		SpriteRenderer sr = proj.GetComponent<SpriteRenderer> (); 
		Rigidbody2D rb = proj.GetComponent<Rigidbody2D> ();
		spri.Add(spr);
		canShoot [point - 1] = false;
		
		//switch statement determines which sprite to use for star and projectile
		//also adds force to make projectile move
		switch (point) {
			
		case 1:
			spr.sprite = Resources.Load<Sprite> ("Sprites/Point_Launched_White");
			rb.MoveRotation (pointAngles[0]);
			sr.sprite = Resources.Load<Sprite> ("Sprites/Point_Attached_White_Lineat60");
			rb.AddForce (pointVectList[0]*projForce);
			break;
		case 2:
			spr.sprite = Resources.Load<Sprite> ("Sprites/Point_Launched_White");
			rb.MoveRotation (pointAngles[1]);
			sr.sprite = Resources.Load<Sprite>("Sprites/Point_Attached_White_Lineat60");
			rb.AddForce(pointVectList[1]*projForce);
			break;
		case 3:
			spr.sprite = Resources.Load<Sprite> ("Sprites/Point_Launched_White");
			rb.MoveRotation (pointAngles[2]);
			sr.sprite = Resources.Load<Sprite>("Sprites/Point_Attached_White_Lineat60");
			rb.AddForce(pointVectList[2]*projForce);
			break;
		case 4:
			spr.sprite = Resources.Load<Sprite> ("Sprites/Point_Launched_White");
			rb.MoveRotation (pointAngles[3]);
			sr.sprite = Resources.Load<Sprite>("Sprites/Point_Attached_White_Lineat60");
			rb.AddForce(pointVectList[3]*projForce);
			break;
		case 5:
			spr.sprite = Resources.Load<Sprite> ("Sprites/Point_Launched_White");
			rb.MoveRotation (pointAngles[4]);
			sr.sprite = Resources.Load<Sprite>("Sprites/Point_Attached_White_Lineat60");
			rb.AddForce(pointVectList[4]*projForce);
			break;
		}

		/**
		 * ADDING COLLIDER INTERFERES WITH PROJECTILE MOTION
		*/

//		PolygonCollider2D pc = proj.AddComponent<PolygonCollider2D> ();
//		pc.density = 0;


		sr.enabled = true; //enable sprite render, projectile shows up
		StartCoroutine(reload(spr,reloadTime, point - 1));
		Destroy(proj, lifetime);


	}

	IEnumerator reload(SpriteRenderer sprIndex, float delayTime, int strPt)
	{
		yield return new WaitForSeconds (delayTime);
		spri [spri.FindIndex (d=>d == sprIndex)].sprite = Resources.Load<Sprite> ("Sprites/Point_Attached_White");
		spri.Remove (sprIndex);
		canShoot [strPt] = true;
	}

	void redrawStar(Quaternion q, int numPoints) {
			
			float angle = q.eulerAngles.z;

			float topAngle = angle + 90;
			pointAngles.Clear ();
			pointVectList.Clear ();
			pointAngles.Add(topAngle);
			for(int i = 1; i < starPointNum; i++)
			{
			pointAngles.Add(topAngle - i * (360 / numPoints) + 180);
			}

			for(int i = 0; i < starPointNum; i++)
			{
			pointVectList.Add(new Vector2(Mathf.Sin (pointAngles [i] * Mathf.Deg2Rad),  Mathf.Cos (pointAngles [i] * Mathf.Deg2Rad)));
			}
		}
	
	//Decides orientation of rotating star
	/*void getOrientation(Quaternion q) {
		
		float angle = q.eulerAngles.z;
	
		topAngle = (Mathf.Atan2 (0,1) * Mathf.Rad2Deg) + angle + 90;
		righttopAngle = topAngle - 72 + 180;
		rightbotAngle = topAngle - 144 + 180;
		leftbotAngle = topAngle - 216 + 180;
		lefttopAngle = topAngle - 288 + 180;

		top.y = -(Mathf.Cos(topAngle * Mathf.Deg2Rad));
		top.x = (Mathf.Sin(topAngle * Mathf.Deg2Rad));
		righttop.x = -(Mathf.Sin(righttopAngle * Mathf.Deg2Rad));
		righttop.y = (Mathf.Cos(righttopAngle * Mathf.Deg2Rad));
		rightbot.x = -(Mathf.Sin(rightbotAngle * Mathf.Deg2Rad));
		rightbot.y = (Mathf.Cos(rightbotAngle * Mathf.Deg2Rad));
		leftbot.x = -(Mathf.Sin(leftbotAngle * Mathf.Deg2Rad));
		leftbot.y = (Mathf.Cos(leftbotAngle * Mathf.Deg2Rad));
		lefttop.x = -(Mathf.Sin(lefttopAngle * Mathf.Deg2Rad));
		lefttop.y = (Mathf.Cos(lefttopAngle * Mathf.Deg2Rad));
	}*/
}