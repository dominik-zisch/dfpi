using StandardAssets.Characters.ThirdPerson;
using UnityEngine;

public class CustomPlayerMover : MonoBehaviour
{
    public  ThirdPersonBrain brain;

    [Range(0,1)]
    public float deadZoneThreshold;
    
    [Range(-1,1)]
    public float forwardStep;
    
    [Range(-1,1)]
    public float sidewayStep;
    
    private ThirdPersonInput input;

    private void Start()
    {
         input = (ThirdPersonInput) brain.thirdPersonInput;
    }

    private void Update()
    {
        var movement = new Vector2(sidewayStep,forwardStep );
        
        if (movement.magnitude > deadZoneThreshold )
        {
            input.moveInput = movement;
            brain.thirdPersonMotor.OnMove();
        }
        else
        {
            input.moveInput = Vector2.zero;
        }
    }


    public void Move(Vector2 movement)
    {
        sidewayStep = movement.x;
        forwardStep = movement.y;
    }

    public void Move(Vector3 movement)
    {
        sidewayStep = movement.x;
        forwardStep = movement.y;
    }
    
    public void Move(Vector4 movement)
    {
        sidewayStep = movement.x;
        forwardStep = movement.y;
    }
    
    public void Jump()
    {
        brain.thirdPersonMotor.OnJumpPressed();
    }
}
