using UnityEngine;

public class Square : MonoBehaviour
{
    private bool isPossibleSquare;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        isPossibleSquare = false;
    }

    

    // Update is called once per frame
    void Update()
    {
        if (isPossibleSquare)
        {
            GameObject.Find($"{this.name}_possible").SetActive(true);
        }
    }
}
