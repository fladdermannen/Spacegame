using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupController : MonoBehaviour {


    public Animator animator;
    public Text pointsText;

	// Use this for initialization
	void Start () {
        AnimatorClipInfo[] clipInfo = animator.GetCurrentAnimatorClipInfo(0);
        Destroy(gameObject, clipInfo[0].clip.length);
	}
	
	public void SetText(string text)
    {
        pointsText.text = text;
    }

    public void SetFontSize(int s)
    {
        pointsText.fontSize = s;
    }
}
