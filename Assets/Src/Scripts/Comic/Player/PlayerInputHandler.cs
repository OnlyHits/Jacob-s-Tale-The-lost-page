using CustomArchitecture;
using UnityEngine.InputSystem;
using UnityEngine;
using System;
using System.Collections.Generic;

namespace Comic
{
    public partial class Player : Character
    {
        private void OnMove(InputType input, Vector2 v)
        {
            if (input == InputType.PRESSED)
            {
                StartMove(v);
            }
            else if (input == InputType.COMPUTED)
            {
                Move(v);
            }
            else if (input == InputType.RELEASED)
            {
                StopMove(v);
            }
        }
        private void OnLook(InputType input, Vector2 v)
        {
            if (input == InputType.PRESSED)
            {
            }
            else if (input == InputType.COMPUTED)
            {
            }
            else if (input == InputType.RELEASED)
            {
            }
        }

        private void OnJump(InputType input, bool b)
        {
            if (input == InputType.PRESSED)
            {
                TryJump();
            }
            else if (input == InputType.COMPUTED)
            {
            }
            else if (input == InputType.RELEASED)
            {
            }
        }
        private void OnSprint(InputType input, bool b)
        {
            if (input == InputType.PRESSED)
            {
            }
            else if (input == InputType.COMPUTED)
            {
            }
            else if (input == InputType.RELEASED)
            {
            }
        }

        private void OnInteract(InputType input, bool b)
        {
            if (input == InputType.PRESSED)
            {
            }
            else if (input == InputType.COMPUTED)
            {
            }
            else if (input == InputType.RELEASED)
            {
            }
        }

        private void OnPower(InputType input, bool b)
        {
            if (input == InputType.PRESSED)
            {
                PowerAction(true);
            }
            else if (input == InputType.COMPUTED)
            {
            }
            else if (input == InputType.RELEASED)
            {
                PowerAction(false);
            }
        }

        private void OnNextPower(InputType input, bool b)
        {
            if (input == InputType.PRESSED)
            {
                SelectNextPower();
            }
            else if (input == InputType.COMPUTED)
            {
            }
            else if (input == InputType.RELEASED)
            {
            }
        }

        private void OnPrevPower(InputType input, bool b)
        {
            if (input == InputType.PRESSED)
            {
                SelectPrevPower();
            }
            else if (input == InputType.COMPUTED)
            {
            }
            else if (input == InputType.RELEASED)
            {
            }
        }

        private void OnNextPage(InputType input, bool b)
        {
            if (input == InputType.PRESSED)
            {
                m_pageManager?.TryNextPage();
            }
            else if (input == InputType.COMPUTED)
            {
            }
            else if (input == InputType.RELEASED)
            {
            }
        }

        private void OnPrevPage(InputType input, bool b)
        {
            if (input == InputType.PRESSED)
            {
                m_pageManager?.TryPrevPage();
            }
            else if (input == InputType.COMPUTED)
            {
            }
            else if (input == InputType.RELEASED)
            {
            }
        }
    }
}
