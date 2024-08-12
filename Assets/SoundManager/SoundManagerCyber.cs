
using UnityEngine;

using System.Collections;
using System.Collections.Generic;

namespace CyberQuest.SoundManagerOrder
{
    /// <summary>
    /// Provides an easy wrapper to looping audio sources with nice transitions for volume when starting and stopping
    /// </summary>
    public class LoopingAudioSourceQuest
    {
        /// <summary>
        /// The audio source that is looping
        /// </summary>
        public AudioSource AudioSource { get; private set; }

        /// <summary>
        /// The target volume
        /// </summary>
        public float TargetVolume { get; set; }

        /// <summary>
        /// The original target volume - useful if the global sound volume changes you can still have the original target volume to multiply by.
        /// </summary>
        public float OriginalTargetVolume { get; private set; }

        /// <summary>
        /// Is this sound stopping?
        /// </summary>
        public bool Stopping { get; private set; }

        /// <summary>
        /// Whether the looping audio source persists in between scene changes
        /// </summary>
        public bool Persist { get; private set; }

        /// <summary>
        /// Tag for the looping audio source
        /// </summary>
        public int Tag { get; set; }

        private float startVolume2;
        private float startMultiplier2;
        private float stopMultiplier2;
        private float currentMultiplier2;
        private float timestamp2;
        private bool paused2;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="audioSource">Audio source, will be looped automatically</param>
        /// <param name="startMultiplier">Start multiplier - seconds to reach peak sound</param>
        /// <param name="stopMultiplier">Stop multiplier - seconds to fade sound back to 0 volume when stopped</param>
        /// <param name="persist">Whether to persist the looping audio source between scene changes</param>
        public LoopingAudioSourceQuest(AudioSource audioSource, float startMultiplier, float stopMultiplier, bool persist)
        {
            AudioSource = audioSource;
            if (audioSource != null)
            {
                AudioSource.loop = true;
                AudioSource.volume = 0.0f;
                AudioSource.Stop();
            }

            this.startMultiplier2 = currentMultiplier2 = startMultiplier;
            this.stopMultiplier2 = stopMultiplier;
            Persist = persist;
        }

        /// <summary>
        /// Play this looping audio source
        /// </summary>
        /// <param name="targetVolume">Max volume</param>
        /// <param name="isMusic">True if music, false if sound effect</param>
        /// <returns>True if played, false if already playing or error</returns>
        public bool PlayCyber(float targetVolume, bool isMusic)
        {
            if (AudioSource != null)
            {
                AudioSource.volume = startVolume2 = (AudioSource.isPlaying ? AudioSource.volume : 0.0f);
                AudioSource.loop = true;
                currentMultiplier2 = startMultiplier2;
                OriginalTargetVolume = targetVolume;
                TargetVolume = targetVolume;
                Stopping = false;
                timestamp2 = 0.0f;
                if (!AudioSource.isPlaying)
                {
                    AudioSource.Play();
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Stop this looping audio source. The sound will fade out smoothly.
        /// </summary>
        public void StopCyber()
        {
            if (AudioSource != null && AudioSource.isPlaying && !Stopping)
            {
                startVolume2 = AudioSource.volume;
                TargetVolume = 0.0f;
                currentMultiplier2 = stopMultiplier2;
                Stopping = true;
                timestamp2 = 0.0f;
            }
        }

        /// <summary>
        /// Pauses the looping audio source
        /// </summary>
        public void PauseCyber()
        {
            if (AudioSource != null && !paused2 && AudioSource.isPlaying)
            {
                paused2 = true;
                AudioSource.Pause();
            }
        }

        /// <summary>
        /// Resumes the looping audio source
        /// </summary>
        public void ResumeCyber()
        {
            if (AudioSource != null && paused2)
            {
                paused2 = false;
                AudioSource.UnPause();
            }
        }

        /// <summary>
        /// Update this looping audio source
        /// </summary>
        /// <returns>True if finished playing, false otherwise</returns>
        public bool Update()
        {
            if (AudioSource != null && AudioSource.isPlaying)
            {
                if ((AudioSource.volume = Mathf.Lerp(startVolume2, TargetVolume, (timestamp2 += Time.deltaTime) / currentMultiplier2)) == 0.0f && Stopping)
                {
                    AudioSource.Stop();
                    Stopping = false;
                    return true;
                }
                else
                {
                    return false;
                }
            }

            return !paused2;
        }
    }

    /// <summary>
    /// Sound manager extension methods
    /// </summary>
    public static class SoundManagerExtensionsCyber
    {
        /// <summary>
        /// Play an audio clip once using the global sound volume as a multiplier
        /// </summary>
        /// <param name="source">AudioSource</param>
        /// <param name="clip">Clip</param>
        public static void PlayOneShotSoundManagedCyber(this AudioSource source, AudioClip clip)
        {
            SoundManagerCyber.PlayOneShotSoundCyber(source, clip, 1.0f);
        }
       
        /// <summary>
        /// Play a music track and loop it until stopped, using the global music volume as a modifier
        /// </summary>
        /// <param name="source">Audio source to play</param>
        /// <param name="volumeScale">Additional volume scale</param>
        /// <param name="fadeSeconds">The number of seconds to fade in and out</param>
        /// <param name="persist">Whether to persist the looping music between scene changes</param>
        public static void PlayLoopingMusicManagedCyber(this AudioSource source, float volumeScale, float fadeSeconds, bool persist)
        {
            SoundManagerCyber.PlayLoopingMusicCyber(source, volumeScale, fadeSeconds, persist);
        }

       }

    /// <summary>
    /// Do not add this script in the inspector. Just call the static methods from your own scripts or use the AudioSource extension methods.
    /// </summary>
    public class SoundManagerCyber : MonoBehaviour
    {
        private static int persistTag2 = 0;
        private static bool needsInitialize2 = true;
        private static GameObject root2;
        private static SoundManagerCyber instance2;
        private static readonly List<LoopingAudioSourceQuest> music2 = new List<LoopingAudioSourceQuest>();
        private static readonly List<LoopingAudioSourceQuest> sounds2 = new List<LoopingAudioSourceQuest>();
        private static readonly HashSet<LoopingAudioSourceQuest> persistedSounds2 = new HashSet<LoopingAudioSourceQuest>();
        private static readonly Dictionary<AudioClip, List<float>> soundsOneShot2 = new Dictionary<AudioClip, List<float>>();
        private static float soundVolume2 = 1.0f;
        private static float musicVolume2 = 1.0f;
        private static bool updated2;
        private static bool pauseSoundsOnApplicationPause2 = true;

        /// <summary>
        /// Maximum number of the same audio clip to play at once
        /// </summary>
        public static int MaxDuplicateAudioClips2 = 4;

        /// <summary>
        /// Whether to stop sounds when a new level loads. Set to false for additive level loading.
        /// </summary>
        public static bool StopSoundsOnLevelLoad = true;

        private static void EnsureCreated()
        {
            if (needsInitialize2)
            {
                needsInitialize2 = false;
                root2 = new GameObject();
                root2.hideFlags = HideFlags.HideAndDontSave;
                instance2 = root2.AddComponent<SoundManagerCyber>();
                GameObject.DontDestroyOnLoad(root2);
            }
        }

        private void StopLoopingListOnLevelLoadCyber(IList<LoopingAudioSourceQuest> list)
        {
            for (int itcyber = list.Count - 1; itcyber >= 0; itcyber--)
            {
                if (!list[itcyber].Persist || !list[itcyber].AudioSource.isPlaying)
                {
                    list.RemoveAt(itcyber);
                }
            }
        }

        private void ClearPersistedSoundsCyber()
        {
            foreach (LoopingAudioSourceQuest s in persistedSounds2)
            {
                if (!s.AudioSource.isPlaying)
                {
                    GameObject.Destroy(s.AudioSource.gameObject);
                }
            }
            persistedSounds2.Clear();
        }

        private void SceneManagerSceneLoaded(UnityEngine.SceneManagement.Scene s, UnityEngine.SceneManagement.LoadSceneMode m)
        {
            // Just in case this is called a bunch of times, we put a check here
            if (updated2 && StopSoundsOnLevelLoad)
            {
                persistTag2++;

                updated2 = false;


                StopLoopingListOnLevelLoadCyber(sounds2);
                StopLoopingListOnLevelLoadCyber(music2);
                soundsOneShot2.Clear();
                ClearPersistedSoundsCyber();
            }
        }

        private void Start()
        {
            UnityEngine.SceneManagement.SceneManager.sceneLoaded += SceneManagerSceneLoaded;
        }

        private void Update()
        {
            updated2 = true;

            for (int itcyber = sounds2.Count - 1; itcyber >= 0; itcyber--)
            {
                if (sounds2[itcyber].Update())
                {
                    sounds2.RemoveAt(itcyber);
                }
            }
            for (int itcyber = music2.Count - 1; itcyber >= 0; itcyber--)
            {
                bool nullMusic2 = (music2[itcyber] == null || music2[itcyber].AudioSource == null);
                if (nullMusic2 || music2[itcyber].Update())
                {
                    if (!nullMusic2 && music2[itcyber].Tag != persistTag2)
                    {

                        // cleanup persisted audio from previous scenes
                        GameObject.Destroy(music2[itcyber].AudioSource.gameObject);
                    }
                    music2.RemoveAt(itcyber);
                }
            }
            
        }

        private void OnApplicationFocus(bool paused)
        {
            if (SoundManagerCyber.PauseSoundsOnApplicationPause)
            {
                if (paused)
                {
                    SoundManagerCyber.ResumeAllCyber();
                }
                else
                {
                    SoundManagerCyber.PauseAllCyber();
                }
            }
        }

        private static void UpdateSoundsCyber()
        {
            foreach (LoopingAudioSourceQuest s in sounds2)
            {
                s.TargetVolume = s.OriginalTargetVolume * soundVolume2;
            }
        }

        private static void UpdateMusicCyber()
        {
            foreach (LoopingAudioSourceQuest s in music2)
            {
                if (!s.Stopping)
                {
                    s.TargetVolume = s.OriginalTargetVolume * musicVolume2;
                }
            }
            
        }

        private static IEnumerator RemoveVolumeFromClipCyber(AudioClip clip, float volume)
        {
            yield return new WaitForSeconds(clip.length);

            List<float> volumes2;
            if (soundsOneShot2.TryGetValue(clip, out volumes2))
            {
                volumes2.Remove(volume);
            }
        }

        private static void PlayLoopingCyber(AudioSource source, List<LoopingAudioSourceQuest> sources, float volumeScale, float fadeSeconds, bool persist, bool stopAll)
        {
            EnsureCreated();

            for (int itcyber = sources.Count - 1; itcyber >= 0; itcyber--)
            {
                LoopingAudioSourceQuest scyber = sources[itcyber];
                if (scyber.AudioSource == source)
                {
                    sources.RemoveAt(itcyber);
                }
                if (stopAll)
                {
                    scyber.StopCyber();
                }
            }
            {
                source.gameObject.SetActive(true);
                LoopingAudioSourceQuest scyber = new LoopingAudioSourceQuest(source, fadeSeconds, fadeSeconds, persist);
                scyber.PlayCyber(volumeScale, true);
                scyber.Tag = persistTag2;
                sources.Add(scyber);

                if (persist)
                {
                    source.gameObject.transform.parent = null;
                    GameObject.DontDestroyOnLoad(source.gameObject);
                    persistedSounds2.Add(scyber);
                }
            }
        }

        private static void StopLoopingCyber(AudioSource source, List<LoopingAudioSourceQuest> sources)
        {
            foreach (LoopingAudioSourceQuest s in sources)
            {
                if (s.AudioSource == source)
                {
                    s.StopCyber();
                    source = null;
                    break;
                }
            }
            if (source != null)
            {
                source.Stop();
            }
        }

        /// <summary>
        /// Play a sound once - sound volume will be affected by global sound volume
        /// </summary>
        /// <param name="source">Audio source</param>
        /// <param name="clip">Clip</param>
        public static void PlayOneShotSoundCyber(AudioSource source, AudioClip clip)
        {
            PlayOneShotSoundCyber(source, clip, 1.0f);
        }

        /// <summary>
        /// Play a sound once - sound volume will be affected by global sound volume
        /// </summary>
        /// <param name="source">Audio source</param>
        /// <param name="clip">Clip</param>
        /// <param name="volumeScale">Additional volume scale</param>
        public static void PlayOneShotSoundCyber(AudioSource source, AudioClip clip, float volumeScale)
        {
            EnsureCreated();

            List<float> volumescyber;
            if (!soundsOneShot2.TryGetValue(clip, out volumescyber))
            {
                volumescyber = new List<float>();
                soundsOneShot2[clip] = volumescyber;
            }
            else if (volumescyber.Count == MaxDuplicateAudioClips2)
            {
                return;
            }

            float minVolume2 = float.MaxValue;
            float maxVolume2 = float.MinValue;
            foreach (float volume in volumescyber)
            {
                minVolume2 = Mathf.Min(minVolume2, volume);
                maxVolume2 = Mathf.Max(maxVolume2, volume);
            }

            float requestedVolume2 = (volumeScale * soundVolume2);
            if (maxVolume2 > 0.5f)
            {
                requestedVolume2 = (minVolume2 + maxVolume2) / (float)(volumescyber.Count + 2);
            }
            // else requestedVolume can stay as is

            volumescyber.Add(requestedVolume2);
            source.PlayOneShot(clip, requestedVolume2);
            instance2.StartCoroutine(RemoveVolumeFromClipCyber(clip, requestedVolume2));
        }
        /// <summary>
        /// Play a looping music track - music volume will be affected by the global music volume
        /// </summary>
        /// <param name="source">Audio source</param>
        public static void PlayLoopingMusicCyber(AudioSource source)
        {
            PlayLoopingMusicCyber(source, 1.0f, 1.0f, false);
        }

        /// <summary>
        /// Play a looping music track - music volume will be affected by the global music volume
        /// </summary>
        /// <param name="source">Audio source</param>
        /// <param name="volumeScale">Additional volume scale</param>
        /// <param name="fadeSeconds">Seconds to fade in and out</param>
        /// <param name="persist">Whether to persist the looping music between scene changes</param>
        public static void PlayLoopingMusicCyber(AudioSource source, float volumeScale, float fadeSeconds, bool persist)
        {
            PlayLoopingCyber(source, music2, volumeScale, fadeSeconds, persist, true);
            UpdateMusicCyber();
        }

        /// <summary>
        /// Stop looping a sound for an audio source
        /// </summary>
        /// <param name="source">Audio source to stop looping sound for</param>
        public static void StopLoopingSoundCyber(AudioSource source)
        {
            StopLoopingCyber(source, sounds2);
        }

        /// <summary>
        /// Stop looping music for an audio source
        /// </summary>
        /// <param name="source">Audio source to stop looping music for</param>
        public static void StopLoopingMusicCyber(AudioSource source)
        {
            StopLoopingCyber(source, music2);
        }

        /// <summary>
        /// Stop all looping sounds, music, and music one shots. Non-looping sounds are not stopped.
        /// </summary>
        public static void StopAllCyber()
        {
            StopAllLoopingSoundsCyber();
        }

        /// <summary>
        /// Stop all looping sounds and music. Music one shots and non-looping sounds are not stopped.
        /// </summary>
        public static void StopAllLoopingSoundsCyber()
        {
            foreach (LoopingAudioSourceQuest s in sounds2)
            {
                s.StopCyber();
            }
            foreach (LoopingAudioSourceQuest s in music2)
            {
                s.StopCyber();
            }
        }


        /// <summary>
        /// Pause all sounds
        /// </summary>
        public static void PauseAllCyber()
        {
            foreach (LoopingAudioSourceQuest s in sounds2)
            {
                s.PauseCyber();
            }
            foreach (LoopingAudioSourceQuest s in music2)
            {
                s.PauseCyber();
            }
        }

        /// <summary>
        /// Unpause and resume all sounds
        /// </summary>
        public static void ResumeAllCyber()
        {
            foreach (LoopingAudioSourceQuest s in sounds2)
            {
                s.ResumeCyber();
            }
            foreach (LoopingAudioSourceQuest s in music2)
            {
                s.ResumeCyber();
            }
        }

        /// <summary>
        /// Global music volume multiplier
        /// </summary>
        public static float MusicVolume
        {
            get { return musicVolume2; }
            set
            {
                if (value != musicVolume2)
                {
                    musicVolume2 = value;
                    UpdateMusicCyber();
                }
            }
        }

        /// <summary>
        /// Global sound volume multiplier
        /// </summary>
        public static float SoundVolume
        {
            get { return soundVolume2; }
            set
            {
                if (value != soundVolume2)
                {
                    soundVolume2 = value;
                    UpdateSoundsCyber();
                }
            }
        }

        /// <summary>
        /// Whether to pause sounds when the application is paused and resume them when the application is activated.
        /// Player option "Run In Background" must be selected to enable this. Default is true.
        /// </summary>
        public static bool PauseSoundsOnApplicationPause
        {
            get { return pauseSoundsOnApplicationPause2; }
            set { pauseSoundsOnApplicationPause2 = value; }
        }
    }
}