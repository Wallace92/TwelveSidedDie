using TMPro;
using UnityEngine;

public class OneSideDie : MonoBehaviour
{
    public int Number => int.Parse(PlainNumber);
    
    private string PlainNumber => m_numberText.Contains(".")
        ? m_numberText.Replace(".", "")
        : m_numberText;

    [SerializeField]
    private string m_numberText;
    
    public void Init(DieData dieData)
    {
        var textMeshProObject = AddTextMeshProGameObject(dieData);
        var textMeshProComponent = textMeshProObject.AddComponent<TextMeshPro>();
        
        SetTextMeshProComponentProperties(textMeshProComponent, dieData);
    }

    private GameObject AddTextMeshProGameObject(DieData dieData)
    {
        GameObject textMeshProObject = new GameObject(name + "_TMP");
        textMeshProObject.transform.position = transform.position + transform.forward * dieData.NumberAlignment;
        textMeshProObject.transform.rotation = transform.rotation * Quaternion.Euler(0, 180, 0);
        textMeshProObject.transform.SetParent(transform);

        return textMeshProObject;
    }

    private void SetTextMeshProComponentProperties(TMP_Text textMeshProComponent, DieData dieData)
    {
        textMeshProComponent.text = m_numberText;
        textMeshProComponent.fontSize = dieData.NumberFontSize; 
        textMeshProComponent.alignment = TextAlignmentOptions.Center;
        textMeshProComponent.rectTransform.sizeDelta = new Vector3(m_numberText.Length, 1);
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, 0.05f);
    }
}
