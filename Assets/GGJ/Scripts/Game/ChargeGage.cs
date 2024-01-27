using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class ChargeGage : MonoBehaviour
{
    [SerializeField] private AnimationCurve widthAnimationCurve;
    [SerializeField] private AnimationCurve heightAnimationCurve;

    private PlayerInputManager playerInputManager;
    PlayerCharge playerCharge;

    private RectTransform view;

    // Start is called before the first frame update
    void Start()
    {
        playerCharge = GetComponentInParent<PlayerCharge>();
        view = GetComponentInChildren<Image>().gameObject.GetComponent<RectTransform>();
        view.gameObject.SetActive(false);
        playerInputManager = GetComponentInParent<PlayerInputManager>();
        playerInputManager.OnPressedFireButton
            .Subscribe(_ => view.gameObject.SetActive(true))
            .AddTo(this);

        playerInputManager.OnCanceledFireButton
            .Subscribe(_ => view.gameObject.SetActive(false))
            .AddTo(this);
    }

    // Update is called once per frame
    void Update()
    {
        if (!view.gameObject.activeSelf) return;
        var width = widthAnimationCurve.Evaluate(playerCharge.normalizedChargedTime);
        var height = heightAnimationCurve.Evaluate(playerCharge.normalizedChargedTime);
        view.sizeDelta = new Vector2(width, height);
        //v.value = playerCharge.normalizedChargedTime;
    }
}
