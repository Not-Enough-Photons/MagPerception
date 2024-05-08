using UnityEngine;
using System.IO;
using System.Reflection;
using System.Linq;

namespace NEP.MagPerception
{
    public static class DataManager
    {
        internal static AssetBundle GetEmbeddedBundle()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();

            string fileName = BoneLib.HelperMethods.IsAndroid() ? "mp_resources_quest.pack" : "mp_resources_pcvr.pack";

            using (Stream resourceStream = assembly.GetManifestResourceStream("NEP.MagPerception.Resources." + fileName))
            {
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    resourceStream.CopyTo(memoryStream);
                    return AssetBundle.LoadFromMemory(memoryStream.ToArray());
                }
            }
        }

        internal static byte[] Internal_LoadFromAssembly(Assembly assembly, string name)
        {
            string[] manifestResources = assembly.GetManifestResourceNames();

            if (manifestResources.Contains(name))
            {
                using (Stream str = assembly.GetManifestResourceStream(name))
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    str.CopyTo(memoryStream);
                    return memoryStream.ToArray();
                }
            }

            return null;
        }
    }
}