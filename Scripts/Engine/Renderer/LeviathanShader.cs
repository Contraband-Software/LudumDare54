using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace LD54.Engine.Leviathan;
public class LeviathanShader
{
    public List<float> attributeValues;
    public List<string> attributes;
    private Game game;
    Effect shader;
    public LeviathanShader(Game game, string path)
    {
        this.game = game;
        shader = game.Content.Load<Effect>(path);
    }
    public void setAllParams()
    {
        for (int i = 0; i < attributes.Count; i++)
        {
            shader.Parameters[attributes[i]]?.SetValue(attributeValues[i]);
        }
    }
    public void updateParam(string name, float value)
    {
        attributeValues[attributes.IndexOf(name)] = value;
    }
    public void addParam(string name, float value)
    {
        attributeValues.Add(value);
        attributes.Add(name);
    }
}