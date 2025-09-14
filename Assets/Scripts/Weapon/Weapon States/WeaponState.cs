using UnityEngine;

public class WeaponState : State
{
    protected Weapon weapon;
    protected Player player;

    public override void Initialize(StateMachine context, GameObject actor)
    {
        base.Initialize(context, actor);
        weapon = actor.GetComponent<Weapon>();
        player = Player.Instance;
    }
}
