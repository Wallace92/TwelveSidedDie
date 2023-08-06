using System;
using TMPro;
using UnityEngine;

public class OneSideDie : MonoBehaviour
{
    public int Number => int.Parse(m_numberText);
    
    [SerializeField]
    private string m_numberText;
    
    public void Init(float numberFontSize)
    {
        GameObject textMeshProObject = new GameObject(name + "_TMP");
        textMeshProObject.transform.position = transform.position + transform.forward * 0.05f;
        textMeshProObject.transform.rotation = transform.rotation * Quaternion.Euler(0, 180, 0);
        textMeshProObject.transform.SetParent(transform);
        
        var textMeshProComponent = textMeshProObject.AddComponent<TextMeshPro>();
        textMeshProComponent.text = m_numberText;
        textMeshProComponent.fontSize = numberFontSize; 
        textMeshProComponent.alignment = TextAlignmentOptions.Center;
        textMeshProComponent.rectTransform.sizeDelta = new Vector3(m_numberText.Length, 1);
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, 0.05f);
    }
}
