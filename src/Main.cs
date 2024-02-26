using MelonLoader;

using BoneLib.BoneMenu;
using NEP.MagPerception.UI;

using UnityEngine;

using System.Linq;
using BoneLib;

namespace NEP.MagPerception
{
    public static class BuildInfo
    {
        public const string Name = "MagPerception"; // Name of the Mod.  (MUST BE SET)
        public const string Author = null; // Author of the Mod.  (Set as null if none)
        public const string Company = null; // Company that made the Mod.  (Set as null if none)
        public const string Version = "1.0.0"; // Version of the Mod.  (MUST BE SET)
        public const string DownloadLink = null; // Download Link for the Mod.  (Set as null if none)
    }

    public class Main : MelonMod
    {
        private static string dataPath = MelonUtils.UserDataDirectory + "/Not Enough Photons/MagPerception/mp_resources.pack";

        private MagPerceptionManager manager;

        public static AssetBundle resources { get; private set; }

        public override void OnInitializeMelon()
        {
            resources = AssetBundle.LoadFromFile(dataPath);

            if (resources == null)
            {
                throw new System.Exception(
                    "Resources file is missing/invalid! Please make sure you installed the file in UserData/MagPerception !");
            }

            SetupBonemenu();

            BoneLib.Hooking.OnLevelInitialized += OnSceneWasLoaded;
        }

        private void SetupBonemenu()
        {
            var nepCategory = MenuManager.CreateCategory("Not Enough Photons", Color.white);
            var mainCategory = nepCategory.CreateCategory("MagPerception", Color.white);
            var offsetCategory = mainCategory.CreateCategory("Offset", Color.white);

            mainCategory.CreateFloatElement("Scale", Color.white, 0.75f, 0.25f, 0.25f, 1.5f, (value) => Settings.InfoScale = value);
            mainCategory.CreateEnumElement("Show Type", Color.white, UIShowType.FadeShow, (showType) => Settings.ShowType = (UIShowType)showType);
            mainCategory.CreateFloatElement("Time Until Hidden", Color.white, 3f, 0.5f, 0f, 10f, (value) => Settings.TimeUntilHidden = value);
            mainCategory.CreateBoolElement("Show With Gun", Color.white, false, (value) => Settings.ShowWithGun = value);

            offsetCategory.CreateFloatElement("X", Color.red, 0.075f, 0.025f, -1f, 1f, (value) => Settings.Offset.x = value);
            offsetCategory.CreateFloatElement("Y", Color.green, 0f, 0.025f, -1f, 1f, (value) => Settings.Offset.y = value);
            offsetCategory.CreateFloatElement("Z", Color.blue, 0f, 0.025f, -1f, 1f, (value) => Settings.Offset.z = value);
        }

        public void OnSceneWasLoaded(LevelInfo info)
        {
            manager = new GameObject("Mag Perception Manager").AddComponent<MagPerceptionManager>();
        }

        public static Object GetObjectFromResources(string name)
        {
            Object[] objects = resources.LoadAllAssets();

            return objects.FirstOrDefault((asset) => asset.name == name);
        }
    }
}
