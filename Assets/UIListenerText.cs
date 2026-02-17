using UnityEngine;
using TMPro;

public class UIListenerText : MonoBehaviour
{

    [SerializeField] PoleDetector detector;
    [SerializeField] TextMeshProUGUI hintText;

    void Update()
    {
        hintText.gameObject.SetActive(detector.IsSomethingDetected);
    }
}
