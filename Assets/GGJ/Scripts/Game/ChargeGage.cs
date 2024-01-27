using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class ChargeGage : MonoBehaviour
{
    private PlayerInputManager playerInputManager;
    PlayerCharge playerCharge;
    Slider slider;
    // Start is called before the first frame update
    void Start()
    {
        playerCharge = GetComponentInParent<PlayerCharge>();
        slider = GetComponent<Slider>();
        slider.enabled = false;
        playerInputManager = GetComponentInParent<PlayerInputManager>();
        playerInputManager.OnPressedFireButton
            .Subscribe(_ => slider.enabled = true)
            .AddTo(this);

        playerInputManager.OnPressedFireButton
            .Subscribe(_ => slider.enabled = false)
            .AddTo(this);
    }

    // Update is called once per frame
    void Update()
    {
        slider.value = playerCharge.normalizedChargedTime;
    }
}
