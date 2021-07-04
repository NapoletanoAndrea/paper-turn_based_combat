using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageTextBehaviour : MonoBehaviour
{
    [SerializeField] float moveSeconds;

    TextMesh textMesh;

    private void Awake()
    {
        textMesh = GetComponent<TextMesh>();
    }

    public void Init(int damage, Vector2 endPoint)
    {
        if (damage > 0)
        {
            textMesh.color = Color.red;
            textMesh.text = "-" + damage;
        }
        else if (damage < 0)
        {
            textMesh.color = Color.green;
            textMesh.text = "+" + damage;
        }
        else
        {
            textMesh.color = Color.white;
            textMesh.text = damage.ToString();
        }

        StartCoroutine(MoveToCorutine(transform, endPoint, moveSeconds));
    }

    private IEnumerator MoveToCorutine(Transform moveObject, Vector2 targetPosition, float seconds)
    {
        Vector2 startPosition = moveObject.position;

        Color startColor = textMesh.color;
        Color color = startColor;

        float t = 0;
        float count = 0;

        while (count <= seconds)
        {
            count += Time.deltaTime;
            t += Time.deltaTime / seconds;

            moveObject.position = Vector2.Lerp(startPosition, targetPosition, t);

            if(count > seconds / 1.2)
            {
                color.a = Mathf.Lerp(startColor.a, 0, t);
                textMesh.color = color;
            }

            yield return null;
        }

        Destroy(gameObject);
    }
}
