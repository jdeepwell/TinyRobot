using UnityEngine;

namespace Deepwell
{
    public static class DW_AudioSourceExtender
    {
        public static void InstantPlay(this AudioSource theSource, AudioClip newClip)
        {
            theSource.Stop();
            theSource.clip = newClip;
            theSource.Play();
        }
    }
}
