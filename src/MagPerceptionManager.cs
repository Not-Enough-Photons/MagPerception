using UnityEngine;

using SLZ.Interaction;
using SLZ.Props.Weapons;

using SLZ.Marrow.Data;

using NEP.MagPerception.UI;
using SLZ.Props;

using TMPro;
using SLZ.Player;

namespace NEP.MagPerception
{
    [MelonLoader.RegisterTypeInIl2Cpp]
    public class MagPerceptionManager : MonoBehaviour
    {
        public MagPerceptionManager(System.IntPtr ptr) : base(ptr) { }

        public static MagPerceptionManager instance;

        public MagazineUI magazineUI { get; private set; }

        public UIHandType handType { get; set; }
        public UIShowType showType { get; set; }

        public MagazineData grabbedMagazineData { get; private set; }

        public Gun lastGun;
        public Magazine lastMag;
        public Hand lastHand;

        public Transform shellEjectorTransform { get; private set; }

        private Transform playerCamera;

        private bool triggerUIOnce = false;

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

            playerCamera = BoneLib.Player.playerHead;
        }

        public void OnMagazineAttached(Magazine magazine, Hand hand)
        {
            var magazineState = magazine.magazineState;
            var magazineData = magazineState.magazineData;
            var cartridgeData = magazineState.cartridgeData;

            magazineUI.isBeingInteracted = true;
            magazineUI.UpdateParent(hand.transform);
            magazineUI.UpdateMagazineText(magazineData.platform, magazineState.AmmoCount, magazineData.rounds, magazine._lastAmmoInventory.GetCartridgeCount(cartridgeData));
            magazineUI.gameObject.SetActive(true);
        }

        public void OnMagazineUpdated(MagazineState magazineState)
        {
            var magazineData = magazineState.magazineData;
            var cartridgeData = magazineState.cartridgeData;

            magazineUI.UpdateMagazineText(magazineData.platform, magazineState.AmmoCount, magazineState.magazineData.rounds, AmmoInventory.Instance.GetCartridgeCount(cartridgeData));
        }

        public void OnMagazineEjected(Magazine magazine)
        {
            magazineUI.isBeingInteracted = false;
            magazineUI.gameObject.SetActive(false);
        }
    }
}