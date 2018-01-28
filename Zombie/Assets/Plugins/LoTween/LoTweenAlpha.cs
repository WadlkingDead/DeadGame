using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using LoGameFrame;

public class LoTweenAlpha : LoTweenSingleBase {

    [SerializeField]
    float from;

    [SerializeField]
    float to;
	
	public float From {
		get { return from; }
		set { from = value; }
	}
	
	public float To {
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

    public bool includeChildren = false;

    Image sprite;
	Image mSprite{
		get{
			if (sprite == null) sprite = GetComponent<Image>();
			return sprite;
		}
        set { sprite = value; }
	}

    CanvasGroup group;
    CanvasGroup mGroup {
        get {
            if (!group)
            {
                group = GetComponent<CanvasGroup>();
                if (!group)
                    group = gameObject.AddComponent<CanvasGroup>();
            }
            
            return group;
        }
    }


#endif



    public float Alpha {
        get {
#if TWEEN_NGUI
            return mSprite.color.a;
#else
            if (includeChildren)
                return mGroup.alpha;
            else
                return mSprite.color.a;
#endif
        }
        set {
#if TWEEN_NGUI
            Color color;
            color = mSprite.color;
            color.a = value;
            mSprite.color = color;
#else
            if (includeChildren)
            {
                mGroup.alpha = value;
            }
            else {
                Color color;
                color = mSprite.color;
                color.a = value;
                mSprite.color = color;
            }
#endif
        }
    }

    public void ResetSprites()
    {
        mSprite = null;
    }

	
	protected override void UpdateValue (float percent)
	{
        if (fixedValue)
            Alpha = percent;
        else
            Alpha = Mathf.Lerp(From, To, percent);
	}

    public override void ResetToBegin()
    {
        base.ResetToBegin();
        Alpha = From;
    }

    public override void ResetToEnd()
    {
        base.ResetToEnd();
        Alpha = To;
    }

}
