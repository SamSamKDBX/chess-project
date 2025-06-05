using UnityEngine;

public class RenameSquares : MonoBehaviour
{
    [ContextMenu("renommer les cases du tableau")]
    public void renameSquares()
    {
        foreach (Transform child in transform)
        {
            child.name = $"square_{child.transform.position.x}_{-child.transform.position.y}";
        }
    }
}
