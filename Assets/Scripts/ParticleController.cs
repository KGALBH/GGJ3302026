using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleController : MonoBehaviour
{
    ParticleSystem particleSystem;

    [Range(0f, 10f)]
    [SerializeField] int occurAfterVelocity;

    [Range(0f, 0.2f)]
    [SerializeField] float dustFormationPeriod;

    [SerializeField] Rigidbody2D playerRigidbody;

    float counter;

    // Start is called before the first frame update
    void Start()
    {
        particleSystem = GetComponent<ParticleSystem>();
        particleSystem.Stop();
    }

    // Update is called once per frame
    void Update()
    {
        counter += Time.deltaTime;

        print(particleSystem.isPlaying);
        if(Mathf.Abs(playerRigidbody.velocity.x) > occurAfterVelocity || Mathf.Abs(playerRigidbody.velocity.y) > occurAfterVelocity)
        {
            if(counter > dustFormationPeriod)
            {
                particleSystem.Emit(3);
                counter = 0;
            }
        }
    }
}
