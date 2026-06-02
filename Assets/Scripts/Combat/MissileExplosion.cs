using UnityEngine;

public class MissileExplosion : Explosive
{
    private bool hasExploded = false;

    private void OnCollisionEnter(Collision collision)
    {
        if (hasExploded)
            return;

        hasExploded = true;

        Explode();
    }
}