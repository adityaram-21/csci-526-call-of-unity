using UnityEngine;

public class RackTester : MonoBehaviour
{
    public LetterRack rack;

    void Start()
    {
        rack.SetLetter(0, 'K');
        rack.SetLetter(1, 'E');
        rack.SetLetter(2, 'Y');
    }
}
