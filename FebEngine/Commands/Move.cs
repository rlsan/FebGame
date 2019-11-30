namespace FebEngine.Commands
{
  public class Move : Command
  {
    private float x;
    private float y;

    public Move(float x, float y)
    {
      this.x = x;
      this.y = y;
    }

    public override void Execute(Actor actor)
    {
      actor.Move(x, y);
    }
  }
}