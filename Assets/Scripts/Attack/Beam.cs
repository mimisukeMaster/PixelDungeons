using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using System.Collections;

public class Beam : AttackController
{
    public LineRenderer lineRenderer;
    private float chargeTime;
    private float emissionTime;
    private float beamLength;

    /// <summary>
    /// ビームの初期設定
    /// </summary>
    public void BeamInit(string TargetTag, int Damage, float destroyTime, float chargeTime, float emissionTime, float beamLength)
    {
        base.Init(TargetTag, Damage, destroyTime);
        this.chargeTime = chargeTime;
        this.beamLength = beamLength;
        this.emissionTime = emissionTime;

        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.enabled = false;

        StartCoroutine(BeamAttack());
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
        Vector3 beamTipPos = transform.parent.position + transform.parent.forward * -beamLength;
        lineRenderer.SetPosition(0, beamRootPos);
        lineRenderer.SetPosition(1, beamTipPos);

        yield return new WaitForSeconds(chargeTime);

        // 実際に攻撃 ビームのパラメータはBeamInitで設定済み
        transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, beamLength * 2.0f);

        yield return new WaitForSeconds(emissionTime);

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
