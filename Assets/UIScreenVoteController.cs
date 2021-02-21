using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIScreenVoteController : Menu
{
    float time = 30;
    public Button buttonVoteWhite;
    float acumulateTime = 0;
    public Text visualTime;
    public List<Image> votingWhite = new List<Image>();
    public List<UIMarkerPJController> voting = new List<UIMarkerPJController>();
    public UILevel01Controller uilevel;


    // Start is called before the first frame update
    void Start()
    {
        time = 30;
        visualTime.text = $"Tiempo restante: {time.ToString()}s";
        buttonVoteWhite.interactable = true;
        VotingInactive();
    }

    private void VotingInactive()
    {
        List<Image> voting2 = votingWhite.FindAll(v => v.gameObject.activeSelf);
        foreach (Image votingImg in voting2)
        {
            votingImg.gameObject.SetActive(false);
        }
    }

    private void OnEnable()
    {
        time = 120;
        visualTime.text = $"Tiempo restante: {time.ToString()}s";
        buttonVoteWhite.interactable = true;
        VotingInactive();
    }

    // Update is called once per frame
    void Update()
    {
        if(time > 0)
        {
            acumulateTime += Time.deltaTime;
            if (acumulateTime >= 1)
            {
                acumulateTime = 0;
                time--;
                visualTime.text = $"Tiempo restante: {time.ToString()}s";
            }
        }
        else
        {
            // se acabo el tiempo
            close();
        }
    }

    internal void inactiveAll()
    {
        buttonVoteWhite.interactable = false;
        foreach(UIMarkerPJController v in voting)
        {
            v.buttonVote.interactable = false;
        }
    }

    public void doVote()
    {
        buttonVoteWhite.interactable = false;
        Image vote = votingWhite.Find(v => !v.gameObject.activeSelf);
        vote.color = Color.red;
        vote.gameObject.SetActive(true);
        inactiveAll();
    }

    public void close()
    {
        uilevel.player.SendMessage("unlock");
        base.close();
    }
}
