namespace LombaxToes.Editor
{
	public class Window : GameWindow
	{
		string folder;

		public Window(GameWindowSettings gws, NativeWindowSettings nws, string folderPath) : base(gws, nws)
		{
			folder = folderPath;
		}

		Model model;

		Vector2 freecamLocal;

		protected override void OnLoad()
		{
			base.OnLoad();

			GL.ClearColor(0.1f, 0.1f, 0.1f, 0.0f);
			GL.Enable(EnableCap.DepthTest);
			GL.Enable(EnableCap.Texture2D);


			MaterialManager.LoadMaterial("standard.vert;standardunlit.frag", "Shaders/standard.vert.glsl", "Shaders/standardunlit.frag.glsl");
			AssetManager.LoadAssets(folder);

			//model = AssetManager.LoadIrb(0x04396A536A8F9378);	//Ratchet
			//model = AssetManager.LoadIrb(0x43A9B7A20CDCC90F);	//Aphelion
			//model = AssetManager.LoadIrb(0x04726A04067CC7C6);	//Sonic eruptor
			EntityManager.entities.Add(new Entity(Camera.transform.position, Vector3.Zero, Vector3.One, AssetManager.LoadIrb(0x28DAF9DDD21C6637)));	//Bronze Agorian Statue

			Camera.CreatePerspective(MathHelper.PiOver2);

			CursorGrabbed = true;
		}

		protected override void OnRenderFrame(FrameEventArgs args)
		{
			base.OnRenderFrame(args);

			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

			EntityManager.Render();

			SwapBuffers();
		}

		protected override void OnUpdateFrame(FrameEventArgs args)
		{
			if(KeyboardState.IsKeyDown(Keys.Escape)) Close();

			float moveSpeed = 20;
			float sensitivity = 1f;

			if(KeyboardState.IsKeyDown(Keys.LeftShift)) moveSpeed *= 10;

			if(KeyboardState.IsKeyDown(Keys.W)) Camera.transform.position += Camera.transform.Forward * (float)args.Time * moveSpeed;
			if(KeyboardState.IsKeyDown(Keys.A)) Camera.transform.position += Camera.transform.Right * (float)args.Time * moveSpeed;
			if(KeyboardState.IsKeyDown(Keys.S)) Camera.transform.position -= Camera.transform.Forward * (float)args.Time * moveSpeed;
			if(KeyboardState.IsKeyDown(Keys.D)) Camera.transform.position -= Camera.transform.Right * (float)args.Time * moveSpeed;

			freecamLocal += MouseState.Delta.Yx * (float)args.Time * sensitivity;

			freecamLocal.X = MathHelper.Clamp(freecamLocal.X, -MathHelper.PiOver2 + 0.0001f, MathHelper.PiOver2 - 0.0001f);

			Camera.transform.rotation = Quaternion.FromAxisAngle(Vector3.UnitX, freecamLocal.X) * Quaternion.FromAxisAngle(Vector3.UnitY, freecamLocal.Y);

			Title = $"Level Editor | FPS: {1f/(args.Time)}";

			base.OnUpdateFrame(args);
		}

		protected override void OnClosed()
		{
			//model.Dispose();

			base.OnClosed();
		}
	}
}