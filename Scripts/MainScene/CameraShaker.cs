using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HamuGame;

namespace HamuGame
{
    //カメラにアタッチする
    public class CameraShaker : MonoBehaviour
    {
        //カメラを揺らす処理
        //揺らす時間・揺らす強さを渡す
        public void ShakeCamera(float duration, float magnitude)
        {
            StartCoroutine(ShakeCoroutine(duration, magnitude));
        }

        private IEnumerator ShakeCoroutine(float duration, float magnitude)
        {
            var pos = transform.localPosition;

            var elapsed = 0f;

            while (duration > elapsed)
            {
                var x = pos.x + Random.Range(-1f, 1f) * magnitude;
                var y = pos.y + Random.Range(-1f, 1f) * magnitude;

                transform.localPosition = new Vector3(x, y, pos.z);

                elapsed += Time.deltaTime;

                yield return null;
            }

            transform.localPosition = pos;
        }
    }
}
