using UnityEngine;

public class CraneSkidNumManager : MonoBehaviour
{
    public Transform[] skid;
    SkidBool skidbool;
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void SortSkidsByCode()
    {
        // Create a custom sorting algorithm based on the code values
        for (int i = 0; i < skid.Length - 1; i++)
        {
            for (int j = i + 1; j < skid.Length; j++)
            {
                float codeI = skid[i].GetComponent<SkidBool>().PirorNum;
                float codeJ = skid[j].GetComponent<SkidBool>().PirorNum;

                if (codeI > codeJ)
                {
                    // Swap the skids
                    Transform temp = skid[i];
                    skid[i] = skid[j];
                    skid[j] = temp;
                }
            }
        }
    }

    void Update()
    {
        
    }
}
