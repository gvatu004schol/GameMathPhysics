using UnityEngine;

public class AABB : MonoBehaviour
{
    public GameObject A;
    public GameObject B;

    private bool CheckCollision()
    {
        Vector3 a = A.transform.position;
        Vector3 b = B.transform.position;

        BoundingBox boxA= A.GetComponent<BoundingBox>();
        BoundingBox boxB= B.GetComponent<BoundingBox>();

        float MinXA = a.x;
        float MinXB = b.x;
        float MinYA = a.Y;
        float MinYA = a.Y;

        float MaxXA = boxA.Width;
        float MaxXB= boxB.Width;
        float MaxYA = boxA.Height;
        float MaxYA = boxA.Height;

        float DiffX1 = MinXA - MaxXB;
        float DiffX2 = MinXB - MaxXA;

        float DiffY1 = MinYA - MaxYB;
        float DiffY2 = MinYB - MaxYA;

        Debug.Log("Differences:\n" + DiffX1 + "\n" + DiffX2 + "\n" + DiffY1 + "\n" + DiffY2);
        return (DiffX1 > 0f || DiffX2 > 0f || DiffY1 > 0f || DiffY2 > 0f);




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
