using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
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
    //private void Update()
    //{


    //}

    public Vector2 MovementInput()
    {
        float horizontal = 0;
        float vertical = 0;

        if (Input.GetKey(keyBindingsDict["MoveLeft"][0])) horizontal -= 1;
        if (Input.GetKey(keyBindingsDict["MoveRight"][0])) horizontal += 1;

        MoveInput = new Vector2(horizontal, vertical).normalized;
        return MoveInput;
    }

    public bool JumpInput() => Input.GetKey(keyBindingsDict["Jump"][0]);
    public bool AttackInput() => Input.GetKeyDown(keyBindingsDict["Attack"][0]);
    public bool SprintInput() => Input.GetKey(keyBindingsDict["Sprint"][0]);
    public bool DropDownAttackInput() => GetComboDown("DropAttack");
    //public bool PlayerDodgeInput() => Input.GetKeyDown(keyBindingsDict["Dodge"][0]);
    //public bool PlayerMountInput() => Input.GetKeyDown(keyBindingsDict["Mount"][0]);
    //public bool PlayerDismountInput() => Input.GetKeyDown(keyBindingsDict["Dismount"][0]);

    //public bool AbilityOneInput() => Input.GetKeyDown(keyBindingsDict["AbilityOne"][0]);
    //public bool AbilityTwoInput() => Input.GetKeyDown(keyBindingsDict["AbilityTwo"][0]);
    //public bool AbilityThreeInput() => Input.GetKeyDown(keyBindingsDict["AbilityThree"][0]);
    //public bool AbilityFourInput() => Input.GetKeyDown(keyBindingsDict["AbilityFour"][0]);
    //public bool AbilityFiveInput() => Input.GetKeyDown(keyBindingsDict["AbilityFive"][0]);

    private bool GetComboDown(string action)
    {
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