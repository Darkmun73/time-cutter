public interface IHitInfoVisitor
{
    void Visit(PlayerHitInfo hitInfo);
    void Visit(EnemyHitInfo hitInfo);
}