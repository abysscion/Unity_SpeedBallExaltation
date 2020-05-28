using Controllers;
using UnityEngine;

public class MetallicAsteroidCollisionScript : MonoBehaviour
{
    private void OnCollisionEnter(Collision other)
    {
        if (!other.transform.CompareTag("Player")) return;
        
        var jumper = other.gameObject.GetComponent<JumpController>();
        var jumperRb = jumper.GetComponent<Rigidbody>();

        if (jumper.isStick)
        {
            jumper.UnstickBall();
            jumperRb.velocity = Vector3.down * 6;
        }
        else
            jumperRb.velocity = Vector3.down * 4;
        SoundController.Instance.PlaySound(SoundController.SoundName.MetallicAsteroidHit);
    }
}
