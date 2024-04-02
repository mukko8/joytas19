using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
	public GameObject prefab;

	void Update () {
		if(Input.GetMouseButtonDown(0)){
			StartCoroutine (Explosion());
		}	
	}
	IEnumerator Explosion(){
		yield return null;
		RaycastHit hit;
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		if(Physics.Raycast(ray,out hit,100.0f)){
			if(hit.collider.tag=="Cube"){
				//当たったCubeを消す
				Destroy (hit.collider.gameObject);
				//rayとcubeと当たった地点い爆発エフェクト生成
				GameObject explosion=Instantiate (prefab, hit.point, Quaternion.identity) as GameObject;
				//爆発あった地点から半径10の球を作成しその中に収まるコライダーを配列で取得する。
				Collider[] colls = Physics.OverlapSphere (hit.point, 10.0f);
				//コライダー配列の走査
				foreach(Collider coll in colls){
					//rigidbodyの取得（なければnullが返る)
					Rigidbody rb = coll.GetComponent<Rigidbody> ();
					if(rb != null){
						//リジッドボディの付いている物体には爆発力を与える(爆発力,中心,半径)
						//rb.AddExplosionForce (800.0f, hit.point, 10.0f);
						//派手にしたい場合は第４引数を与えて上に飛ばしてやる(爆発力,中心,半径,上方向に飛ばす力)
						//(物理的にリアルな挙動ではなくなるが派手になってゲーム向き)
						rb.AddExplosionForce (500.0f, hit.point, 10.0f,10.0f);
					}
				}
				//爆発が終わったら爆発エフェクトオブジェクトを消す,第二引数を与えて生成されて５秒後(爆発の演出が終わった後)に削除する。
				Destroy (explosion, 5.0f);
			}
		}
	}
}
