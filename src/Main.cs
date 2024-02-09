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
            mainCategory.CreateEnumElement("Show On Hand", Color.white, UIHandType.Left, (handType) => manager.handType = (UIHandType)handType);
            mainCategory.CreateEnumElement("Show Type", Color.white, UIShowType.Always, (showType) => manager.showType = (UIShowType)showType);
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
