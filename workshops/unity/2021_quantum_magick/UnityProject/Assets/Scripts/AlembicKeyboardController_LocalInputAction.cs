using UnityEngine;
using UnityEngine.Formats.Alembic.Importer;
using UnityEngine.InputSystem;

public class AlembicKeyboardController_LocalInputAction : MonoBehaviour
{
    // REFERENCE (PLACEHOLDER) TO OUR ALEMBIC OBJECT
    public AlembicStreamPlayer alembic;

    // LOCAL INPUT ACTION
    public InputAction inputKeys;

    // HOW FAST TO MOVE THE ANIMATION
    [Range(0, 0.2F)]
    public float speed;

    // WHEN THE GAME BEGINS
    public void Start()
    {
        // ENABLE OUR INPUT ACTION
        inputKeys.Enable();
    }

    // AT EVERY FRAME
    void Update()
    {
        // MOVE THE CURRENT TIME OF THE ALEMBIC BACKWARD OR FORWARD DEPENDING ON OUR KEY INPUT
        alembic.CurrentTime += speed * inputKeys.ReadValue<float>();    
    }

}
