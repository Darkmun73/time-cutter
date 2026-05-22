using UnityEngine;

[RequireComponent(typeof(HitReceiver))]
public class HitBloodSplasher : MonoBehaviour, IHitInfoVisitor
{
    [SerializeField] private ParticleSystem BloodSplashParticlesPrefab;

    public void Visit(PlayerHitInfo hitInfo)
    {
        Quaternion rotation = Quaternion.Euler(new(0, 0, hitInfo.Angle));
        Instantiate(BloodSplashParticlesPrefab, transform.position, rotation);
        //bloodSplash.transform.rotation = new(hitInfo.Angle, rotation.y, rotation.z, rotation.w);
    }

    public void Visit(EnemyHitInfo hitInfo)
    {
        var direction = DirectionHelper.GetXDirection(hitInfo.Source.position, transform.position);
        var zRotation = direction == Direction.Left ? Angles.StraightAngle : Angles.ZeroAngle; 

        Quaternion rotation = Quaternion.Euler(new(0, 0, zRotation));
        Instantiate(BloodSplashParticlesPrefab, transform.position, rotation);
    }
}