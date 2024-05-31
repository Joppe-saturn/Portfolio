using UnityEngine;

public class cloneManager : MonoBehaviour
{
    private void Start()
    {
        Mesh parentMesh = transform.parent.GetComponent<MeshFilter>().mesh;
        Material[] parentMaterial = transform.parent.GetComponent<MeshRenderer>().materials;
        Transform parentTransForm = transform.parent.transform;

        //this one makes sure the distance between the clone and the parent is always the same regardless of size
        transform.localScale = new Vector3(1 / parentTransForm.localScale.x, 1 / parentTransForm.localScale.y, 1 / parentTransForm.localScale.z);

        //this one gives each clone the mesh of the parent
        foreach (Transform child in transform)
        {
            child.GetComponent<MeshFilter>().mesh = parentMesh;
            child.GetComponent<MeshRenderer>().materials = parentMaterial;
            child.transform.localScale = parentTransForm.localScale;
            if(parentTransForm.tag == "astroid")
            {
                child.transform.tag = "astroid2";
            }
            if(parentTransForm.tag == "Player")
            {
                child.GetComponent<BoxCollider>().enabled = false;
            }
        }
    }

    private void Update()
    {
        Transform parentTransForm = transform.parent.transform;

        transform.localScale = new Vector3(1 / parentTransForm.localScale.x, 1 / parentTransForm.localScale.y, 1 / parentTransForm.localScale.z);

        transform.rotation = Quaternion.identity;
        foreach (Transform child in transform)
        {
            child.transform.rotation = transform.parent.transform.rotation;
            child.transform.localScale = parentTransForm.localScale;
        }
    }
}