using System;
using UnityEngine;

namespace BahaTurret
{
    public class ShellCasing : MonoBehaviour
    {
        public float startTime;
        public Vector3 initialV;

        Vector3 velocity;
        Vector3 angularVelocity;

        float atmDensity;

        void OnEnable()
        {
            startTime = Time.time;
            velocity = initialV;
            velocity += transform.rotation*
                        new Vector3(UnityEngine.Random.Range(-.1f, .1f), UnityEngine.Random.Range(-.1f, .1f),
                            UnityEngine.Random.Range(6f, 8f));
            angularVelocity =
                new Vector3(UnityEngine.Random.Range(-10f, 10f), UnityEngine.Random.Range(-10f, 10f),
                    UnityEngine.Random.Range(-10f, 10f))*10;

            atmDensity =
                (float)
                FlightGlobals.getAtmDensity(
                    FlightGlobals.getStaticPressure(transform.position, FlightGlobals.currentMainBody),
                    FlightGlobals.getExternalTemperature(), FlightGlobals.currentMainBody);
        }

        void FixedUpdate()
        {
            if (!gameObject.activeInHierarchy)
            {
                return;
            }

            //gravity
            velocity += FlightGlobals.getGeeForceAtPosition(transform.position)*TimeWarp.fixedDeltaTime;

            //drag
            velocity -= 0.005f*velocity*atmDensity;

            transform.rotation *= Quaternion.Euler(angularVelocity*TimeWarp.fixedDeltaTime);
            transform.position += velocity*TimeWarp.deltaTime;

            if (BDArmorySettings.SHELL_COLLISIONS)
            {
                RaycastHit hit;
                if (Physics.Linecast(transform.position, transform.position + velocity*Time.fixedDeltaTime, out hit,
                    557057))
                {
                    velocity = Vector3.Reflect(velocity, hit.normal);
                    velocity *= 0.55f;
                    velocity = Quaternion.AngleAxis(UnityEngine.Random.Range(0f, 90f), UnityEngine.Random.onUnitSphere)*
                               velocity;
                }
            }
        }

        void Update()
        {
            if (!gameObject.activeInHierarchy)
            {
                return;
            }

            if (Time.time - startTime > 2)
            {
                gameObject.SetActive(false);
            }
        }
    }
}