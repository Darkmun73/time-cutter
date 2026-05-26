using TMPro;
using UnityEngine;

public class PopUpDamageController : MonoBehaviour
{
    [SerializeField] private GameObject popUpDamageWrapperPrefab;
    [SerializeField] private Transform popUpInitialPosition;

    [SerializeField] private Health health;

    void OnEnable()
    {
        health.Damaged += CreatePopUpDamage;
    }

    private void CreatePopUpDamage(float damage)
    {
        var popUpDamageWrapperObject = Instantiate(popUpDamageWrapperPrefab, transform);
        popUpDamageWrapperObject.transform.localPosition = popUpInitialPosition.localPosition;

        var textObject = popUpDamageWrapperObject.GetComponentInChildren<TMP_Text>(); // TODO: Не вызывать GetComponent каждый раз
        textObject.text = $"-{damage:F1}";

        var animator = popUpDamageWrapperObject.GetComponentInChildren<Animator>();
        var animationTimeLength = animator.GetCurrentAnimatorStateInfo(0).length; //TODO: хранить где-нибудь в отдельной штуке, и не обращаться через аниматор

        Debug.Log($"wrapper local: {popUpDamageWrapperObject.transform.localPosition}");
        Debug.Log($"wrapper global: {popUpDamageWrapperObject.transform.position}");
        Debug.Log($"textObject local: {textObject.transform.localPosition}");
        Debug.Log($"textObject global: {textObject.transform.position}");
        Destroy(popUpDamageWrapperObject, animationTimeLength);
    }

}
