using UnityEngine;

public class CreateSquarePossibles : MonoBehaviour
{
    public GameObject squarePrefab;

    [ContextMenu("creer les square possible")]
    public void renameSquares()
    {
        foreach (Transform child in transform)
        {
            foreach (Transform child2 in child)
            {
                DestroyImmediate(child2.gameObject);
            }
            GameObject possibleSquare = Instantiate(squarePrefab, new Vector3(child.position.x, child.position.y, -2), Quaternion.identity, child);
            possibleSquare.name = child.name + "_possible";
            possibleSquare.SetActive(false);
        }
    }
}
