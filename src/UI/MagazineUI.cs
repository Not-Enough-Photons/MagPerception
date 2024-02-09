using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using SLZ.Marrow.Data;
using TMPro;

namespace NEP.MagPerception.UI
{
    [MelonLoader.RegisterTypeInIl2Cpp]
    public class MagazineUI : MonoBehaviour
    {
        public MagazineUI(System.IntPtr ptr) : base(ptr) { }

        public TextMeshProUGUI ammoCounterText;
        public TextMeshProUGUI ammoInventoryText;
        public TextMeshProUGUI ammoTypeText;

        public Animator animator;

        public bool isBeingInteracted { get; set; } = false;

        private Vector3 lastHandPos
        {
            get => MagPerceptionManager.instance.lastHand.transform.position;
        }

        private Vector3 shellEjectorPos
        {
            get => MagPerceptionManager.instance.shellEjectorTransform.position;
        }

        private Coroutine enableRoutine;

        private Dictionary<Weight, Color> weightColorTable = new Dictionary<Weight, Color>()
        {
            { Weight.LIGHT, Color.yellow },
            { Weight.MEDIUM, new Color(1f, 0.6372304f, 0f) },
            { Weight.HEAVY,  new Color(255, 47, 28)}
        };

        private void Awake()
        {
            animator = GetComponent<Animator>();
        }

        private void OnEnable()
        {
            if(enableRoutine == null)
            {
                enableRoutine = MelonLoader.MelonCoroutines.Start(CoEnable()) as Coroutine;
            }
        }

        private IEnumerator CoEnable()
        {
            animator?.Play("mag_enter_01");
            yield return new WaitForSeconds(0.3f);
            
            while (isBeingInteracted)
            {
                animator?.Play("mag_idle_01");
                yield return null;
            }

            animator?.Play("mag_exit_01");
            yield return new WaitForSeconds(0.3f);
            UpdateParent(MagPerceptionManager.instance.transform);
            gameObject.SetActive(false);

            enableRoutine = null;
        }

        private void Update()
        {
            transform.LookAt(BoneLib.Player.playerHead);
        }

        public void UpdateMagazineText(string ammoType, int ammoCount, int maxAmmo, int ammoInventory)
        {
            ammoCounterText.text = $"{ammoCount}/{maxAmmo}";
            ammoInventoryText.text = "RESERVE: " + ammoInventory.ToString();
            ammoTypeText.text = ammoType;
        }

        public void UpdateParent(Transform attachment)
        {
            transform.parent = attachment;
            transform.localPosition = Vector3.zero;
        }
    }
}