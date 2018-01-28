using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using LoGameFrame;

public class LoTweenColor : LoTweenSingleBase
{

    [SerializeField]
    Color from;

    [SerializeField]
    Color to;

    public Color From
    {
        get { return from; }
        set { from = value; }
    }

    public Color To
    {
        get { return to; }
        set { to = value; }
    }

#if TWEEN_NGUI
    UIWidget sprite;
    UIWidget mSprite
    {
		get{
			if (sprite == null) sprite = GetComponent<UIWidget>();
			return sprite;
		}
        set {
            sprite = value;
        }
	}
#else
    Image sprite;
    Image mSprite
    {
        get
        {
            if (sprite == null) sprite = GetComponent<Image>();
            return sprite;
        }
        set { sprite = value; }
    }
#endif



    public void ResetSprites()
    {
        mSprite = null;
    }


    protected override void UpdateValue(float percent)
    {
        mSprite.color = LoTweenControl.Lerp(From, To, percent);
    }

    public override void ResetToBegin()
    {
        base.ResetToBegin();
        mSprite.color = From;
    }

    public override void ResetToEnd()
    {
        base.ResetToEnd();
        mSprite.color = To;
    }

}
