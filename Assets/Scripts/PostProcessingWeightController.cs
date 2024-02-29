using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PostProcessingWeightController : MonoBehaviour
{
    public string targetLayerName = "NoPostEffect";
    public PostProcessVolume postProcessVolume;
    public Camera mainCamera;
    public Camera Camera;
    public float transitionSpeed = 0.5f;
    private float targetWeight = 0f;
    private bool transitioning = false;
    private bool cameraEnabled = true;
    private GameObject lastClickedObject; 
    

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) 
        {
            Ray ray = Camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (lastClickedObject != null)
                {
                    //클릭된 오브젝트의 레이어를 원래대로 복원
                    lastClickedObject.layer = LayerMask.NameToLayer("Default");
                }

                //클릭된 오브젝트의 레이어를 변경
                hit.transform.gameObject.layer = LayerMask.NameToLayer(targetLayerName);

                //클릭된 오브젝트를 기억
                lastClickedObject = hit.transform.gameObject;

                // weight 값을 반전
                targetWeight = Mathf.Abs(targetWeight - 1);
                transitioning = true;

                // 카메라 체크박스
                Camera.enabled = !Camera.enabled;
                cameraEnabled = Camera.enabled;
            }
        }

        // weight 값 조절
        if (transitioning)
        {
            float currentWeight = postProcessVolume.weight;
            currentWeight = Mathf.MoveTowards(currentWeight, targetWeight, transitionSpeed * Time.deltaTime);
            postProcessVolume.weight = currentWeight;
            if (Mathf.Approximately(currentWeight, targetWeight))
            {
                transitioning = false;
            }
        }
    }

}
