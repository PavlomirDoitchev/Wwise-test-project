using UnityEngine;

public class ReverbZone : MonoBehaviour
{
    [SerializeField] private string rtpcName = "ReverbAmount";
    [SerializeField, Range(0, 100)] private float insideValue = 80f;
    [SerializeField, Range(0, 100)] private float outsideValue = 0f;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private float blendSpeed = 2f;

    private float currentValue;
    private float targetValue;

    private void OnTriggerEnter(Collider other) 
    { 
    //    if (((1 << other.gameObject.layer) & playerLayer) != 0)
    //        targetValue = insideValue;
    if(other.gameObject.layer == LayerMask.NameToLayer("Player"))
            targetValue = insideValue;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
            targetValue = outsideValue;
    }

    private void Update()
    {
        currentValue = Mathf.MoveTowards(currentValue, targetValue, blendSpeed * Time.deltaTime);
        AkUnitySoundEngine.SetRTPCValue(rtpcName, currentValue);
    }
}