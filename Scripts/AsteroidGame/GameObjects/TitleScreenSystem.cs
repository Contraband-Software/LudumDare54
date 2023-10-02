namespace LD54.AsteroidGame.GameObjects;

using Engine.Leviathan;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class TitleScreenSystem : GameObject
{
    private ILeviathanEngineService? render;

    private SpriteFont titleFont;
    private SpriteFont subTitleFont;

    private readonly int showTime = 0;
    private float timeShowed = 0;

    public TitleScreenSystem(int showTime, SpriteFont titleFont, SpriteFont subtitleFont, Game appCtx) : base("TitleScreenObject", appCtx)
    {
        this.titleFont = titleFont;
        this.subTitleFont = subtitleFont;
        this.showTime = showTime;
    }

    private LeviathanUIElement gameTitle;
    private LeviathanUIElement gameSubtitle1;
    private LeviathanUIElement gameSubtitle2;
    public override void OnLoad(GameObject? parentObject)
    {
        render = this.app.Services.GetService<ILeviathanEngineService>();

        {
            Matrix fuckingTitlePosition = this.GetGlobalTransform();

            Vector2 titlePos = this.render.getWindowSize() / 2f;
            string titleText = "EVENT HORIZON";
            float titleScale = 1;
            fuckingTitlePosition.Translation += new Vector3(titlePos - titleFont.MeasureString(titleText) / 2 * titleScale, 0);
            gameTitle = new LeviathanUIElement(this.app, fuckingTitlePosition, new Vector2(titleScale), titleText, this.titleFont, Color.White);
        }
        {
            Matrix fuckingTitlePosition = this.GetGlobalTransform();

            Vector2 subTitle1Pos = this.render.getWindowSize() / 2f + new Vector2(0, 100);
            string subTitle1Text = "By Contraband Studio, 2023, for Ludum Dare";
            fuckingTitlePosition.Translation += new Vector3(subTitle1Pos - subTitleFont.MeasureString(subTitle1Text) / 2, 0);
            gameSubtitle1 = new LeviathanUIElement(this.app, fuckingTitlePosition, new Vector2(1), subTitle1Text, this.subTitleFont, Color.White);
        }
        {
            Matrix fuckingTitlePosition = this.GetGlobalTransform();

            Vector2 subTitle1Pos = this.render.getWindowSize() / 2f + new Vector2(0, 200);
            string subTitle1Text = "With the Poo Machine Game Engine";
            fuckingTitlePosition.Translation += new Vector3(subTitle1Pos - subTitleFont.MeasureString(subTitle1Text) / 2, 0);
            gameSubtitle2 = new LeviathanUIElement(this.app, fuckingTitlePosition, new Vector2(1), subTitle1Text, this.subTitleFont, Color.White);
        }
    }

    private bool showedTitle = false;
    private bool showedEngine = false;
    private bool showedStudio = false;
    public override void Update(GameTime gameTime)
    {
        int increment = this.showTime / 4;

        timeShowed = gameTime.TotalGameTime.Seconds;

        if (this.timeShowed > increment && !showedTitle)
        {
            showedTitle = true;
            this.render.addUISprite(gameTitle);
        } else if (this.timeShowed > increment * 2 && !showedStudio)
        {
            showedStudio = true;
            this.render.addUISprite(gameSubtitle1);

        } else if (this.timeShowed > increment * 3 && !showedEngine)
        {
            showedEngine = true;
            this.render.addUISprite(gameSubtitle2);
        } else if (this.timeShowed > increment * 4)
        {
            this.app.Services.GetService<ISceneControllerService>().ChangeScene("GameScene");
        }

        base.Update(gameTime);
    }

    public override void OnUnload()
    {
        this.render.removeUISprite(gameTitle);
        this.render.removeUISprite(gameSubtitle1);
        this.render.removeUISprite(gameSubtitle2);

        base.OnUnload();
    }
}
