using UnityEngine;
using UnityEngine.UI;

namespace Michsky.UI.Heat
{
    [ExecuteInEditMode]
    [DisallowMultipleComponent]
    [AddComponentMenu("Heat UI/UI Manager/UI Manager Logo")]
    public class UIManagerLogo : MonoBehaviour
    {
        public enum LogoType
        {
            GameLogo,
            BrandLogo
        }

        // Resources
        public UIManager UIManagerAsset;

        // Settings
        [SerializeField] private LogoType logoType = LogoType.GameLogo;
        private Image objImage;

        private void Awake()
        {
            enabled = true;

            if (UIManagerAsset == null) UIManagerAsset = Resources.Load<UIManager>("Heat UI Manager");
            if (objImage == null) objImage = GetComponent<Image>();
            if (!UIManagerAsset.enableDynamicUpdate)
            {
                UpdateImage();
                enabled = false;
            }
        }

        private void Update()
        {
            if (UIManagerAsset == null) return;
            if (UIManagerAsset.enableDynamicUpdate) UpdateImage();
        }


        private void UpdateImage()
        {
            if (objImage == null)
                return;

            if (logoType == LogoType.GameLogo)
                objImage.sprite = UIManagerAsset.gameLogo;
            else if (logoType == LogoType.BrandLogo) objImage.sprite = UIManagerAsset.brandLogo;
        }
    }
}