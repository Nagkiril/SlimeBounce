
namespace SlimeBounce.Slime
{
    //Deployable slime guarantees that when we're spawning this slime via Player's choice, it will not produce irregular behaviour (can be picked up by Player and dragged around from his spot, et cetera)
    //As such certain slimes cannot be deployable, i.e. vanilla Protector (normally doesn't allow dragging unless his shield is broken) and Popping (instantly explodes, thus useless as a deployable)
    //If we plan to deploy a certain slime, we make a conscious choice of promoting his core to this class, and if irratic behavior (such as previous examples) is somehow desired, then we require its explicit accomodation
    public class DeployableSlime : SlimeCore
    {
        protected override void OnClickStart()
        {
            base.OnClickStart();
            PickByPlayer();
        }

        protected override void OnClickEnd()
        {
            base.OnClickEnd();
            DropByPlayer();
        }
    }
}
