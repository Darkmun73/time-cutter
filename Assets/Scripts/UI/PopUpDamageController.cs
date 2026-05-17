using TMPro;
using UnityEngine;

public class PopUpDamageController : MonoBehaviour
{
    [SerializeField] private GameObject popUpDamagePrefab;

    [SerializeField] private Health health;

    void OnEnable()
    {
        health.Damaged += CreatePopUpDamage;
    }

    private void CreatePopUpDamage(float damage)
    {
        var popUpDamageObject = Instantiate(popUpDamagePrefab, transform);

        var textObject = popUpDamageObject.GetComponent<TMP_Text>();
        textObject.text = $"-{damage:F1}";

        var animator = popUpDamageObject.GetComponent<Animator>();
        var animationTimeLength = animator.GetCurrentAnimatorStateInfo(0).length;

        Destroy(popUpDamageObject, animationTimeLength);
    }

}
