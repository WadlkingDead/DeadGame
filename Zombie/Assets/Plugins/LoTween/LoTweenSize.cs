using UnityEngine;
using System.Collections;
using LoGameFrame;

public class LoTweenSize : LoTweenSingleBase
{

    [SerializeField]
    Vector2 from;

    [SerializeField]
    Vector2 to;

    public Vector2 From
    {
        get { return from; }
        set { from = value; }
    }

    public Vector2 To
    {
        get { return to; }
        set { to = value; }
    }

    RectTransform sprite;
    RectTransform mSprite
    {
        get
        {
            if (sprite == null) sprite = GetComponent<RectTransform>();
            return sprite;
        }
        set { sprite = value; }
    }


    public void ResetSprites()
    {
        mSprite = null;
    }


    protected override void UpdateValue(float percent)
    {
        mSprite.sizeDelta = LoTweenControl.Lerp(From, To, percent);
    }

    public override void ResetToBegin()
    {
        base.ResetToBegin();
        mSprite.sizeDelta = From;
    }

    public override void ResetToEnd()
    {
        base.ResetToEnd();
        mSprite.sizeDelta = To;
    }

}
