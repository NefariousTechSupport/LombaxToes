namespace LombaxToes.Editor
{
	public static class Program
	{
		public static NativeWindowSettings nws = new NativeWindowSettings()
		{
			Size = new Vector2i(1280, 720),
			Title = "Level Editor",
			Flags = ContextFlags.ForwardCompatible,
		};

		public static GameWindowSettings gws = new GameWindowSettings()
		{
			//Uncomment to introduce framerate cap
			//RenderFrequency = 60,
			//UpdateFrequency = 60,
			IsMultiThreaded = false		//Disable this and see what'll happen
		};

		public static void Main(string[] args)
		{
			using(Window mainWindow = new Window(gws, nws, args[0]))
			{
				mainWindow.Run();
			}
		}
	}
}