using UnityEngine;
using System;
using System.Linq;
using CustomArchitecture;

public class VisualEffectManager : BaseBehaviour
{
    [Serializable]
    public class ParticleDictionary
    {
        public string name;
        public GameObject prefab;
    }

    [Serializable]
    public class SpriteDictionary
    {
        public string name;
        public Sprite sprite;
    }

    public ParticleDictionary[] prefabs;
    public SpriteDictionary[] sprites;
    public Transform trash;

    public GameObject GetPrefab(string name)
    {
        return this.prefabs.Where(p => p.name == name).FirstOrDefault().prefab;
    }

    public Sprite GetSprite(string name)
    {
        return this.sprites.Where(s => s.name == name).FirstOrDefault().sprite;
    }

    public void PlayVfx(GameObject prefab, Vector3 position, Quaternion rotation, float size)
    {
        GameObject vfx = Instantiate(prefab, position, rotation, this.trash);
        vfx.transform.localScale = Vector3.one * size;
    }

    public void GetNPlayVfx(string name, Vector3 position, Quaternion rotation, float size = 1)
    {
        GameObject vfx = this.GetPrefab(name);
        this.PlayVfx(vfx, position, rotation, size);
    }

    public GameObject PlayNGetVfx(string name, Vector3 position, float size = 1)
    {
        GameObject vfxObj = this.GetPrefab(name);
        GameObject vfx = Instantiate(vfxObj, position, vfxObj.transform.rotation, this.trash);
        vfx.transform.localScale = Vector3.one * size;

        return vfx;
    }

    public GameObject PlayNGetVfx(string name, Vector3 position, Quaternion rotation, float size = 1)
    {
        GameObject vfxObj = this.GetPrefab(name);

        GameObject vfx = Instantiate(vfxObj, position, rotation, this.trash);
        vfx.transform.localScale = Vector3.one * size;

        return vfx;
    }

    public void PlayVfxWithColor(GameObject prefab, Vector3 position, Quaternion rotation, float size, Color color)
    {
        GameObject vfx = Instantiate(prefab, position, rotation, this.trash);
        vfx.transform.localScale = Vector3.one * size;
        ParticleSystem[] ps = vfx.GetComponentsInChildren<ParticleSystem>();
        foreach (var item in ps)
        {
            var settings = item.main;
            float initAlpha = settings.startColor.color.a;
            Color finalColor = new Color(color.r, color.g, color.b, initAlpha);
            settings.startColor = new ParticleSystem.MinMaxGradient(finalColor);
        }
    }

    public void SetColorAll(GameObject prefabVFX, Color color)
    {    //,bool composite = false
        ParticleSystem[] ps = prefabVFX.GetComponentsInChildren<ParticleSystem>();
        foreach (var item in ps)
        {
            ParticleSystem.MainModule settings = item.main;
            float initAlpha = settings.startColor.color.a;
            Color finalColor = new Color(color.r, color.g, color.b, initAlpha);
            if (settings.startColor.color != new Color(1, 1, 1, initAlpha))
            {
                settings.startColor = finalColor;
            }

            ParticleSystem.ColorOverLifetimeModule cotSetting = item.colorOverLifetime;
            Gradient g = cotSetting.color.gradient;
            Gradient n = new Gradient();
            n.SetKeys(g.colorKeys, g.alphaKeys);

            GradientColorKey[] keys = new GradientColorKey[g.colorKeys.Length];
            for (int j = 0; j < keys.Length; j++)
            {
                keys[j].time = g.colorKeys[j].time;
                keys[j].color = color;
            }

            n.SetKeys(keys, g.alphaKeys);
            cotSetting.color = n;
        }
    }

    public void SetColorMain(ParticleSystem prefabVFX, Color color)
    {
        ParticleSystem.MainModule settings = prefabVFX.main;
        float initAlpha = settings.startColor.color.a;
        Color finalColor = new Color(color.r, color.g, color.b, initAlpha);
        settings.startColor = finalColor;
    }

    public override void Pause(bool pause = true)
    {
        foreach (Transform child in trash)
        {
            GameObject objVfx = child?.gameObject;

            if (!objVfx)
            {
                continue;
            }

            ParticleSystem[] pSystems = objVfx.GetComponentsInChildren<ParticleSystem>();
            foreach (ParticleSystem pSystem in pSystems)
            {
                if (pause)
                {
                    pSystem.Pause();
                }
                else
                {
                    pSystem.Play();
                }
            }

        }
        base.Pause(pause);
    }

    private void OnDestroy()
    {
        foreach (Transform child in trash)
        {
            GameObject objVfx = child?.gameObject;
            if (!objVfx)
            {
                continue;
            }
            Destroy(objVfx);
        }
    }
}

