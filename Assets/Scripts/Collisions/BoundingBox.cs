using UnityEngine;

public class BoundingBox : MonoBehaviour
{
    public float Width = 1.0f;
    public float Height = 1.0f;

    private void OnDrawGizmos()
    {
        Handles.DrawSolidRectangleWithoutline (new Rect(transform.position.x , transform.position.y, Width, Height, Color.green, Color.darkGreen));


    }








    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
