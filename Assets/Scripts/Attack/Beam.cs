using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using System.Collections;

public class Beam : AttackController
{
    public LineRenderer lineRenderer;
    private float chargeTime;
    private float emittionTime;
    private float beamLength;

    public void Init(string TargetTag, int Damage,float destroyTime,float chargeTime,float emittionTime,float beamLength)
    {
        base.Init(TargetTag,Damage,destroyTime);
        this.chargeTime  = chargeTime;
        this.beamLength = beamLength;
        this.emittionTime = emittionTime;
        BeamInit();
        StartCoroutine(BeamAttack());
    }

    /// <summary>
    /// 予測線の初期設定
    /// </summary>
    private void BeamInit()
    {
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.enabled = false;
    }
    
    /// <summary>
    /// ビーム攻撃コルーチン
    /// </summary>
    private IEnumerator BeamAttack()
    {
        lineRenderer.enabled = true;
        
        // 予測線の準備
        SetBeamAppearance(0.5f, 0.5f, Color.cyan, Color.blue);
        Vector3 beamRootPos = transform.parent.position;
        Vector3 beamTipPos =  transform.parent.position + transform.parent.forward * -beamLength;
        lineRenderer.SetPosition(0, beamRootPos);
        lineRenderer.SetPosition(1, beamTipPos);

        yield return new WaitForSeconds(chargeTime);

        // 実際に攻撃
        transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, beamLength * 2.0f);
        GetComponentInChildren<AttackController>().Init("Player", damage, 2.0f);

        yield return new WaitForSeconds(emittionTime);

        Destroy(transform.parent.gameObject);
    }

    /// <summary>
    /// ビームの外観を設定
    /// </summary>
    private void SetBeamAppearance(float startWidth, float endWidth, Color startColor, Color endColor)
    {
        lineRenderer.startWidth = startWidth;
        lineRenderer.endWidth = endWidth;
        lineRenderer.startColor = startColor;
        lineRenderer.endColor = endColor;
    }
}
