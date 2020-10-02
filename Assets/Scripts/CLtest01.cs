using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class CLtest01 : MonoBehaviour
{
    private SteamVR_Action_Boolean actionToHaptic = SteamVR_Actions._default.GrabPinch;
    //これでグリップをピンチした時のアクションを取ってるはず
    private SteamVR_Action_Vibration haptic = SteamVR_Actions._default.Haptic;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
         GameObject obj = (GameObject)Resources.Load ("Magic01");
         if (actionToHaptic.GetStateDown(SteamVR_Input_Sources.Any)) {
             Debug.Log("toriga-wohanasu");
             Instantiate (obj, new Vector3(-5.0f,2.0f,0.0f), Quaternion.identity);
         }
    }
}
