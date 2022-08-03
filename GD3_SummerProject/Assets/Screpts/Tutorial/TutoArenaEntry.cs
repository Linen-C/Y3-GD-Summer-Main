using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutoArenaEntry : MonoBehaviour
{
    [Header("�A���[�i(�����擾)")]
    [SerializeField] TutoArenaCTRL arenaCtrl;

    void Start()
    {
        arenaCtrl = transform.parent.GetComponent<TutoArenaCTRL>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag != "Player") { return; }

        // �N��
        arenaCtrl.Entry();

        // �p�ς݂Ȃ̂ŏ���
        Destroy(gameObject);
    }

}
