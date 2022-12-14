using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HamuGame;

namespace HamuGame
{
    //シーンが移動する際に音量の設定を引き継ぐ用
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
