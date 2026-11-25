public class ProjectilePool : Pool<Projectile>
{
    public override Projectile Get()
    {
        Projectile projectile = base.Get();
        projectile.Initialize(this);
        return projectile;
    }
}
