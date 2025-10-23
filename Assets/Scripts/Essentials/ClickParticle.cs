using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickParticle : MonoBehaviour
{
    [SerializeField] ParticleSystem clickParticle;
    [SerializeField] ParticleSystem spaceParticle;

    void Update()
    {
        Vector3 mouseScreenPosition = Input.mousePosition;
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(mouseScreenPosition);
        Vector2 mouseWorldPosition2D = new Vector2(mouseWorldPosition.x, mouseWorldPosition.y);

        if(Input.GetKeyDown(KeyCode.Mouse0)) {
            var particle = Instantiate(clickParticle, mouseWorldPosition2D, Quaternion.identity);
            Destroy(particle.gameObject, particle.main.duration);
        }
        if(Input.GetKeyDown(KeyCode.Space) && spaceParticle) spaceParticle.Play();
    }
}
