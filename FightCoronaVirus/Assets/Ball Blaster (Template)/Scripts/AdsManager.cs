using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Monetization;
using BallBlaster;

public class AdsManager : MonoBehaviour
{


    [Header("- Texts -")]
    [SerializeField] private Text _coinsCountText;

    public int coinReward = 150;
    public GameObject rewardAdButton;

    [Header("- Picked UP coin effect -")]
    [SerializeField] private Transform _parentCanvas;
    [SerializeField] private GameObject _pickedUpCoinTextPrefab;
    [SerializeField] private Vector3 _pickedCoinTextOffset;


    [Header("- Sound -")]
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _rewardClip;


    private string gameId = "3494152";
    string placementId_video = "video";
    string placementId_rewardedVideo = "rewardVideo";

    private bool testMode = true;

    void Start()
    {
        if (Application.platform == RuntimePlatform.IPhonePlayer)
            gameId = "3494153";
        else if (Application.platform == RuntimePlatform.Android)
            gameId = "3494152";


        Monetization.Initialize(gameId, testMode);
    }

    public void ShowAd()
    {
        StartCoroutine(WaitForAd());
    }


    public void ShowRewardedAd()
    {
        StartCoroutine(WaitForAd(true));
    }


    IEnumerator WaitForAd(bool reward = false)
    {
        string placementId = reward? placementId_rewardedVideo : placementId_video;
        while (!Monetization.IsReady(placementId))
        {
            yield return null;
        }

        ShowAdPlacementContent ad = null;
        ad = Monetization.GetPlacementContent(placementId) as ShowAdPlacementContent;

        if (ad != null)
        {
            if (reward)
                ad.Show(AdFinished);
            else
                ad.Show();
        }
    }

    void AdFinished(ShowResult result)
    {
        if (result == ShowResult.Finished)
        {
            //Reward the player

            Data.SetCoinsCount(Data.GetCoinsCount() + coinReward);
            _coinsCountText.text = NumberFormatter.ToKMB(Data.GetCoinsCount());

            // Spawn textObject
            GameObject textObject = Instantiate(_pickedUpCoinTextPrefab, _pickedCoinTextOffset, _pickedUpCoinTextPrefab.transform.rotation, _parentCanvas);
            textObject.GetComponentInChildren<Text>().text = coinReward.ToString();
            Destroy(textObject, 2f);

            PlaySound(_rewardClip);


            

        }
    }

    private void PlaySound(AudioClip clip)
    {
        if (_audioSource)
        {
            _audioSource.clip = clip;
            _audioSource.Play();
        }
    }



}
