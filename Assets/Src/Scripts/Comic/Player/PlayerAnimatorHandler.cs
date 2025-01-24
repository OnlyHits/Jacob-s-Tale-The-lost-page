using CustomArchitecture;
using UnityEngine.InputSystem;
using UnityEngine;
using System;
using System.Collections.Generic;

namespace Comic
{
    public partial class Player : Character
    {
        private readonly string ANIM_IDLE = "Idle";
        private readonly string ANIM_RUN = "Run";
        private readonly string ANIM_JUMP = "Jump";
        private readonly string ANIM_FALL = "Fall";

        private void PlayRun(bool play = true)
        {
            m_animator.SetBool(ANIM_RUN, play);
        }

        private void PlayJump(bool play = true)
        {
            m_animator.SetTrigger(ANIM_JUMP);
        }

        private void PlayFall(bool play = true)
        {
            m_animator.SetBool(ANIM_FALL, play);
        }

        private void TryResetIdle()
        {
            m_animator.SetTrigger(ANIM_IDLE);
        }
    }
}
