using System;
using System.Collections.Generic;
using System.Linq;
using MoreCommands.HandleProviders;

namespace MoreCommands;

public static class Prefabs
{
    private static readonly Regions _regions = new();
    private static readonly Subregions _subregions = new();
    private static readonly Entities _entities = new();
    private static readonly Items _items = new();

    public static Handle<M_Region> Regions() => _regions.Handle();
    public static Regions RegionProvider() => _regions;

    public static Handle<M_Subregion> Subregions() => _subregions.Handle();
    public static Subregions SubregionProvider() => _subregions;

    public static Handle<GameEntity> Entities() => _entities.Handle();

    public static Handle<Item> Items() => _items.Handle();
    public static Items ItemProvider() => _items;
}

public abstract class HandleProvider<T>
{
    public abstract Func<T, string> Name {get;}
    public abstract Handle<T> Handle();
    
    public virtual T FromCommand(string[] args)
    {
        if (args.Length == 0)
        {
            Accessors.CommandConsoleAccessor.EchoToConsole($"Available {typeof(T).Name}:\n- {Handle().Join()}");
            return default;
        }
        var obj = Handle().FilterExact(args[0]).Any();
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
            return new Handle<T>([], Name);
        }
        List<T> result = [];
        foreach (string arg in args)
        {
            T obj = Handle().FilterExact(arg).Any();
            if (obj == null)
            {
                Accessors.CommandConsoleAccessor.EchoToConsole($"No such {typeof(T).Name}: {arg}");
                return new Handle<T>([], Name);
            }
            result.Add(obj);
        }
        return new Handle<T>(result, Name);
    }
}

public class Handle<T>(IEnumerable<T> data, Func<T, string> nameGetter)
{
    private readonly IEnumerable<T> _data = data;
    private readonly Func<T, string> _nameGetter = nameGetter;

    private string GetName(T value)
    {
        return _nameGetter(value)?.ToLowerInvariant();
    }

    public Handle<T> Filter(string filter)
    {
        return new(_data.Where(value => Helpers.Substr(GetName(value), filter)), _nameGetter);
    }

    public Handle<T> FilterExact(string name)
    {
        return new(
            _data.Where(value => string.Equals(GetName(value), name, StringComparison.OrdinalIgnoreCase)),
            _nameGetter);
    }

    public T Any()
    {
        return _data.FirstOrDefault();
    }

    public string AnyName()
    {
        return GetName(Any());
    }

    public string[] Names()
    {
        return [.. _data.Select(GetName)];
    }

    public string Join(string delimiter = "\n- ")
    {
        return string.Join(delimiter, Names());
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
