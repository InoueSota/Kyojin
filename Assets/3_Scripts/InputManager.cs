using UnityEngine;

public class InputManager : MonoBehaviour
{
    // Šî–{ƒNƒ‰ƒX
    public class InputPattern
    {
        public float input = 0f;
        public float preInput = 0f;

        private bool isGetInput = false;

        public void GetInput(string _inputName)
        {
            if (!isGetInput)
            {
                preInput = input;
                input = Input.GetAxisRaw(_inputName);

                isGetInput = true;
            }
        }
        public void SetIsGetInput(bool _isGetInput)
        {
            isGetInput = _isGetInput;
        }
    }

    // “ü—Í‚ÌŽí—Þ
    public InputPattern horizontal;
    public InputPattern vertical;
    public InputPattern a;
    public InputPattern b;
    public InputPattern lb;
    public InputPattern rb;

    void Start()
    {
        horizontal = new InputPattern();
        vertical = new InputPattern();
        a = new InputPattern();
        b = new InputPattern();
        lb = new InputPattern();
        rb = new InputPattern();
    }

    public void SetIsGetInput()
    {
        horizontal.SetIsGetInput(false);
        vertical.SetIsGetInput(false);
        a.SetIsGetInput(false);
        b.SetIsGetInput(false);
        lb.SetIsGetInput(false);
        rb.SetIsGetInput(false);
    }

    public void GetAllInput()
    {
        horizontal.GetInput("Horizontal");
        vertical.GetInput("Vertical");
        a.GetInput("A");
        b.GetInput("B");
        lb.GetInput("LB");
        rb.GetInput("RB");
    }

    public bool IsTrgger(InputPattern _inputPattern)
    {
        if (_inputPattern.input != 0f && _inputPattern.preInput == 0f)
        {
            return true;
        }
        return false;
    }

    public bool IsPush(InputPattern _inputPattern)
    {
        if (_inputPattern.input != 0f && _inputPattern.preInput != 0f)
        {
            return true;
        }
        return false;
    }

    public bool IsRelease(InputPattern _inputPattern)
    {
        if (_inputPattern.input == 0f && _inputPattern.preInput != 0f)
        {
            return true;
        }
        return false;
    }

    public float ReturnInputValue(InputPattern _inputPattern)
    {
        return _inputPattern.input;
    }
}
