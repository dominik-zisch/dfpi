using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Formats.Alembic.Importer;

public class VectorToFloatConverter : MonoBehaviour
{
    public List<AlembicStreamPlayer> alembicPlayers;

    [ReadOnly]
    public float rawValue;
   
    [ReadOnly]
    public float[] validatedValues;
    
    // Start is called before the first frame update
    void Start()
    { }

    // Update is called once per frame
    void Update()
    { }

    public void Convert(Vector3 vector)
    {
        rawValue       = vector.y;

        if (alembicPlayers == null)
            return;
        
        if (validatedValues == null || validatedValues.Length != alembicPlayers.Count)
        {
            validatedValues = new float[alembicPlayers.Count];
        }
        
        for (int i = 0; i < alembicPlayers.Count; i++)
        {
            var alembicPlayer = alembicPlayers[i];
            
            // IF RAWVALUE IS SMALLER THAN THIS ALEMBIC PLAYER'S START TIME
            if (rawValue < alembicPlayer.StartTime)
            {
                validatedValues[i] = alembicPlayer.StartTime;  
            }
            // IF RAWVALUE IS LARGER THAN THIS ALEMBIC PLAYER'S END TIME
            else if (rawValue > alembicPlayer.EndTime)
            {
                validatedValues[i] = alembicPlayer.EndTime;
            }
            // OTHERWISE JUST PASS THE VALUE IN
            else
            {
                validatedValues[i] = rawValue;
            }
        
            alembicPlayer.CurrentTime = validatedValues[i];
        }
    }
}
