using UnityEngine;
using UnityEngine.Formats.Alembic.Importer;
using UnityEngine.InputSystem;

public class AlembicAnimationController : MonoBehaviour
{
    // REFERENCE (PLACEHOLDER) TO OUR ALEMBIC OBJECT
    public AlembicStreamPlayer alembic;

    // LOCAL INPUT ACTION
    public InputAction inputKeys;

    // HOW FAST TO MOVE THE ANIMATION
    [Range(0, 0.2F)]
    public float speed;

    public void Start()
    {
        // ENABLE OUR INPUT ACTION
        inputKeys.Enable();
    }

    // AT EVERY FRAME
    void Update()
    {
        // MOVE THE CURRENT TIME OF THE ALEMBIC BACKWARD OR FORWARD DEPENDING ON THE PROVIDED VALUE
        alembic.CurrentTime += speed * GetValue();    
    }

    public virtual float GetValue()
    {
        // GET KEYBOARD INPUT
        return inputKeys.ReadValue<float>();
    }

}
