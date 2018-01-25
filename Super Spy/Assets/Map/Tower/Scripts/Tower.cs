using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : Check {
	public int AttackRang = 1;
	// Use this for initialization
	public GameObject mAttackTarget;
	public float mAttackCD = 1.5f;//攻ji冷却cd
	public float _time=0;
    public Bullet bullet;
	SpriteRenderer show;
	public int totalHp=5;
	int GrayHp=5;
	int RedHp = 0;
	int BlueHp = 0;
	string enemyTag;
	void Start()
	{
		show = transform.Find ("rang").GetComponent<SpriteRenderer> ();
		show.enabled = false;
	}
    public void Update()
	{
		_time += Time.deltaTime;
		if (_time >= mAttackCD) //超guo冷却cd就攻ji，并重置jishi
		{
			_time = 0;
			//get attacktarget
			mAttackTarget = this.FindObjectAroundthePoint (transform.position, Vector3.forward, 360, 180, 10.1f);
			if (mAttackTarget != null) {
				string selfTag = transform.gameObject.tag;
				switch (selfTag) 
				{
				case "Gray"://还没属于任何一方
					enemyTag = mAttackTarget.tag;
					if (enemyTag == "Blue") {
						if (RedHp > 0) {//red来打过
							RedHp--;
							GrayHp = totalHp - RedHp;
						} else {
							BlueHp++;
							GrayHp = totalHp - BlueHp;
							if (GrayHp == 0) {
								transform.gameObject.tag = enemyTag;
								transform.GetComponent<TowerController> ().ChangeStage (enemyTag);
							}
						}
					} else if (enemyTag == "Red") {
						if (BlueHp > 0) {
							BlueHp--;
							GrayHp = totalHp - BlueHp;
						} else {
							RedHp++;
							GrayHp = totalHp - RedHp;
							if (GrayHp == 0) {
								transform.gameObject.tag = enemyTag;
								transform.GetComponent<TowerController> ().ChangeStage (enemyTag);
							}
						}
					}
					break;
				case "Blue":
					BlueHp--;
					GrayHp = totalHp - BlueHp;
					if (BlueHp == 0) 
					{
						transform.gameObject.tag = "Gray";
						transform.GetComponent<TowerController> ().ChangeStage ("Gray");
					}
						
					break;
				case "Red":
					RedHp--;
					GrayHp = totalHp - RedHp;
					if (RedHp == 0)
					{
						transform.gameObject.tag = "Gray";
						transform.GetComponent<TowerController> ().ChangeStage ("Gray");
					}
					break;
				}

				show.enabled = true;
				Attack (this.mAttackTarget);
			} else {
				show.enabled = false;
			}
		}
	}
	public void Attack(GameObject enemy)
	{
        Bullet n = Instantiate(bullet, new Vector3(transform.position.x, transform.position.y+6, transform.position.z), transform.rotation);
        n.InitData(enemy, 8, 2);
    }
}
