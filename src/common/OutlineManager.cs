using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UIElements.Collections;
using System;

namespace MoreCommands.Outlines;

public static class OutlinesController
{
    private static Dictionary<GameEntity, Renderer[]> _trackedEntities = [];

    private static Dictionary<string, Color> _activeOutlines = [];
    private static readonly Dictionary<string, Color> _defaultOutlines = new()
    {
        {"item_injector", new Color(0.3f, 0.9f, 0.3f)}, // light green
        {"item_blinkeye", Color.magenta},
        {"denizen_sluggrub", Color.yellow},
        {"item_pillbottle", Color.cyan},
        {"item_food_bar", new Color(0.6f, 0.3f, 0)}, // brown
    };

    public static bool ToggleDefault()
    {
        if (new HashSet<string>(_activeOutlines.Keys, StringComparer.OrdinalIgnoreCase).SetEquals(_defaultOutlines.Keys)) _activeOutlines.Clear();
        else _activeOutlines = _defaultOutlines.Copy();
        RefreshAll();
        return _activeOutlines.Count > 0;
    }

    public static bool IsEnabled(string entityIdLower)
    {
        Plugin.Assert(entityIdLower == entityIdLower.ToLower());

        return _activeOutlines.ContainsKey(entityIdLower);
    }

    public static Color EnableOutlines(string entityIdLower, Color color)
    {
        Plugin.Assert(entityIdLower == entityIdLower.ToLower());
        Plugin.Assert(!IsEnabled(entityIdLower));

        Color resultColor = _activeOutlines[entityIdLower] = _defaultOutlines.Get(entityIdLower, color);
        RefreshAll();
        return resultColor;
    }

    public static bool DisableOutlines(string entityIdLower)
    {
        Plugin.Assert(entityIdLower == entityIdLower.ToLower());

        bool removed = _activeOutlines.Remove(entityIdLower);
        if (removed) RefreshAll();
        return removed;
    }

    public static void RegisterEntity(GameEntity entity)
    {
        if (entity == null || _trackedEntities.ContainsKey(entity)) return;

        var renderers = entity.GetComponentsInChildren<Renderer>(true)
            .Where(r => !(r is ParticleSystemRenderer || r is TrailRenderer))
            .ToArray();

        if (renderers.Length > 0)
        {
            _trackedEntities.Add(entity, renderers);
            RefreshSingle(entity, renderers);
        }
    }

    public static void UnregisterEntity(GameEntity entity)
    {
        if (entity != null) _trackedEntities.Remove(entity);
    }


    private static void RefreshAll()
    {
        foreach (var kvp in _trackedEntities.ToList()) 
        {
            RefreshSingle(kvp.Key, kvp.Value);
        }
    }

    private static void RefreshSingle(GameEntity entity, Renderer[] renderers)
    {
        bool shouldHighlight = _activeOutlines.TryGetValue(entity.entityPrefabID.ToLower(), out Color targetColor);
        Material outlinesMat = shouldHighlight ? OutlinesMaterialFactory.Get(targetColor) : null;

        foreach (var r in renderers)
        {
            if (r == null) continue;

            List<Material> existingMats = [.. r.sharedMaterials];
            bool changed = false;

            int removed = existingMats.RemoveAll(m => m != null && m.shader.name == OutlinesMaterialFactory.SHADER_NAME); 
            if (removed > 0) changed = true;

            if (shouldHighlight && outlinesMat != null)
            {
                existingMats.Add(outlinesMat);
                changed = true;
            }

            if (changed) r.materials = [.. existingMats];
        }
    }
    
    public static void ClearAll()
    {
         _trackedEntities.Clear();
         _activeOutlines.Clear();
    }
}

public static class OutlinesMaterialFactory
{
    public const string SHADER_NAME = "GUI/Text Shader";
    private static Dictionary<Color, Material> _cache = [];

    public static Material Get(Color color)
    {
        if (_cache.TryGetValue(color, out var mat) && mat != null) return mat;

        // Имя шейдера используется как "тег" для удаления в RefreshSingle
        var shader = Shader.Find(SHADER_NAME);
        var newMat = new Material(shader);
        
        newMat.SetInt("unity_GUIZTestMode", (int)UnityEngine.Rendering.CompareFunction.Always);
        newMat.SetInt("_ZTest", (int)UnityEngine.Rendering.CompareFunction.Always);
        newMat.SetInt("_ZWrite", 0);
        newMat.color = color;
        newMat.mainTexture = Texture2D.whiteTexture;
        newMat.renderQueue = 5000;

        _cache[color] = newMat;
        return newMat;
    }
}
