using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DotAnimation : MonoBehaviour
{
    private const float BOB_SPEED = 2f;
    private const float BOB_DISTANCE = 0.3f;
    private const float ROTATION_SPEED = 1f;
    private const float ROTATION_MAX = 10f;
    public int Value = 1;
    private float _rot = 0;
    private GameObject _dot;
    private bool _isIdle = true;
    void Start()
    {
        _dot = transform.GetChild(0).gameObject;
    }

    void FixedUpdate()
    {
        if (_isIdle)
        {
            _dot.transform.localPosition = new Vector3(0, Mathf.Sin(Time.time * BOB_SPEED) * BOB_DISTANCE, 0);
            _rot = (_rot + ROTATION_SPEED) % 360;
            _dot.transform.localRotation = Quaternion.Euler(0, _rot, 90);
        }
    }

    public void PlaySpawnAnimation()
    {
        StartCoroutine(SpawnAnimation(transform.GetChild(0).gameObject));
    }

    public void PlayCollectedAnimation()
    {
        StartCoroutine(CollectedAnimation());
    }

    private IEnumerator SpawnAnimation(GameObject _dot)
    {
        float oldTS = Time.timeScale;
        if (oldTS != 0)
        {
            _isIdle = false;
            Camera.main.GetComponent<CinemachineBrain>().enabled = false;
            Camera.main.transform.position = _dot.transform.position + -Camera.main.transform.forward * 20;
            Time.timeScale = 0.0f;
        }
        _dot.GetComponentInChildren<Animator>().enabled = true;
        yield return new WaitForSecondsRealtime(3f);
        _dot.GetComponentInChildren<Animator>().enabled = false;


        if (oldTS != 0)
        {
            Time.timeScale = oldTS;
            Camera.main.GetComponent<CinemachineBrain>().enabled = true;

        }
        _isIdle = true;
    }

    private IEnumerator CollectedAnimation()
    {
        transform.GetChild(0).gameObject.SetActive(false);
        transform.GetChild(1).gameObject.SetActive(true);
        transform.GetChild(2).gameObject.SetActive(false);
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }
    public static GameObject SpawnDot(Vector3 position)
    {
        GameObject dot = Instantiate(Resources.Load("DotObj"), position, Quaternion.identity) as GameObject;
        dot.GetComponent<DotAnimation>().PlaySpawnAnimation();
        return dot;
    }
}
