using UnityEngine;

public class CarController : MonoBehaviour
{
    public Transform StartPoint;
    public GameObject truck;
    int i = 0;
    // �浹�� ���۵� �� ȣ��˴ϴ�.
 
    public void MakeCar()
    {
        if (i == 20)
        {
            Debug.Log("�����Ҵ緮�� �������ϴ�");
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
        // Ʈ���� ������ �������� ������Ʈ�� ExitPoint���� Ȯ���մϴ�.
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
