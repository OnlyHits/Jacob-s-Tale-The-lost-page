using CustomArchitecture;
using UnityEngine.InputSystem;
using UnityEngine;
using System;
using System.Collections.Generic;

namespace Comic
{
    public partial class Player : BaseBehaviour
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
    }
}
