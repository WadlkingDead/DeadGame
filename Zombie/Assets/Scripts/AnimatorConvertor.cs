using UnityEngine;
using System.Collections;

public class AnimatorConvertor {

	public static void ChangeAnimal(Animator animator, RuntimeAnimatorController controler, AnimationClip[] clips)
	{
        if (animator == null)
        {
            return;
        }
		AnimatorOverrideController overrideController = new AnimatorOverrideController();
		overrideController.runtimeAnimatorController = controler;

		int tmpAnimalCount = controler.animationClips.Length;
		
		int tmpClipsCount = clips.Length;
		
		for (int i = 0; i < tmpAnimalCount; i++)
		{
			string tmpName = controler.animationClips[i].name;
			
			for (int j = 0; j < tmpClipsCount; j++)
			{
				string distName = clips[j].name;
				
				if (tmpName == distName)
				{
					overrideController[tmpName] = clips[j];
					
					break;
				}
			}
		}

		animator.runtimeAnimatorController = overrideController;
	}

    public static void ChangeAnimal(Animator animator, AnimalBundle bundle)
    {
        if (animator == null)
        {
            Debug.LogError("Animator Is Null !!");
            return;
        }
        if (bundle == null)
        {
            Debug.LogError("AnimalBundle Is Null !!");
            return;
        }
        ChangeAnimal(animator, bundle.controler, bundle.clips);
    }


}
