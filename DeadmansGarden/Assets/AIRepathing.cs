
using UnityEngine;

public class AIRepathing : MonoBehaviour
{
    [SerializeField] private AIScript aI;
    private void Awake()
    {
        aI = FindObjectOfType<AIScript>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("AI"))
        {

            if (!aI.GetPlayerDetection())
            {
                aI.CallNumerator();
            }


        }
    }




}
