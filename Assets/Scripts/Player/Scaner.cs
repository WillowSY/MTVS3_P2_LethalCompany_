using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scanner : MonoBehaviour
{
    public float scanRange = 100f; // 스캔 범위
    public string[] detectableTags = { "Item", "Enemy" }; // 감지 가능한 태그 목록
    public string weaponTag = "Weapon"; // 무기 태그
    private bool _isScanning;

    public GameObject ScanFX;
    private UIManager _uiManager;
    private SoundEmitter _soundEmitter;

    void Start()
    {
        _uiManager = Object.FindFirstObjectByType<UIManager>();
        _soundEmitter = Object.FindFirstObjectByType<SoundEmitter>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1) && !_isScanning)
        {
            _soundEmitter.PlayScanSound();
            StartCoroutine(Scanning());
        }
    }

    // 주변 스캔
    private void ScanSurroundings()
    {
        RaycastHit[] hits = Physics.RaycastAll(Camera.main.transform.position,
            Camera.main.transform.forward, scanRange);
        
        GameObject closestDetectableObject = null;
        float closestDistance = Mathf.Infinity;

        foreach (RaycastHit hit in hits)
        {
            if (hit.collider != null && IsDetectableTag(hit.collider.tag) && hit.collider.tag != weaponTag)
            {
                float distance = Vector3.Distance(Camera.main.transform.position, hit.collider.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestDetectableObject = hit.collider.gameObject;
                }
            }
        }

        if (closestDetectableObject != null)
        {
            ItemDataInfo(closestDetectableObject);
        }
        else
        {
            Debug.Log("No detectable objects found");
        }
    }

    private IEnumerator Scanning()
    {
        _isScanning = true;
        ScanSurroundings();
        for (int i = 0; i < 100; i++)
        {
            ScanFX.transform.localScale += new Vector3(0.4f,0.4f,0.4f);
            yield return new WaitForSeconds(0.01f);
        }
        ScanFX.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
        _isScanning = false;
    }

    // 감지 가능한 태그인지 확인
    private bool IsDetectableTag(string tag)
    {
        foreach (string detectableTag in detectableTags)
        {
            if (tag == detectableTag)
            {
                return true;
            }
        }
        return false;
    }
    
    // 정보 표시
    private void ItemDataInfo(GameObject scannedObject)
    {
        Scrap data = scannedObject.GetComponent<Scrap>();

        MonsterData monster = scannedObject.GetComponent<MonsterData>();
        string info = "";
        if (scannedObject.CompareTag("Item"))
        {
            info = "아이템 발견 :<br>" + data.scrap.name;
        }
        else if (scannedObject.CompareTag("Enemy"))
        {
            info = "적 발견 :<br>" + monster.monsterName;
        }
        StartCoroutine(_uiManager.ScanDisplayInfo(info));
    }
}
