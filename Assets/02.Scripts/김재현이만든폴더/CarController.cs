using UnityEngine;

public class CarController : MonoBehaviour
{
    public Transform StartPoint;
    public GameObject truck;
    int i = 0;
    // 충돌이 시작될 때 호출됩니다.
 
    public void MakeCar()
    {
        if (i == 20)
        {
            Debug.Log("오늘할당량이 끝났습니다");
        }
        else { 
        GameObject newObject = Instantiate(truck,StartPoint);
        // Access the variable from the instantiated object
            truckTrigger component = newObject.GetComponent<truckTrigger>(); // Replace YourComponent with the actual component type
            if (component != null)
            {
                // Initialize your variable with the value of the variable held by the object
                component.truckNum = i; // Replace variableToAccess with the actual variable name
                // Now you can use variableValue3 as needed
                i++;
            }
            else
            {
                Debug.LogWarning("Prefab does not contain the expected component.");
            }

        }

    }
    private void OnTriggerEnter(Collider other)
    {
        // 트리거 영역을 빠져나간 오브젝트가 ExitPoint인지 확인합니다.
        if (other.CompareTag("Truck"))
        {
            MakeCar();
        }
    
    }

    void Start()
    {
        MakeCar();
    }
}
