using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.SceneManagement;

public class PopUpManager : MonoBehaviour
{
   public static PopUpManager Instance { get; private set; }
        
        private ObjectPool<DamageNumbers> _damageLabelPopupPool;
        
        [Header("Damage Label Popup")]
        [SerializeField] private DamageNumbers damageLabelPrefab;

        [Header("Display Setup")] 
        [Range(0.8f, 1.5f), SerializeField] public float displayLength = 1f;
        private Camera _mainCamera;
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
                Destroy(gameObject);
            
            _damageLabelPopupPool = new ObjectPool<DamageNumbers>(
                () =>
                {
                    DamageNumbers damageLabel = Instantiate(damageLabelPrefab, transform);
                    damageLabel.Initialize(displayLength, this);
                    return damageLabel;
                },
                damageLabel => damageLabel.gameObject.SetActive(true),
                damageLabel => damageLabel.gameObject.SetActive(false)
            );
            
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnDestroy()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            _mainCamera = Camera.main;
        }
        
        public void DamageDone(int damage, Vector3 position, bool isCrit)
        {
            Vector3 screenPosition = position;
            screenPosition.z = 0;
            bool direction = screenPosition.x < Screen.width * 0.5f;
            
            SpawnDamagePopup(damage, screenPosition, direction, isCrit);
        }
        
        private void SpawnDamagePopup(int damage, Vector3 position, bool direction, bool isCrit)
        {
            DamageNumbers damageLabel = _damageLabelPopupPool.Get();
            damageLabel.Display(damage, position, direction, isCrit);
        }
        
        public void ReturnDamageLabelToPool(DamageNumbers damageLabel3d)
        {
            _damageLabelPopupPool.Release(damageLabel3d);
        }
}
