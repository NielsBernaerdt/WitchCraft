using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
	[SerializeField] private Slider _elixirSlider;

	private void Awake()
	{
		_elixirSlider = GetComponentInChildren<Slider>();
		GlobalEventHandler.Instance.OnElixirGenerated += UpdateSlider;
	}

	private void UpdateSlider(object sender, int value)
	{
		_elixirSlider.value = value / 100f;
	}
}
