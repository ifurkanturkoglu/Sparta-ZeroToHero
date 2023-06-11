using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController Instance;
    [SerializeField] Transform player;
    [SerializeField] float camSpeed = 0.0125f;
    [SerializeField]public  Vector3 offset = new Vector3(0,8,-10);
    Vector3 a = Vector3.zero;

    float shakeIntensity = 0.4f;
    float shakeTimer;
    Vector3 orginalPosition;

    bool isSkillAnimation;
    void Start()
    {
        if(Instance == null)
            Instance = this;
    }
    void Update()
    {
        if(shakeTimer > 0){
            transform.localPosition = orginalPosition + Random.insideUnitSphere*shakeIntensity;
            shakeTimer -= Time.deltaTime;
        }
    }
    void LateUpdate()
    {   
        transform.localPosition = Vector3.SmoothDamp(transform.position,player.transform.position+offset,ref a,camSpeed);
        transform.LookAt(player);
    }

    public void ScreenShake(float shakeTime){
        orginalPosition = transform.localPosition;
        shakeTimer = shakeTime;
    }

    public IEnumerator skillAnimaton(int skillType){
        float time =0;
        while(Time.timeScale >= 0.5f){
            Time.timeScale -= Time.deltaTime;
        }
        while(time <.5f){
            time += Time.deltaTime;
            offset += new Vector3(0,0,Time.deltaTime*20);
            yield return null;
        }
        
        yield return new WaitForSeconds(.2f);
        PlayerController.Instance.Force(PlayerController.Instance.skillTypes[skillType - 1].skillEffectScale);
        Time.timeScale = 1;
        while(time >= 0){
            time -= Time.deltaTime;
            offset -= new Vector3(0,0,Time.deltaTime*20);
            yield return null;
        }
        
    }
}
