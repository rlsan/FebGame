namespace Fubar.Commands
{
  public class Jump : Command
  {
    public override void Execute(Actor actor)
    {
      actor.Jump();
    }
  }
}