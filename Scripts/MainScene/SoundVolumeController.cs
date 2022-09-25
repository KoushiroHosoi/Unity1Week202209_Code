using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HamuGame;

namespace HamuGame
{
    //ƒV[ƒ“‚ªˆÚ“®‚·‚éÛ‚É‰¹—Ê‚ÌÝ’è‚ðˆø‚«Œp‚®—p
    public static class SoundVolumeController
    {
        private static float bgmVolume;
        private static float seVolume;

        public static float BgmVolume { get => bgmVolume; }
        public static float SeVolume { get => seVolume; }

        public static void SetVolume(float bgm,float se)
        {
            bgmVolume = bgm;
            seVolume = se;
        }
    }
}
