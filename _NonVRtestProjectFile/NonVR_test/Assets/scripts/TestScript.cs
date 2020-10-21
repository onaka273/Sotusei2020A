using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    public GameObject TargetObject;
    public GameObject[] Prefabs;
    
    private GameObject TargetInstance;
    // Start is called before the first frame update
    void Start()
    {
        TargetInstance = Instantiate(TargetObject);
        var effectInstance = Instantiate(Prefabs[0]);
        effectInstance.transform.parent = TargetInstance.transform;
        effectInstance.transform.localPosition = Vector3.zero;
        effectInstance.transform.localRotation = new Quaternion();
        var meshUpdater = effectInstance.GetComponent<PSMeshRendererUpdater>();
        meshUpdater.UpdateMeshEffect(TargetInstance);
    }

    // Update is called once per frame
    void Update()
    {
    }
}
