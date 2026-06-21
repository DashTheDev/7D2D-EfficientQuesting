using System.IO;
using System.Reflection;
using HarmonyLib;
using Newtonsoft.Json;

namespace EfficientQuesting;

public partial class EfficientQuestingMod : IModApi
{
    public static Mod ModInstance { get; private set; }
    public static EfficientQuestingConfig Config { get; private set; }
    public static bool IsDebug => Config is not null && Config.IsDebug;

    public void InitMod(Mod _modInstance)
    {
        ModInstance = _modInstance;
        Config = new EfficientQuestingConfig();
        LoadConfig();

        Harmony harmony = new(_modInstance.Name);
        harmony.PatchAll(Assembly.GetExecutingAssembly()); 
    }

    private void LoadConfig()
    {
        string path = Path.Combine(ModInstance.Path, "config.json");

        if (File.Exists(path))
        {
            Config = JsonConvert.DeserializeObject<EfficientQuestingConfig>(File.ReadAllText(path));
        }

        File.WriteAllText(path, JsonConvert.SerializeObject(Config, Formatting.Indented));
    }
}