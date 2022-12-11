using System.Collections.Generic;

public static class Utils
{
	public static T Random<T>(this List<T> list)
	{
		if (list.Count == 0)
			throw new System.IndexOutOfRangeException("List contains no elements");

		return list[UnityEngine.Random.Range(0, list.Count)];
	}
}