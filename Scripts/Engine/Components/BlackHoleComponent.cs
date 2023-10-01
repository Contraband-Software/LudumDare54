namespace LD54.Engine.Components;

using Leviathan;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class BlackHoleComponent : Component
{
    private ILeviathanEngineService re;

    public Matrix Offset;

    public float Rotation = 0;


    private LeviathanSprite bh;
    private LeviathanSprite background;
    public BlackHoleComponent(string name, Game appCtx) : base(name, appCtx)
    {

    }


    public override void OnLoad(GameObject? parentObject)
    {
        gameObject = parentObject;
        re = this.app.Services.GetService<ILeviathanEngineService>();
        Offset = Matrix.CreateTranslation(-100,-100, 0);



        bh = new LeviathanSprite(this.app,Matrix.CreateTranslation(200,100,0),0,new Vector2(200),1,this.app.Content.Load<Texture2D>("Sprites/image"),false,0);
        re.addSprite(bh);


    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        bh.SetTransform(gameObject.GetGlobalTransform() * Offset);
    }

    public override void OnUnload()
    {
        this.app.Services.GetService<ILeviathanEngineService>().removeSprite(bh);
        PrintLn("OnUnload: SpriteRendererComponent");
    }
}
