namespace LD54.Engine.Leviathan;

using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

interface ILeviathanEngineService
{
    public int addSprite(LevithanSprite sprite);

    public void removeSprite(int index);

    public int AddLight(Vector2 position, Vector3 color);

    public void removeLight(int id);

    public void setLightColor(int id, Color color);

    public Vector3 getLightColor(int id);

    public Vector2 getLightPosition(int id);

    public void updateLightPosition(int id, Vector2 offset);
}

public class LeviathanEngine : DrawableGameComponent, ILeviathanEngineService
{

    private Vector2[] lightPositions = new Vector2[64];
    private Vector3[] lightColors = new Vector3[64];
    private Queue<int> openLocations = new Queue<int>();
    private IGraphicsDeviceManager graphics;
    private SpriteBatch spriteBatch;
    private Effect lightingShader;
    private Game game;

    RenderTarget2D colorTarget;
    RenderTarget2D normalTarget;
    RenderTarget2D litTarget;
    RenderTarget2D postProcessTarget;
    private bool pingpong = false;

    public List<LevithanSprite> sprites = new List<LevithanSprite>();
    public List<LeviathanShader> postProcessShaders = new List<LeviathanShader>();

    public LeviathanEngine(Game g) : base(g)
    {
        game = g;
        graphics = g.Services.GetService<IGraphicsDeviceManager>();
        //graphics.GraphicsProfile = GraphicsProfile.HiDef;
        for (int i = 0; i < 64; i++)
        {
            openLocations.Enqueue(i);
        }

    }

    public override void Initialize()
    {
        colorTarget = new RenderTarget2D(game.GraphicsDevice,
            game.GraphicsDevice.PresentationParameters.BackBufferWidth,
            game.GraphicsDevice.PresentationParameters.BackBufferHeight);
        normalTarget = new RenderTarget2D(game.GraphicsDevice,
            game.GraphicsDevice.PresentationParameters.BackBufferWidth,
            game.GraphicsDevice.PresentationParameters.BackBufferHeight);
        litTarget = new RenderTarget2D(game.GraphicsDevice,
            game.GraphicsDevice.PresentationParameters.BackBufferWidth,
            game.GraphicsDevice.PresentationParameters.BackBufferHeight);
        postProcessTarget = new RenderTarget2D(game.GraphicsDevice,
            game.GraphicsDevice.PresentationParameters.BackBufferWidth,
            game.GraphicsDevice.PresentationParameters.BackBufferHeight);

        spriteBatch = new SpriteBatch(game.GraphicsDevice);
        lightingShader = game.Content.Load<Effect>("Shaders/lighting");
    }

    public int addPostProcess(LeviathanShader shader)
    {
        postProcessShaders.Add(shader);
        return postProcessShaders.IndexOf(shader);
    }
    public void removePostProcess(int i)
    {
        postProcessShaders.RemoveAt(i);
    }

    public int addSprite(LevithanSprite sprite)
    {
        sprites.Add(sprite);
        return sprites.IndexOf(sprite);
    }
    public void removeSprite(int index)
    {
        sprites.RemoveAt(index);
    }

    //public override void LoadContent()
    //{
    //}

    public override void Draw(GameTime gameTime)
    {
        Matrix view = Matrix.Identity * Matrix.CreateTranslation(0, 0, 0);

        int width = game.GraphicsDevice.Viewport.Width;
        int height = game.GraphicsDevice.Viewport.Height;
        Matrix projection = Matrix.CreateOrthographicOffCenter(0, width, height, 0, 0, 1);
        Vector3 translation;
        view.Decompose(out _, out _, out translation);

        game.GraphicsDevice.SetRenderTarget(colorTarget);
        game.GraphicsDevice.Clear(Color.Black);
        spriteBatch.Begin(transformMatrix: view);

        foreach (LevithanSprite sprite in sprites)
        {
            spriteBatch.Draw(sprite.color, new Rectangle(sprite.getPositionXY().ToPoint(), sprite.size), Color.White);
        }

        spriteBatch.End();

        game.GraphicsDevice.SetRenderTarget(normalTarget);
        game.GraphicsDevice.Clear(new Color(0.5f, 0.5f, 1f));
        spriteBatch.Begin(transformMatrix: view);

        foreach (LevithanSprite sprite in sprites)
        {
            if (sprite.useNormal)
            {
                spriteBatch.Draw(sprite.normal, new Rectangle(sprite.getPositionXY().ToPoint(), sprite.size), Color.White);
            }
        }

        spriteBatch.End();

        lightingShader.Parameters["translation"]?.SetValue(new Vector2(translation.X, translation.Y));
        lightingShader.Parameters["viewProjection"]?.SetValue(projection);
        lightingShader.Parameters["time"]?.SetValue((float)gameTime.TotalGameTime.TotalSeconds);
        lightingShader.Parameters["width"]?.SetValue(width);
        lightingShader.Parameters["height"]?.SetValue(height);
        lightingShader.Parameters["lightColors"]?.SetValue(lightColors);
        lightingShader.Parameters["lightPositions"]?.SetValue(lightPositions);
        lightingShader.Parameters["normalSampler"]?.SetValue(normalTarget);


        game.GraphicsDevice.SetRenderTarget(litTarget);
        game.GraphicsDevice.Clear(Color.Black);
        spriteBatch.Begin(effect: lightingShader);
        spriteBatch.Draw(colorTarget, new Vector2(0), Color.White);
        spriteBatch.End();

        game.GraphicsDevice.SetRenderTarget(null);
        game.GraphicsDevice.Clear(Color.Black);

        foreach (LeviathanShader postProcess in postProcessShaders)
        {
            postProcess.shader.Parameters["viewProjection"]?.SetValue(projection);
            postProcess.shader.Parameters["time"]?.SetValue((float)gameTime.TotalGameTime.TotalSeconds);
            postProcess.shader.Parameters["width"]?.SetValue(width);
            postProcess.shader.Parameters["height"]?.SetValue(height);
            postProcess.SetAllParams();
            if (pingpong)
            {
                game.GraphicsDevice.SetRenderTarget(litTarget);
            }
            else
            {
                game.GraphicsDevice.SetRenderTarget(postProcessTarget);
            }

            game.GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin(effect: postProcess.shader);
            if (pingpong)
            {
                spriteBatch.Draw(postProcessTarget, new Vector2(0), Color.White);
            }
            else
            {
                spriteBatch.Draw(litTarget, new Vector2(0), Color.White);
            }
            spriteBatch.End();
            pingpong = !pingpong;
        }
        game.GraphicsDevice.SetRenderTarget(null);
        game.GraphicsDevice.Clear(Color.Black);
        spriteBatch.Begin();
        if (pingpong)
        {
            spriteBatch.Draw(postProcessTarget, new Vector2(0), Color.White);
        }
        else
        {
            spriteBatch.Draw(litTarget, new Vector2(0), Color.White);
        }
        spriteBatch.End();
    }

    //public override void Update(GameTime gameTime)
    //{

    //}

    public int AddLight(Vector2 position, Vector3 color)
    {
        int location = openLocations.Dequeue();
        lightPositions[location] = position;
        lightColors[location] = color;
        return location;
    }
    public void removeLight(int id)
    {
        lightColors[id] = new Vector3(0);
        openLocations.Enqueue(id);
    }

    public void setLightColor(int id,  Color color) {
        lightColors[id] = color.ToVector3();
    }
    public void setLightPosition(int id, Vector2 position)
    {
        lightPositions[id] = position;
    }
    public Vector3 getLightColor(int id)
    {
        return lightColors[id];
    }
    public Vector2 getLightPosition(int id)
    {
        return lightPositions[id];
    }
    public void updateLightPosition(int id, Vector2 offset)
    {
        lightPositions[id] += offset;
    }
}
