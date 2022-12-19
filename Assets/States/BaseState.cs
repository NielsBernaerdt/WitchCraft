public abstract class BaseState
{
	protected BasePawn _pawn;
	public abstract BaseState Update();
	public abstract void OnEnter();
	public abstract void OnExit();
}