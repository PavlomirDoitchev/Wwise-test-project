using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public bool InputEnabled { get; set; } = true;

    [Header("Key Bindings")]
    [SerializeField] private List<KeyBinding> keyBindingsList = new List<KeyBinding>();

    private Dictionary<string, KeyCode[]> keyBindingsDict;
    public Vector2 MoveInput { get; private set; }

    private void Awake()
    {
        keyBindingsDict = new Dictionary<string, KeyCode[]>();
        foreach (var binding in keyBindingsList)
            keyBindingsDict[binding.ActionName] = binding.Keys;

        LoadKeyBindings();
    }

    public Vector2 MovementInput()
    {
        if (!InputEnabled) return Vector2.zero;

        float horizontal = 0;
        float vertical = 0;

        if (Input.GetKey(keyBindingsDict["MoveLeft"][0])) horizontal -= 1;
        if (Input.GetKey(keyBindingsDict["MoveRight"][0])) horizontal += 1;

        MoveInput = new Vector2(horizontal, vertical).normalized;
        return MoveInput;
    }

    public bool JumpInput() => InputEnabled && Input.GetKeyDown(keyBindingsDict["Jump"][0]);
    public bool JumpHeld() => InputEnabled && Input.GetKey(keyBindingsDict["Jump"][0]);
    public bool AttackInput() => InputEnabled && Input.GetKeyDown(keyBindingsDict["Attack"][0]);
    public bool SprintInput() => InputEnabled && Input.GetKey(keyBindingsDict["Sprint"][0]);
    public bool DropAttackInput() => InputEnabled && GetComboDown("DropAttack");
    public bool DashInput() => InputEnabled && Input.GetKeyDown(keyBindingsDict["Dash"][0]);
    public bool SlideInput() => InputEnabled && GetComboDown("Slide");
    public bool DropPlatform() => InputEnabled && GetComboDown("DropPlatform");
    public bool CrouchInput() => InputEnabled && Input.GetKey(keyBindingsDict["Crouch"][0]);

    private bool GetComboDown(string action)
    {
        if (!InputEnabled) return false;
        if (!keyBindingsDict.ContainsKey(action)) return false;

        KeyCode[] keys = keyBindingsDict[action];
        if (keys.Length == 0) return false;

        for (int i = 0; i < keys.Length - 1; i++)
            if (!Input.GetKey(keys[i])) return false;

        return Input.GetKeyDown(keys[keys.Length - 1]);
    }

    public void RebindKey(string action, KeyCode[] newKeys)
    {
        if (!keyBindingsDict.ContainsKey(action)) return;

        keyBindingsDict[action] = newKeys;

        for (int i = 0; i < newKeys.Length; i++)
            PlayerPrefs.SetInt(action + i, (int)newKeys[i]);
        PlayerPrefs.SetInt(action + "Length", newKeys.Length);
        PlayerPrefs.Save();
    }

    private void LoadKeyBindings()
    {
        foreach (var binding in keyBindingsList)
        {
            int length = PlayerPrefs.GetInt(binding.ActionName + "Length", binding.Keys.Length);
            KeyCode[] keys = new KeyCode[length];

            for (int i = 0; i < length; i++)
                keys[i] = (KeyCode)PlayerPrefs.GetInt(binding.ActionName + i, (int)binding.Keys[i]);

            binding.Keys = keys;
            keyBindingsDict[binding.ActionName] = keys;
        }
    }
}
