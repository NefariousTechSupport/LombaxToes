namespace LombaxToes.Editor
{
	public class Window : GameWindow
	{
		public static int width;
		public static int height;

		string folder;
		Vector2 freecamLocal;
		Capabilities[] caps;

		public enum Capabilities
		{
			Transparency,
			BackfaceCulling,
		}

		public Window(GameWindowSettings gws, NativeWindowSettings nws, string folderPath, params Capabilities[] caps) : base(gws, nws)
		{
			this.caps = caps;
			folder = folderPath;
		}

		protected override void OnLoad()
		{
			base.OnLoad();

			GL.ClearColor(0.1f, 0.1f, 0.1f, 0.0f);
			GL.Enable(EnableCap.DepthTest);
			GL.Enable(EnableCap.Texture2D);

			if(caps.Any(x => x == Capabilities.Transparency))
			{
				GL.Enable(EnableCap.Blend);
				GL.BlendFuncSeparate(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha, BlendingFactorSrc.One, BlendingFactorDest.Zero);
			}

			if(caps.Any(x => x == Capabilities.BackfaceCulling))
			{
				GL.Enable(EnableCap.CullFace);
			}

			MaterialManager.LoadMaterial("standard.vert;standardunlit.frag", "Shaders/standard.vert.glsl", "Shaders/standardunlit.frag.glsl");
			MaterialManager.LoadMaterial("standard.vert;standardwhite.frag", "Shaders/standard.vert.glsl", "Shaders/standardwhite.frag.glsl");

			AssetManager.LoadAssets(folder);

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
			if(KeyboardState.IsKeyDown(Keys.Space))
			{
				CursorGrabbed = true;
				CursorVisible = false;
			}
			if(MouseState.IsButtonDown(MouseButton.Left))
			{
				CursorGrabbed = false;
				CursorVisible = true;
			}

			float moveSpeed = 20;
			float sensitivity = 0.01f;

			if(KeyboardState.IsKeyDown(Keys.LeftShift)) moveSpeed *= 10;

			if(KeyboardState.IsKeyDown(Keys.W)) Camera.transform.Position += Camera.transform.Forward * (float)args.Time * moveSpeed;
			if(KeyboardState.IsKeyDown(Keys.A)) Camera.transform.Position += Camera.transform.Right * (float)args.Time * moveSpeed;
			if(KeyboardState.IsKeyDown(Keys.S)) Camera.transform.Position -= Camera.transform.Forward * (float)args.Time * moveSpeed;
			if(KeyboardState.IsKeyDown(Keys.D)) Camera.transform.Position -= Camera.transform.Right * (float)args.Time * moveSpeed;

			freecamLocal += MouseState.Delta.Yx * sensitivity;

			freecamLocal.X = MathHelper.Clamp(freecamLocal.X, -MathHelper.PiOver2 + 0.0001f, MathHelper.PiOver2 - 0.0001f);

			Camera.transform.Rotation = Quaternion.FromAxisAngle(Vector3.UnitX, freecamLocal.X) * Quaternion.FromAxisAngle(Vector3.UnitY, freecamLocal.Y);

			Title = $"Level Editor | FPS: {1f/(args.Time)}";

			base.OnUpdateFrame(args);
		}

		protected override void OnResize(ResizeEventArgs e)
		{
			base.OnResize(e);

			width = this.ClientRectangle.Size.X;
			height = this.ClientRectangle.Size.Y;

			GL.Viewport(0, 0, width, height);
			Camera.CreatePerspective(MathHelper.PiOver2);
		}

		protected override void OnClosed()
		{
			//model.Dispose();

			base.OnClosed();
		}
	}
}