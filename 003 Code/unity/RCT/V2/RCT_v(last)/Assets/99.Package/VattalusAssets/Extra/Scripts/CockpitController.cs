using System;
using System.Collections.Generic;
using UnityEngine;

public class CockpitController : MonoBehaviour
{
    [Header("Control Parameters")]
    public float turnspeed = 5f;
    public float strafeSpeed = 5f;
    public float yawTorque = 1f;
    public float pitchTorque = 1f;
    public float rollTorque = 1f;

    [Header("Input Representation")]
    public Transform joystick;
    private Vector3 joystickAngleTarget = new Vector3();

    public Transform yoke;
    private Vector3 yokeAngleTarget = new Vector3();
    private Vector3 yokeInitialPos = new Vector3();

    //Pedals typically are used for turning left/right
    public Transform leftPedal;
    public Transform rightPedal;

    private Rigidbody rb;

    [Serializable]
    public class CockpitAnimClass
    {
        [HideInInspector]
        public bool state = false;
        public KeyCode keyBinding;
        public Animation animComponent;
        public float animSpeed = 1f;
        public AudioSource openAudioSource;
        public AudioSource closeAudioSource;
    }
    public List<CockpitAnimClass> animationsList = new List<CockpitAnimClass>();

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        if (yoke != null) yokeInitialPos = yoke.transform.localPosition;

#if !UNITY_EDITOR
        Cursor.visible = false;
#endif
    }

    private void Update()
    {
        float roll = 0f;
        if (Input.GetKey(KeyCode.E)) roll -= 1;
        if (Input.GetKey(KeyCode.Q)) roll += 1;

        float pitch = 0f;
        if (Input.GetKey(KeyCode.S)) pitch -= 1;
        if (Input.GetKey(KeyCode.W)) pitch += 1;

        float yaw = 0f;
        if (Input.GetKey(KeyCode.A)) yaw -= 1;
        if (Input.GetKey(KeyCode.D)) yaw += 1;

        rb.AddRelativeTorque(pitch * turnspeed * Time.deltaTime * pitchTorque, yaw * turnspeed * Time.deltaTime * yawTorque,
            roll * turnspeed * Time.deltaTime * rollTorque);

        //Move joystick
        if (joystick != null)
        {
            joystickAngleTarget = new Vector3(
                Mathf.Lerp(-25f, 25f, (pitch + 1f) / 2f),
               Mathf.Lerp(20f, -20f, (roll + 1f) / 2f),
               Mathf.Lerp(-19f, 19f, (yaw + 1f) / 2f));

            joystick.transform.localRotation = Quaternion.Lerp(joystick.transform.localRotation, Quaternion.Euler(joystickAngleTarget), 5f * Time.deltaTime);
        }

        //Move yoke
        if (yoke != null)
        {
            yokeAngleTarget = new Vector3(0f, Mathf.Lerp(60f, -60f, (roll + 1f) / 2f), 0f);

            yoke.transform.localRotation = Quaternion.Lerp(yoke.transform.localRotation, Quaternion.Euler(yokeAngleTarget), 2.5f * Time.deltaTime);
            yoke.transform.localPosition = Vector3.Lerp(yoke.transform.localPosition, yokeInitialPos + new Vector3(0f, -0.06f * pitch, 0f), 2.5f * Time.deltaTime);
        }

        //Move pedals
        if (leftPedal != null && rightPedal != null)
        {
            float pedalAngle = Mathf.Lerp(20f, -20f, (yaw + 1f) / 2f);
            leftPedal.transform.localRotation = Quaternion.Lerp(leftPedal.transform.localRotation, Quaternion.Euler(new Vector3(pedalAngle, 0f, 0f)), 2.5f * Time.deltaTime);
            rightPedal.transform.localRotation = Quaternion.Lerp(rightPedal.transform.localRotation, Quaternion.Euler(new Vector3(-pedalAngle, 0f, 0f)), 2.5f * Time.deltaTime);
        }

        //Check for keypresses to play animations
        if (animationsList != null && animationsList.Count > 0)
        {
            foreach (var anim in animationsList)
            {
                if (Input.GetKeyDown(anim.keyBinding) && anim.animComponent != null && anim.animComponent.clip != null)
                {
                    if (anim.animComponent.IsPlaying("Take 001")) return;

                    anim.state = !anim.state;

                    if (anim.state)
                    {
                        anim.animComponent[anim.animComponent.clip.name].time = 0f;
                        anim.animComponent[anim.animComponent.clip.name].speed = anim.animSpeed;
                        anim.animComponent.Play();

                        if (anim.openAudioSource != null) anim.openAudioSource.Play();
                    }
                    else
                    {
                        anim.animComponent[anim.animComponent.clip.name].time = anim.animComponent[anim.animComponent.clip.name].length;
                        anim.animComponent[anim.animComponent.clip.name].speed = -anim.animSpeed;
                        anim.animComponent.Play();

                        if (anim.closeAudioSource != null) anim.closeAudioSource.Play();
                    }
                }
            }
        }

        //#if !UNITY_EDITOR
        //        if (Input.GetKeyDown(KeyCode.Escape))
        //            Application.Quit();
        //#endif
    }
}
