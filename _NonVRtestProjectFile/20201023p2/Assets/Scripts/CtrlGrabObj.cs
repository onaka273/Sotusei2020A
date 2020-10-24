using Valve.VR;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CtrlGrabObj : MonoBehaviour
{
 public SteamVR_Input_Sources handType;
     // 入力をポーリングする手のタイプ。これらは、All、Left、またはRightのいずれかです
public SteamVR_Action_Boolean grabAction; // グラブアクションへの参照。


private GameObject collidingObject; 
    // 現在衝突しているオブジェクトを取得することができます。
    public SteamVR_Behaviour_Pose controllerPose;
    //テレポートアクションへの参照
private GameObject objectInHand; 
    // 現在取得しているGameObjectへの参照として機能します。

    private void SetCollidingObject(Collider col)
    {
        if (collidingObject || !col.GetComponent<Rigidbody>())
        {
        return;
        }
        //すでになにか持っている・判定がない場合はターゲッティングしない
    collidingObject = col.gameObject;
    //オブジェクトをターゲットとして割り当てる
    }

    public void OnTriggerEnter(Collider other)
    {
    SetCollidingObject(other);
    }
    //トリガーコライダーが別のコライダーに入ると、他のコライダーを潜在的なグラブターゲットとして設定します（？）

    public void OnTriggerStay(Collider other)
    {
    SetCollidingObject(other);
    }
    //プレーヤーがコントローラーをオブジェクトの上にしばらく置いたときにターゲットが設定されるようにする。
    //これがないと、衝突が失敗したり、バグが発生したりする可能性があります。

    public void OnTriggerExit(Collider other)
    {
        if (!collidingObject)
        {
        return;
        }
    collidingObject = null;
    }
    //恐らくオブジェクトから離れたらターゲット外すというコード

    private void GrabObject()
    {
    objectInHand = collidingObject;
    collidingObject = null;
    // 衝突するGameObjectをプレーヤーの手に移動し、collidingObject変数から削除します。
    var joint = AddFixedJoint();
    joint.connectedBody = objectInHand.GetComponent<Rigidbody>();
    //コントローラーをオブジェクトに接続する新しいジョイントを追加します。
    }

    private FixedJoint AddFixedJoint()
    {
    FixedJoint fx = gameObject.AddComponent<FixedJoint>();
    fx.breakForce = 20000;
    fx.breakTorque = 20000;
    return fx;
    }
    //新しい固定ジョイントを作成し、コントローラーに追加してから、簡単に壊れないようにセットアップします。
    //最後に、あなたはそれを返します。？

    //▼▼▼▼▼▼▼ここ重要！！！！▼▼▼▼▼▼▼
    private void ReleaseObject()
    {
    if (GetComponent<FixedJoint>())
    //コントローラに固定ジョイントが取り付けられていることを確認
        {
        GetComponent<FixedJoint>().connectedBody = null;
        Destroy(GetComponent<FixedJoint>());
        //ジョイントによって保持されているオブジェクトへの接続を削除し、ジョイントを破壊します
        objectInHand.GetComponent<Rigidbody>().velocity = controllerPose.GetVelocity();
        objectInHand.GetComponent<Rigidbody>().angularVelocity = controllerPose.GetAngularVelocity();
        //プレーヤーがオブジェクトを離したときのコントローラーの速度と回転を追加すると、リアルな円弧になります
        }
    objectInHand = null;
    //以前にアタッチされたオブジェクトへの参照を削除します
    }

    void Start()
    {
    GameObject obj = (GameObject)Resources.Load ("RwCube");
    }
    //プレハブをロード


    // Update is called once per frame
    void Update()
    {

    if (grabAction.GetLastStateDown(handType))
    {
        if (collidingObject)
        {
        GrabObject();
        }
    }
    //プレイヤーがグラブアクションをトリガーしたら、オブジェクトをグラブします。

    if (grabAction.GetLastStateUp(handType))
    {
        if (objectInHand)
        {
        Vector3 tmp = GameObject.Find("Controller (left)").transform.position;
        //コントロ－ラーの位置を取得
        float x = tmp.x;
        float y = tmp.y;
        float z = tmp.z;

        Debug.Log(x);
        Debug.Log(y);
        Debug.Log(z);

        ReleaseObject();
        }
    }
    //プレーヤーがグラブアクションにリンクされた入力を解放し、コントローラーにオブジェクトがアタッチされている場合、これはそれを解放します。

    }
}
