using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private bool jumpHeldLastFrame = false;
    private bool jumpReleasedSinceLastJump = true;
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

    //public bool JumpInput()
    //{
    //    bool isHeld = Input.GetKey(keyBindingsDict["Jump"][0]);

    //    if (!isHeld)
    //        jumpReleasedSinceLastJump = true;

    //    jumpHeldLastFrame = isHeld;

    //    return isHeld && jumpReleasedSinceLastJump;
    //}

    //public void ConsumeJump()
    //{
    //    jumpReleasedSinceLastJump = false;
    //}
    public bool JumpInput() => Input.GetKey(keyBindingsDict["Jump"][0]);
    public bool AttackInput() => Input.GetKeyDown(keyBindingsDict["Attack"][0]);
    public bool SprintInput() => Input.GetKey(keyBindingsDict["Sprint"][0]);
    public bool DropAttackInput() => GetComboDown("DropAttack");
    public bool DashInput() => Input.GetKeyDown(keyBindingsDict["Dash"][0]);


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

//using System.Collections.Generic;
//using UnityEngine;

//public class InputManager : MonoBehaviour
//{
//    [Header("Key Bindings")]
//    [SerializeField] private List<KeyBinding> keyBindingsList = new List<KeyBinding>();

//    private Dictionary<string, KeyCode[]> keyBindingsDict;
//    public Vector2 MoveInput { get; private set; }

//    private bool jumpHeldLastFrame = false;
//    private bool jumpReleasedSinceLastJump = true;
//    private bool jumpPressedThisFrame = false;
//    private bool jumpHeldThisFrame = false;

//    private void Awake()
//    {
//        keyBindingsDict = new Dictionary<string, KeyCode[]>();
//        foreach (var binding in keyBindingsList)
//            keyBindingsDict[binding.ActionName] = binding.Keys;

//        LoadKeyBindings();
//    }

//    private void Update()
//    {
//        CacheInputs();
//    }

//    private void CacheInputs()
//    {
//        bool isHeld = Input.GetKey(keyBindingsDict["Jump"][0]);
//        jumpPressedThisFrame = !jumpHeldLastFrame && isHeld;
//        jumpHeldThisFrame = isHeld;

//        if (!isHeld)
//            jumpReleasedSinceLastJump = true;

//        jumpHeldLastFrame = isHeld;
//        float horizontal = 0;
//        float vertical = 0;

//        if (Input.GetKey(keyBindingsDict["MoveLeft"][0])) horizontal -= 1;
//        if (Input.GetKey(keyBindingsDict["MoveRight"][0])) horizontal += 1;

//        MoveInput = new Vector2(horizontal, vertical).normalized;
//    }

//    public Vector2 MovementInput() => MoveInput;

//    public bool JumpInput() => jumpPressedThisFrame && jumpReleasedSinceLastJump;

//    public bool IsJumpHeld() => jumpHeldThisFrame;

//    public void ConsumeJump() => jumpReleasedSinceLastJump = false;

//    public bool AttackInput() => Input.GetKeyDown(keyBindingsDict["Attack"][0]);
//    public bool SprintInput() => Input.GetKey(keyBindingsDict["Sprint"][0]);
//    public bool DropAttackInput() => GetComboDown("DropAttack");
//    public bool DashInput() => Input.GetKeyDown(keyBindingsDict["Dash"][0]);

//    private bool GetComboDown(string action)
//    {
//        if (!keyBindingsDict.ContainsKey(action)) return false;

//        KeyCode[] keys = keyBindingsDict[action];
//        if (keys.Length == 0) return false;

//        for (int i = 0; i < keys.Length - 1; i++)
//            if (!Input.GetKey(keys[i])) return false;

//        return Input.GetKeyDown(keys[keys.Length - 1]);
//    }

//    public void RebindKey(string action, KeyCode[] newKeys)
//    {
//        if (!keyBindingsDict.ContainsKey(action)) return;

//        keyBindingsDict[action] = newKeys;

//        for (int i = 0; i < newKeys.Length; i++)
//            PlayerPrefs.SetInt(action + i, (int)newKeys[i]);
//        PlayerPrefs.SetInt(action + "Length", newKeys.Length);
//        PlayerPrefs.Save();
//    }

//    private void LoadKeyBindings()
//    {
//        foreach (var binding in keyBindingsList)
//        {
//            int length = PlayerPrefs.GetInt(binding.ActionName + "Length", binding.Keys.Length);
//            KeyCode[] keys = new KeyCode[length];

//            for (int i = 0; i < length; i++)
//                keys[i] = (KeyCode)PlayerPrefs.GetInt(binding.ActionName + i, (int)binding.Keys[i]);

//            binding.Keys = keys;
//            keyBindingsDict[binding.ActionName] = keys;
//        }
//    }
//}
