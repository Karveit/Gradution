﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using DreamerTool.Util;
using DreamerTool.UI;
using DreamerTool.GameObjectPool;
public class GameScene : Scene
{
    float timer;
    private int hit_count;
    public Text hit_count_text;
    public int HitCount {
        get
        {
            return hit_count;
        }
        set
        {
            hit_count = value;
            hit_count_text.GetComponent<DOTweenAnimation>().DORestart();
            hit_count_text.text = hit_count + "  Combo";
        }
    }
    
    public void LoadSceneJumpByFadeAnim(string scene_name)
    {
        GetView<GameInfoView>().FadeAnim(() => {
            LoadingScene.LoadScene(scene_name);
        });
    }
    private void Awake()
    {
        base.Awake();
        GameInit();
      StartCoroutine( Util.GetDateTimeFromURL());
    }
    public static void GameInit()
    {
        GameObjectPoolManager.InitByScriptableObject();
        ActorModel model = new ActorModel();
        SkillModel.Init();
    }
    // Start is called before the first frame update
    void Start()
    {
       
 
    }
    public void TransferPlayerByFadeAnim(Transform pos)
    {
        GetView<GameInfoView>().FadeAnim(() => { ActorController._controller.Transfer(pos);
            Camera.main.GetComponent<Cinemachine.CinemachineBrain>().ActiveVirtualCamera.VirtualCameraGameObject.GetComponent<Cinemachine.CinemachineConfiner>().InvalidatePathCache();
        Camera.main.GetComponent<Cinemachine.CinemachineBrain>().ActiveVirtualCamera.VirtualCameraGameObject.GetComponent<Cinemachine.CinemachineConfiner>().m_BoundingShape2D = pos.gameObject.GetComponent<PolygonCollider2D>(); });
    }
    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale != 1)
        {

            timer += Time.unscaledDeltaTime;

            if (timer >= 0.15f)
            {

                timer = 0;
                Time.timeScale = 1f;
            }
        }
        else
        {
            timer = 0;
        }
    }
}
