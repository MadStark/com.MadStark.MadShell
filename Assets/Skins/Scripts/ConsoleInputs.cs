using UnityEngine;

namespace MadStark.MadShell
{
    [DefaultExecutionOrder(-100)]
    public class ConsoleInputs : MonoBehaviour
    {
        [SerializeField] private ConsoleBehaviour console;
        [SerializeField] private KeyCode toggleKey = KeyCode.Tilde;


        private void Awake()
        {
            if (console == null)
                enabled = false;
        }

        private void Update()
        {
            if (Input.GetKeyDown(toggleKey) && !console.Focused)
            {
                if (console.Visible)
                    console.Hide();
                else
                    console.ShowAndFocus();
            }
        }

        private void Reset()
        {
            console = GetComponent<ConsoleBehaviour>();
        }
    }
}
