namespace LD54.Scripts.Engine;

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class SceneController
{
    private List<Scene> scenes = new();

    private Scene? activeScene;
    private Scene? nextScene;

    public Scene? GetCurrentScene()
    {
        if (this.activeScene == null)
        {
            return null;
        }
        return this.activeScene;
    }

    public void AddScene(Scene scene)
    {
        this.scenes.Add(scene);
    }

    public void ChangeScene(string next)
    {
        Scene nextScene = this.GetSceneByName(next);

        if(this.activeScene != nextScene)
        {
            this.nextScene = nextScene;
        }
    }

    public void Update(GameTime gameTime)
    {
        if (this.nextScene != null)
        {
            this.TransitionScene();
        }

        if(this.activeScene != null)
        {
            this.activeScene.Update(gameTime);
        }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        //  If there is an active scene, draw it.
        if(this.activeScene != null)
        {
            this.activeScene.Draw(spriteBatch);
        }
    }

    private Scene GetSceneByName(string scene)
    {
        foreach (Scene s in this.scenes)
        {
            if (s.GetName() == scene)
            {
                return s;
            }
        }

        throw new ArgumentException("No such scene with that name");
    }

    private void TransitionScene()
    {
        if(this.activeScene != null)
        {
            this.activeScene.UnloadContent();
        }

        //  Perform a garbage collection to ensure memory is cleared
        GC.Collect();

        this.activeScene = this.nextScene;

        this.nextScene = null;

        // guaranteed to be not null by ChangeScene function not having a nullable (?) parameter
        this.activeScene?.Initialize();
    }
}
