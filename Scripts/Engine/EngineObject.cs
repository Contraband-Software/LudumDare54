namespace LD54.Scripts.Engine;

using System.Collections.Generic;

public abstract class EngineObject
{
    protected string name;

#pragma warning disable CA1051
    public List<string> Tags = new();
#pragma warning restore CA1051

    public EngineObject(string name)
    {
        this.name = name;
    }

    public string GetName()
    {
        return this.name;
    }
}
