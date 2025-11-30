using System;
using System.Collections.Generic;
using System.Linq;
using MoreCommands.HandleProviders;

namespace MoreCommands;

public static class Prefabs
{
    private static readonly Levels _levels = new();
    private static readonly Regions _regions = new();
    private static readonly Subregions _subregions = new();
    private static readonly Entities _entities = new();
    private static readonly Items _items = new();
    
    public static Handle<M_Level> Levels()
    {
        return _levels.Handle();
    }
    public static Levels LevelProvider()
    {
        return _levels;
    }

    public static Handle<M_Region> Regions()
    {
        return _regions.Handle();
    }
    public static Regions RegionProvider()
    {
        return _regions;
    }

    public static Handle<M_Subregion> Subregions()
    {
        return _subregions.Handle();
    }
    public static Subregions SubregionProvider()
    {
        return _subregions;
    }

    public static Handle<GameEntity> Entities()
    {
        return _entities.Handle();
    }
    public static Entities EntityProvider()
    {
        return _entities;
    }

    public static Handle<Item> Items()
    {
        return _items.Handle();
    }
    public static Items ItemProvider()
    {
        return _items;
    }
}

public abstract class HandleProvider<T>
{
    public abstract Func<T, string> Name {get;}
    public virtual Func<T, T> Finalizer {get;} = obj => obj;
    public abstract Handle<T> Handle();
    
    public virtual T FromCommand(string[] args)
    {
        if (args.Length == 0)
        {
            Accessors.CommandConsoleAccessor.EchoToConsole($"Available {typeof(T).Name}:\n- {Handle().Join()}");
            return default;
        }
        var obj = Handle().Filter(args[0]).Any();
        if (obj == null)
        {
            Accessors.CommandConsoleAccessor.EchoToConsole($"No such {typeof(T).Name}: {args[0]}");
            return default;
        }
        Accessors.CommandConsoleAccessor.EchoToConsole($"Found {typeof(T).Name}: {Colors.Highlighted(Name(obj))}");
        return obj;
    }

    public virtual Handle<T> FromCommandMany(string[] args)
    {
        if (args.Length == 0)
        {
            Accessors.CommandConsoleAccessor.EchoToConsole($"Available {typeof(T).Name}:\n- {Handle().Join()}");
            return null;
        }
        IEnumerable<T> result = [];
        foreach (string arg in args)
        {
            var filtered = Handle().Filter(arg);
            if (filtered.Count() > 1)
            {
                Accessors.CommandConsoleAccessor.EchoToConsole($"Ambiguous {typeof(T).Name}: {arg} matches {filtered.Join()}\nchoosing first");
            }
            var obj = filtered.Any();
            if (obj == null)
            {
                Accessors.CommandConsoleAccessor.EchoToConsole($"No such {typeof(T).Name}: {arg}");
                return null;
            }
            result.Append(obj);
        }
        return new Handle<T>(result, Name, Finalizer);
    }
}

public class Handle<T>(IEnumerable<T> d, Func<T, string> nameGetter, Func<T, T> finalizer)
{
    private readonly IEnumerable<T> _data = d;
    private readonly Func<T, string> _nameGetter = x => nameGetter(x)?.ToLower() ?? null;
    private readonly Func<T, T> _finalizer = finalizer;

    public Handle<T> Filter(string filter)
    {
        return new(_data.Where(x => Helpers.Substr(_nameGetter(x), filter.ToLower())), _nameGetter, _finalizer);
    }

    public T Any()
    {
        var result = _data.FirstOrDefault();
        return result != null ? _finalizer(result) : default;
    }

    public string AnyName()
    {
        return _nameGetter(Any());
    }

    public string[] Names()
    {
        return [.. _data.Select(x => _nameGetter(x))];
    }

    public string Join(string delimiter = "\n- ")
    {
        return string.Join(delimiter, Names());
    }

    public List<T> ToList()
    {
        return [.. _data];
    }

    public IEnumerable<T> Data()
    {
        return _data;
    }

    public int Count()
    {
        return _data.Count();
    }
}
