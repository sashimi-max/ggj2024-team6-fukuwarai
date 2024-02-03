using UniRx;
using UnityEngine.InputSystem;

/// <summary>
/// InputSystem ReactiveProperty Extension
/// </summary>
public static class InputSystemExtension
{
    public static ReadOnlyReactiveProperty<bool> GetButtonProperty(this InputAction inputAction)
    {
        return Observable.FromEvent<InputAction.CallbackContext>(
                h => inputAction.performed += h,
                h => inputAction.performed -= h)
            .Select(x => x.ReadValueAsButton())
            .ToReadOnlyReactiveProperty(false);
    }

    //Axis入力だと0等が反応しないためGetDeltaAxisPropertyのみの使用でもよさそうです
    public static ReadOnlyReactiveProperty<float> GetAxisProperty(this InputAction inputAction)
    {
        return Observable.FromEvent<InputAction.CallbackContext>(
                h => inputAction.performed += h,
                h => inputAction.performed -= h)
            .Select(x => x.ReadValue<float>())
            .ToReadOnlyReactiveProperty(0);
    }

    //Delta入力はUpdate基準なのでUpdate基準に変換(主にマウスで使用)
    public static ReadOnlyReactiveProperty<float> GetDeltaAxisProperty(this InputAction inputAction)
    {
        return Observable.EveryUpdate().Select(_ => inputAction.ReadValue<float>()).ToReadOnlyReactiveProperty(0);
    }
}