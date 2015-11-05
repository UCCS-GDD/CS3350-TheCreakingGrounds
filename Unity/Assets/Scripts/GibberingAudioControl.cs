using UnityEngine;
using System.Collections;
using UnityEditor.Audio;
using Assets.Scripts;

public class GibberingAudioControl : MonoBehaviour {

	//Load all assets from proper path "Resources" is root.
	AudioClip[] sounds;

	//Pull AudioSource components from the gameObject this script is attached to.
	AudioSource[] sources;

	// Use this for initialization
	void Start () {
	
	}

	void Awake () {

		sounds = Resources.LoadAll<AudioClip> ("Sounds/GibberingMadnessTracks");
		sources = gameObject.GetComponents<AudioSource> ();

		//Upon instantiation, run through each source and assign a clip to it from array sounds.
		//Then play that clip.
		foreach (AudioSource source in sources) {
			int index = Random.Range (0,sounds.Length);
			source.clip = sounds[index];
			source.Play ();
		}
	}
	
	// Update is called once per frame
	void Update () {
		//As the game runs, check each source to make sure it is playing something
		//If a source has finished playing (!isPlaying) then pickTrack to either assign a new track or repeat current.
		//Then play the new clip.
		foreach (AudioSource source in sources) {
			if (!(source.isPlaying)) {
				pickTrack(source, source.clip);
				source.Play ();
			}
			//If the source is playing still, do nothing to it, allow it to run unhindered.
		}
	}

	void pickTrack(AudioSource source, AudioClip clip) {
		//First check to see if we are selecting a new track or sticking with the same one.
		if (Random.value < GameSettings.chanceToRepeatTrack) {
			//this checks against GameSettings variable for repetition.
			return;
		} else { //We are not repeating, so we are selecting a new clip.

			int index = Random.Range (0, sounds.Length);
			AudioClip newClip = sounds[index]; //assigns a new clip to the index determined randomly.
			if (clip.Equals(newClip)) { //if the newClip is the same as the clip we were just playing we do stuff.
				//just shifts by one, if at max, wrap to 0.
				if (index < sounds.Length-1) { 
					newClip = sounds[index + 1];
				} else {
					newClip = sounds[0];
				}
			}

			source.clip = newClip; //After checks, assigns source.clip to be the newly made and assigned clip.
		}
		return;
	}

}
