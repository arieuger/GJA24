using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour
{
    [SerializeField] private GameObject cam;

    [SerializeField] private float duration;
    [SerializeField] private float magnitude;
  
    public static CameraShake Instance { get; private set; }
    
    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;
    }

    public void ShakeCamera()
    {
        if (gameObject) StartCoroutine(Shake());
    }
    
    private IEnumerator Shake()
    {
        Vector3 orignalPosition = cam.transform.position;
        float remainingTime = duration;
        
        while (remainingTime > 0f)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            cam.transform.position = new Vector3(x, y, -10f);
            remainingTime -= Time.deltaTime;
            
            
            yield return null;
        }
        cam.transform.position = orignalPosition;
    }
}