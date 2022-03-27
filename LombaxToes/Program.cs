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
			string input = string.Empty;
			List<Window.Capabilities> caps = new List<Window.Capabilities>();

			for(int i = 0; i < args.Length; i++)
			{
				if(args[i][0] == '-')
				{
					switch(args[i].Substring(1))
					{
						case "transparency":
							caps.Add(Window.Capabilities.Transparency);
							break;
						case "backface-culling":
							caps.Add(Window.Capabilities.BackfaceCulling);
							break;
						case "i":
							if(i + 1 == args.Length)
							{
								PrintHelpMessage("Please specify a folder.");
								return;
							}
							input = args[i + 1];
							i++;
							break;
						default:
							PrintHelpMessage("Unrecognised Switch.");
							return;
					}
				}
				else
				{
					PrintHelpMessage("Please specify a folder.");
					return;
				}
			}

			using(Window mainWindow = new Window(gws, nws, input, caps.ToArray()))
			{
				mainWindow.Run();
			}
		}
		static void PrintHelpMessage(string reason)
		{
			Console.WriteLine(
				reason + "\n" +
				"LombaxToes [options] -i <folder containing assetlookup.dat>\n" + 
				"\noptions:\n" + 
				"  -transparency     : enables transparency\n" +
				"  -backface-culling : enables backface culling\n"
			);
		}
	}
}