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

        private Dictionary<Weight, Color> weightColorTable = new Dictionary<Weight, Color>()
        {
            { Weight.LIGHT, Color.yellow },
            { Weight.MEDIUM, new Color(1f, 0.6372304f, 0f) },
            { Weight.HEAVY,  new Color(255, 47, 28)}
        };

        private float timeSinceLastEvent = 0.0f;
        private float maxTimeSinceEvent = 5.0f;
        private bool fadeOut = false;

        private float fadeOutTime = 0.0f;
        private float fadeOutDuration = 1.0f;

        private Vector3 targetPosition;
        private Quaternion lastRotation;

        private void Awake()
        {
            animator = GetComponent<Animator>();
        }
        
        private void FixedUpdate()
        {
            transform.LookAt(BoneLib.Player.playerHead);

            transform.localPosition = Vector3.Lerp(transform.localPosition, targetPosition, 8f * Time.fixedDeltaTime);
            transform.rotation = Quaternion.Slerp(lastRotation, transform.rotation, 8f * Time.fixedDeltaTime);

            UIShowType showType = MagPerceptionManager.instance.showType;

            switch(showType)
            {
                case UIShowType.Always:
                    return;
                case UIShowType.FadeShow:
                    if (Vector3.Distance(BoneLib.Player.playerHead.transform.position, transform.position) > 2f)
                    {
                        FadeOut();
                        return;
                    }

                    timeSinceLastEvent += Time.deltaTime;
                    
                    if (timeSinceLastEvent > maxTimeSinceEvent)
                    {
                        timeSinceLastEvent = 0.0f;
                        FadeOut();
                    }
                    break;
                case UIShowType.Hide:
                    gameObject.SetActive(false);
                    break;
            }
        }

        public void OnMagEvent()
        {
            UIShowType showType = MagPerceptionManager.instance.showType;

            switch(showType)
            {
                case UIShowType.FadeShow:
                    FadeIn();
                    break;
            }
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
            targetPosition = Vector3.zero;
        }

        private void FadeIn()
        {
            timeSinceLastEvent = 0.0f;
            if (fadeOut)
            {
                gameObject.SetActive(true);
                animator?.Play("mag_enter_01");
                fadeOut = false;
            }
        }

        private void FadeOut()
        {
            if (!fadeOut)
            {
                animator?.SetTrigger("exit");
                fadeOut = true;
            }
            else
            {
                fadeOutTime += Time.deltaTime;

                if (fadeOutTime > fadeOutDuration)
                {
                    fadeOutTime = 0.0f;
                    gameObject.SetActive(false);
                }
            }
        }
    }
}