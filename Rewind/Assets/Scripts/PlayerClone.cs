using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static DG.Tweening.DOTweenCYInstruction;

[RequireComponent(typeof(Collider2D))]
public class PlayerClone : MonoBehaviour
{
    private LineRenderer lr;

    [SerializeField]
    private SpriteRenderer sr;
    
    [SerializeField] private AudioClip latchClip;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player"
            && collision.gameObject.transform.parent == null)
        {
            Player p = collision.GetComponent<Player>();
            Rigidbody2D prb = collision.GetComponent<Rigidbody2D>();

            if (!p.isAvoidingClones)
            {
                prb.linearVelocity = Vector2.zero;
                prb.gravityScale = 0;
                p.AnimateLatch(transform.position);
                SoundtrackManager.instance.PlayOneShot(latchClip, 2f);
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player"
            && collision.gameObject.transform.parent == null)
        {
            collision.gameObject.transform.SetParent(this.transform);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player"
            && collision.gameObject.transform.parent == this.transform)
        {
            collision.gameObject.transform.SetParent(null);
        }
    }

    private void Awake()
    {
        this.lr = this.GetComponent<LineRenderer>();
    }

    public void Reverse(Vector3[] positions, float duration)
    {
        //cut the array in half
        positions = positions.Where((x, i) => i % 2 == 0).ToArray();

        lr.positionCount = positions.Length;
        lr.SetPositions(positions);

        Color randomColor = new Color(Random.Range(0.5f, 1), Random.Range(0.5f, 1), Random.Range(0.5f, 1), 0.4f);

        sr.color = randomColor;
        lr.material.color = randomColor;

        this.transform.position = positions[0];

        this.transform.DOPath(positions,
                              duration * 3,
                              PathType.CatmullRom,
                              PathMode.Sidescroller2D,
                              5).SetEase(Ease.Flash).OnComplete(() => Disappear());
    }

    private void Disappear()
    {
        StartCoroutine(DisappearCoroutine(1));
    }

    private IEnumerator DisappearCoroutine(float disappearDuration)
    {
        yield return new WaitForEndOfFrame();
        Sequence fadeSeq = DOTween.Sequence();
        fadeSeq.Append(sr.DOFade(0.04f, disappearDuration));
        fadeSeq.Join(lr.material.DOFade(0.04f, disappearDuration));
        fadeSeq.OnComplete(() => Destroy(this.gameObject));
        fadeSeq.Play();
    }
}
