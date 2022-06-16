using UnityEngine;
using UnityEngine.UI;

public class ArenaCTRL : MonoBehaviour
{

    [Header("最大・現在ウェーブ数")]
    [SerializeField] int max_Wave;
    [SerializeField] public int now_Wave;
    [Header("BPMコントロール(自動取得)")]
    [SerializeField] GC_BpmCTRL bpmCTRL;   // メトロノーム受け取り用
    [Header("敵管理")]
    [SerializeField] ArenaEnemyCTRL enemyCtrl;
    [Header("インターバル用")]
    [SerializeField] int interval_MaxCount_Nomal;
    [SerializeField] int interval_MaxCount_Clear;
    [SerializeField] int interval_NowCount;
    [SerializeField] bool doInterval_Nomal;
    [SerializeField] bool doInterval_Clear;
    [SerializeField] Text interval_text;
    [Header("表示テキスト")]
    [SerializeField] string text_nomal1;
    [SerializeField] string text_nomal2;
    [SerializeField] string text_nomal3;
    [SerializeField] string text_nomal4;
    [SerializeField] string text_clear1;
    [SerializeField] string text_clear2;

    bool inWave = false;


    void Start()
    {
        var gameCtrlObj = GameObject.FindGameObjectWithTag("GameController");
        bpmCTRL = gameCtrlObj.GetComponent<GC_BpmCTRL>();

        enabled = false;
    }

    void Update()
    {
        if (inWave)
        {
            if (enemyCtrl.DoEnemyAllDestroy())
            {
                inWave = false;
                Debug.Log("ウェーブ進行");
                ProgressCheck();
            }
        }
        if (doInterval_Nomal)
        {
            Interval_Nomal();
        }
        if (doInterval_Clear)
        {
            Interval_Clear();
        }
    }


    public void Entry()
    {
        enabled = true;
        ProgressCheck();
    }

    void Interval_Nomal()
    {
        if (bpmCTRL.Metronome())
        {
            interval_NowCount++;
            switch (interval_NowCount)
            {
                case 1:
                    interval_text.text = text_nomal1;
                break;
                case 2:
                    interval_text.text = text_nomal2;
                    break;
                case 3:
                    interval_text.text = text_nomal3;
                    break;
                case 4:
                    interval_text.text = text_nomal4 + now_Wave.ToString();
                    break;
                default:
                    break;
            }
        }

        if (interval_NowCount > interval_MaxCount_Nomal)
        {
            interval_text.text = " ";
            enemyCtrl.WavaStart();

            doInterval_Nomal = false;
            inWave = true;
        }
    }

    void Interval_Clear()
    {
        if (bpmCTRL.Metronome())
        {
            interval_NowCount++;
            switch (interval_NowCount)
            {
                case 1:
                    interval_text.text = text_clear1;
                    break;
                case 2:
                    interval_text.text = text_clear2;
                    break;
            }
        }

        if (interval_NowCount > interval_MaxCount_Clear)
        {
            interval_text.text = " ";
            ArenaClear();
        }
    }

    void ProgressCheck()
    {
        interval_NowCount = 0;

        if (now_Wave == max_Wave)
        {
            doInterval_Clear = true;
        }
        else 
        {
            now_Wave++;
            doInterval_Nomal = true;
        }

    }

    void ArenaClear()
    {
        Debug.Log("殲滅");
        enabled = false;
    }
}
