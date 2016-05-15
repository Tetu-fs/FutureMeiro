using UnityEngine;
using System.Collections;

public class WallToWay : MonoBehaviour {

    private MeshRenderer myMesh;
    // Use this for initialization
    void Start () {
        myMesh = GetComponent<MeshRenderer>();

    }

    void MeshDenable()
    {
        myMesh.enabled = false;
    }

}
