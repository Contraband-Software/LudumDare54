namespace LD54.Engine.Components;

using Leviathan;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class BlackHoleComponent : Component
{
    private ILeviathanEngineService re;

    public Matrix Offset;

    public float Rotation = 0;

    private int light;

    private LeviathanSprite bh;
    private LeviathanSprite background;
    public BlackHoleComponent(string name, Game appCtx) : base(name, appCtx)
    {

    }


    public override void OnLoad(GameObject? parentObject)
    {
        gameObject = parentObject;
        re = this.app.Services.GetService<ILeviathanEngineService>();
        Offset = Matrix.CreateTranslation(-300,-300, 0);



        bh = new LeviathanSprite(this.app,Matrix.CreateTranslation(200,100,0),0,new Vector2(600),1,this.app.Content.Load<Texture2D>("Sprites/noiseTexture"), this.app.Content.Load<Texture2D>("Sprites/discnormal"), false,0);
        re.addSprite(bh);
        light = re.AddLight(new Vector2(200, 200), new Vector3(40000, 15000, 30000));

    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        bh.rotation -= 0.01f;
        bh.SetTransform(gameObject.GetGlobalTransform() * Offset);
        re.SetLightPosition(light, new Vector2(gameObject.GetGlobalPosition().X, gameObject.GetGlobalPosition().Y));
    }

    public override void OnUnload()
    {
        this.app.Services.GetService<ILeviathanEngineService>().removeSprite(bh);
        PrintLn("OnUnload: SpriteRendererComponent");
    }
}
