using UnityEngine.Events;

public class EnemyEvents : EntityEvents
{
    public event UnityAction Attacking;

    public void OnAttacking()
    {
        Attacking?.Invoke();
    }
}
