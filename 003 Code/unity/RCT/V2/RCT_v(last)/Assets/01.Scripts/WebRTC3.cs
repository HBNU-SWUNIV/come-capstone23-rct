using System.Collections;
using UnityEngine;


public class WebRTC3 : MonoBehaviour
{

    // public GameManager gameManager;
    public Renderer cuberenderer;
    // public RTCDataChannel channel;
    // byte[] byte_ ;


    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("webrtc3 code in");

        StartCoroutine(Data_load());

        // StartCoroutine(WaitForGameManagerInitialization());
    }
    IEnumerator Data_load()
    {   
        while(true)
        {   
            if (GameManager.Instance.byte_ != null)
            {
                Texture2D texture = new Texture2D(2, 2);
                // Debug.Log(byte_);
                // texture.LoadImage(Convert.FromBase64String(System.Text.Encoding.UTF8.GetString(byte_)));
                texture.LoadImage(GameManager.Instance.byte_);
                // Apply the texture to the sphere
                cuberenderer.material.mainTexture = texture;
                cuberenderer.enabled = true;

            }
            yield return new WaitForSeconds(0.1f);
            
        }
        
    }
}
