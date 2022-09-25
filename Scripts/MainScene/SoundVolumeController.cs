using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HamuGame;

namespace HamuGame
{
    //�V�[�����ړ�����ۂɉ��ʂ̐ݒ�������p���p
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
