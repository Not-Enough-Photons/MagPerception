using UnityEngine;

using SLZ.Interaction;
using SLZ.Props.Weapons;

using SLZ.Marrow.Data;

using NEP.MagPerception.UI;

using TMPro;
using SLZ.Props;
using MelonLoader;

namespace NEP.MagPerception
{
    [MelonLoader.RegisterTypeInIl2Cpp]
    public class MagPerceptionManager : MonoBehaviour
    {
        public MagPerceptionManager(System.IntPtr ptr) : base(ptr) { }

        public static MagPerceptionManager instance;

        public MagazineUI magazineUI { get; private set; }

        public MagazineData grabbedMagazineData { get; private set; }

        public Hand playerLeftHand => BoneLib.Player.leftHand;
        public Hand playerRightHand => BoneLib.Player.rightHand;

        public Transform lastGrabbedTransform;
        public Gun lastGun;
        public Magazine lastMag;
        public Hand lastHand;

        private void Awake()
        {
            instance = this;
        }

        private void Start()
        {
            GameObject magUI = GameObject.Instantiate(Main.resources.LoadAsset("MagazineLayer").Cast<GameObject>(), transform);

            magUI.transform.SetParent(transform);
            magazineUI = magUI.AddComponent<MagazineUI>();

            magazineUI.ammoCounterText = magUI.transform.Find("AmmoCounter").GetComponent<TextMeshProUGUI>();
            magazineUI.ammoInventoryText = magUI.transform.Find("AmmoInventory").GetComponent<TextMeshProUGUI>();
            magazineUI.ammoTypeText = magUI.transform.Find("AmmoType").GetComponent<TextMeshProUGUI>();
            magazineUI.animator = magUI.GetComponent<Animator>();

            magUI.SetActive(false);
        }

        /// <summary>
        /// Called when a player grabs a magazine.
        /// </summary>
        public void OnMagazineAttached(Magazine magazine)
        {
            lastMag = magazine;
            magazineUI.Show();
            magazineUI.OnMagEvent();
            magazineUI.UpdateParent(lastMag.insertPointTransform);
            magazineUI.DisplayMagInfo(magazine.magazineState);
        }

        /// <summary>
        /// Called when the player inserts a magazine into their gun.
        /// </summary>
        public void OnMagazineInserted(MagazineState magazineState, Gun gun)
        {
            lastGun = gun;
            magazineUI.Show();
            magazineUI.OnMagEvent();
            magazineUI.UpdateParent(lastGun.firePointTransform);
            magazineUI.DisplayGunInfo(lastGun);
        }

        /// <summary>
        /// Called when the player ejects the magazine from their gun.
        /// </summary>
        public void OnMagazineEjected(MagazineState magazineState, Gun gun)
        {
            magazineUI.Show();
            magazineUI.OnMagEvent();
            magazineUI.UpdateParent(lastGun.firePointTransform);
            magazineUI.DisplayGunInfo(lastGun);
        }

        /// <summary>
        /// Called when a player grabs a gun.
        /// </summary>
        public void OnGunAttached(Gun gun)
        {
            if (!Settings.ShowWithGun)
            {
                return;
            }

            lastGun = gun;
            magazineUI.OnMagEvent();
            magazineUI.UpdateParent(gun.firePointTransform);
            magazineUI.Show();
        }

        /// <summary>
        /// Called when a player lets go of a gun.
        /// </summary>
        public void OnGunDetached(Gun gun)
        {
            if (!Settings.ShowWithGun)
            {
                return;
            }

            lastGun = null;
            magazineUI.OnMagEvent();

            if(lastMag != null)
            {
                magazineUI.UpdateParent(gun.firePointTransform);
                magazineUI.DisplayMagInfo(lastMag.magazineState);
            }
        }

        /// <summary>
        /// Called when a round (spent or unspent) is ejected from the chamber.
        /// </summary>
        public void OnGunEjectRound()
        {
            if (!Settings.ShowWithGun)
            {
                return;
            }

            magazineUI.OnMagEvent();
            magazineUI.DisplayGunInfo(lastGun);
        }
    }
}