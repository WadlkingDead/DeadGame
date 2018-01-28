using UnityEngine;
using System.Collections;
using LoGameFrame;

public class LoTweenVolume : LoTweenSingleBase
{

    [SerializeField]
    float from;

    [SerializeField]
    float to;

    public float From
    {
        get { return from; }
        set { from = value; }
    }

    public float To
    {
        get { return to; }
        set { to = value; }
    }

    AudioSource player;
    AudioSource mPlayer
    {
        get
        {
            if (player == null) player = GetComponent<AudioSource>();
            return player;
        }
        set { player = value; }
    }

    public float Volume
    {
        get { return mPlayer.volume; }
        set { mPlayer.volume = value; }
    }


    protected override void UpdateValue(float percent)
    {
        if (fixedValue)
            Volume = percent;
        else
            Volume = Mathf.Lerp(From, To, percent);
    }

    public override void ResetToBegin()
    {
        base.ResetToBegin();
        Volume = From;
    }

    public override void ResetToEnd()
    {
        base.ResetToEnd();
        Volume = To;
    }

}
