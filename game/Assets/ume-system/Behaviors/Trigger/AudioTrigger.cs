using System;
using UnityEngine;


namespace UME
{
    [AddComponentMenu("UME/Triggers/AudioTrigger")]
	[Serializable]
    public class AudioTrigger : BaseTrigger{
		
	    public AudioClip clip;
	    [Range(1f, 10f)] public float volumeMax = 1.0f;
		[HideInInspector]public bool velocityVolume = false;
		[HideInInspector]public AudioSource audioSrc;
	  
		public override void Initialize()
	    {
			
			if (clip != null) {
				clip.LoadAudioData ();
				audioSrc = gameObject.AddComponent<AudioSource>();
				audioSrc.playOnAwake = false;
				audioSrc.priority = 1;
				audioSrc.clip = clip;
			}

	    }

		public override void Activate(Collider2D other)
		{

			if (audioSrc != null) {
				float volume = volumeMax;
				if (velocityVolume){
					volume = Math.Min(other.attachedRigidbody.velocity.magnitude, volumeMax);
				}
				audioSrc.PlayOneShot (clip, volume);
			}
		

		}
	}
}
