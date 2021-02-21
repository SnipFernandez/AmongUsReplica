using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMarkerPJController : MonoBehaviour
{
    public Text tName;
    public Button buttonVote;
    public Image icon;
    public List<Image> voting = new List<Image>();
    public UIScreenVoteController screenVoteController;

    public string Name { get { return tName.text; } set { tName.text = value; } }

    public void setColor(Color color) {
        icon.color = color;
    }

    // Start is called before the first frame update
    void Start()
    {
        buttonVote.interactable = true;
        VotingInactive();
    }

    private void VotingInactive()
    {
        List<Image> voting2 = voting.FindAll(v => v.gameObject.activeSelf);
        foreach (Image votingImg in voting2)
        {
            votingImg.gameObject.SetActive(false);
        }
    }

    private void OnEnable()
    {
        buttonVote.interactable = true;
        VotingInactive();
    }

    public void doVote() {
        buttonVote.interactable = false;
        Image vote = voting.Find(v => !v.gameObject.activeSelf);
        vote.color = Color.red;
        vote.gameObject.SetActive(true);
        screenVoteController.inactiveAll();
    }
}
