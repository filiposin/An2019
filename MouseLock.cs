public static class MouseLock
{
	private static bool mouseLocked;

	public static bool MouseLocked
	{
		get
		{
			return mouseLocked;
		}
		set
		{
			mouseLocked = value;
		}
	}
}
