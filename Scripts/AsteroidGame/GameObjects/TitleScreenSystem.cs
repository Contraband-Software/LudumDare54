namespace LD54.AsteroidGame.GameObjects;

using Engine.Leviathan;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class TitleScreenSystem : GameObject
{
    private ILeviathanEngineService? render;

    private SpriteFont font;

    private LeviathanUIElement gameTitle;

    public TitleScreenSystem(SpriteFont font, Game appCtx) : base("TitleScreenObject", appCtx)
    {
        this.font = font;
    }

    public override void OnLoad(GameObject? parentObject)
    {
        render = this.app.Services.GetService<ILeviathanEngineService>();

        Vector2 titlePos = this.render.getWindowSize() / 2f;
        string titleText = "EVENT HORIZON";
        float titleScale = 10;

        Matrix fuckingTitlePosition = this.GetGlobalTransform();
        // fuckingTitlePosition.Translation += new Vector3(titlePos - font.MeasureString(), 0);
        gameTitle = new LeviathanUIElement(this.app, fuckingTitlePosition, new Vector2(titleScale), titleText, this.font, Color.White);

        this.render.addUISprite(gameTitle);
    }

    public override void OnUnload()
    {
        this.render.removeUISprite(gameTitle);

        base.OnUnload();
    }
}
