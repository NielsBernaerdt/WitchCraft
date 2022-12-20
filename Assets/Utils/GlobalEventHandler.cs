using System;

public class GlobalEventHandler : MySingleton<GlobalEventHandler>
{
	public event EventHandler<int> OnElixirGenerated;

	private void InvokeEvent(EventHandler<int> eventHandler, object sender, int eventArgs)
	{
		EventHandler<int> handler = eventHandler;
		handler?.Invoke(sender, eventArgs);
	}

	public void InvokeOnElixirGenerated(object sender, int value) => InvokeEvent(OnElixirGenerated, sender, value);
}