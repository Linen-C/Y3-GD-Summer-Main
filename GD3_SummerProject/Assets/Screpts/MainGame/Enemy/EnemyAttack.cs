using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [Header("マニュアル")]
    [SerializeField] GameObject _cursor;
    [SerializeField] GameObject _bullet;
    [SerializeField] GameObject flashObj;   // フラッシュ用
    [Header("オート")]
    [SerializeField] Animator _flashAnim;
    [SerializeField] EnemyCTRL _enemyCTRL;

    bool _coolDownReset = false;  // クールダウンのリセットフラグ


    private void Start()
    {
        _enemyCTRL = GetComponent<EnemyCTRL>();
        _flashAnim = flashObj.GetComponent<Animator>();
    }

    public void SetBulletNull()
    {
        _bullet = null;
    }

    public void Attack(int needCharge, bool shooting, GC_BpmCTRL bpm, Renderer renderer, EnemyWepon weapon, Animator anim)
    {
        // ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ //
        // 攻撃・クールダウン
        // ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ //

        if ((_enemyCTRL._nowWeaponCharge == needCharge) && bpm.Signal() && _coolDownReset == false)
        {
            if (shooting)
            {
                if (renderer.isVisible)
                {
                    Instantiate(
                    _bullet,
                    new Vector3
                    (_cursor.transform.position.x,
                    _cursor.transform.position.y,
                    _cursor.transform.position.z),
                    _cursor.transform.rotation);
                }
            }
            else { weapon.Attacking(); }

            anim.SetTrigger("Attack");
            _coolDownReset = true;
        }

        if (bpm.Metronome())
        {
            if (_coolDownReset == true)
            {
                //state = State.Alive;
                _enemyCTRL._nowWeaponCharge = 1;
                _coolDownReset = false;
            }
            else if (_enemyCTRL._nowWeaponCharge < needCharge) { _enemyCTRL._nowWeaponCharge++; }

            if (_enemyCTRL._nowWeaponCharge == (needCharge - 1))
            {
                anim.SetTrigger("Charge");
                _flashAnim.SetTrigger("FlashTrigger");
            }
        }

        // ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ //
    }
}
