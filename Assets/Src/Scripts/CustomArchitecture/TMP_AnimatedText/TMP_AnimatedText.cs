using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using ExtensionMethods;

namespace CustomArchitecture
{
    [RequireComponent(typeof(TMP_Text))]
    public class TMP_AnimatedText : BaseBehaviour
    {
        private Coroutine               m_dialogueCoroutine = null;

        protected TMP_Text              m_textMeshPro = null;
        protected Mesh                  m_mesh = null;
        protected Vector3[]             m_vertices = null;
        protected Color32[]             m_colors = null;

        protected DialogueConfig        m_dialogueConfig;
        protected DynamicDialogueData   m_dynamicDatas;
        protected int                   m_sentenceIndex = 0;
        protected bool                  m_isCompute = false;

        private void Awake()
        {
            m_textMeshPro = gameObject.GetComponent<TMP_Text>();
            
            if (m_textMeshPro == null)
                Debug.LogWarning("No TextMeshPro found");
        }

        public void StartDialogue(DialogueConfig config, DynamicDialogueData datas)
        {
            m_dialogueConfig = config;
            m_dynamicDatas = datas;

            m_sentenceIndex = 0;

            SetDialogue();
            m_isCompute = true;

//            m_dialogueCoroutine = StartCoroutine()
        }

        public void StopDialogue()
        {
            m_isCompute = false;
        }

        protected IEnumerator DialogueCoroutine()
        {
            if (m_dialogueConfig.m_handleByInput)
            {
                
            }
            else
            {

                while (m_sentenceIndex < m_dialogueConfig.m_dialogueSentences.Length)
                {
                    yield return StartCoroutine(ApparitionCoroutine());
                    yield return new WaitForSeconds(m_dialogueConfig.m_durationBetweenSentence);
                }
            }
        }

        private IEnumerator ApparitionCoroutine()
        {
            yield return null;
            // if (m_dialogueConfig.m_dialogueSentences[m_sentenceIndex].m_apparitionType == DialogueApparitionType.INCREMENTAL)
            // {

            // }
            // else if (m_dialogueConfig.m_dialogueSentences[m_sentenceIndex].m_apparitionType == DialogueApparitionType.SIMULTANEOUS)
            // {

            // }
        }

        protected void SetDialogue()
        {
            m_textMeshPro.text = m_dynamicDatas.m_sentenceData[m_sentenceIndex].m_fullText;
        }

        protected override void OnUpdate(float elapsed_time)
        {
            // if (_handleByInput)
            //     UpdateInput();

            if (!m_isCompute)
                return;

            m_textMeshPro.ForceMeshUpdate();
            m_mesh = m_textMeshPro.mesh;
            m_vertices = m_mesh.vertices;
            m_colors = m_mesh.colors32;

            int index = 0;
            foreach (var d in m_dynamicDatas.m_sentenceData[m_sentenceIndex].m_animatedTextDatas)
            {
                for (int i = d.m_firstIndex, j = 0; i < d.m_lastIndex; ++i)
                {
                    TMP_CharacterInfo info = m_textMeshPro.textInfo.characterInfo[i];

                    if (info.character == ' ' || info.character == '\n')
                        continue;

                    int vertexIndex = info.vertexIndex;

                    UpdateTextModifier(vertexIndex, m_dialogueConfig.m_dialogueSentences[m_sentenceIndex].m_animatedText[index], i);
                    UpdateColor(vertexIndex, m_dialogueConfig.m_dialogueSentences[m_sentenceIndex].m_animatedText[index], d.m_colorRandomSpeed, j);

                    ++j;
                }

                ++index;
            }

            m_mesh.vertices = m_vertices;
            m_mesh.colors32 = m_colors;
        }

        protected IEnumerator SimultaneousApparition()
        {
            yield return null;
        }

        protected IEnumerator IncrementalApparition()
        {
            float time = 0.0f;
            bool updateI = false;
            yield return null;
            // for (int i = 0; i < m_textMeshPro.text.Length; updateI = false)
            // {
            //     m_textMeshPro.ForceMeshUpdate();

            //     m_mesh = m_textMeshPro.mesh;
            //     m_vertices = m_mesh.vertices;
            //     m_colors = m_mesh.colors32;

            //     foreach (var d in _dialogues[_dialogueIndex]._partOfSide[_partOfSideIndex]._modifiableText)
            //     {
            //         for (int inc = d._firstIndex, j = 0; inc < d._lastIndex; ++inc)
            //         {
            //             TMP_CharacterInfo info = m_textMeshPro.textInfo.characterInfo[inc];

            //             int vertexIndex = info.vertexIndex;

            //             if (info.character == ' '|| info.character == '\n')
            //             {
            //                 if (inc == i)
            //                 {
            //                     i++;
            //                     updateI = true;
            //                     time = 0.0f;
            //                 }
            //                 continue;
            //             }

            //             if (inc == i)
            //             {
            //                 if (Time.deltaTime < _currentPopSpeed)
            //                 {
            //                     time += Time.deltaTime / _currentPopSpeed;

            //                     if ((updateI = PopCharacter(vertexIndex, time, info)))
            //                     {
            //                         i++;
            //                         time = 0.0f;
                                    
            //                     }                            
            //                 }
            //                 else
            //                     updateI = true;
            //             }

            //             if (inc < i || (inc == i && !updateI))
            //             {
            //                 UpdateTextModifier(vertexIndex, d, inc);
            //                 UpdateColor(vertexIndex, d, j);
            //             }
            //             else if ((inc == i && updateI) || inc > i)
            //             {
            //                 Vector3 center = new Vector3(info.topLeft.x + (info.topRight.x - info.topLeft.x) * 0.5f, info.bottomLeft.y + (info.topRight.y - info.bottomRight.y) * 0.5f, 1);

            //                 m_vertices[vertexIndex] = center;
            //                 m_vertices[vertexIndex + 1] = center;
            //                 m_vertices[vertexIndex + 2] = center;
            //                 m_vertices[vertexIndex + 3] = center;
            //             }
            //             ++j;
            //         }
            //     }
            //     m_mesh.colors32 = m_colors;
            //     m_mesh.vertices = m_vertices;

            //     yield return null;
            // }
        }

        // private void UpdateInput()
        // {
        //     if (Input.GetKeyDown(KeyCode.Return))
        //     {
        //         if (_popCoroutine == null)
        //         {
        //             if (_partOfSideIndex + 1 >= _dialogues[_dialogueIndex]._partOfSide.Length)
        //             {
        //                 StopDialogue();
        //             }
        //             else
        //             {
        //                 _canSkip = false;
        //                 if (_popDuration == 0.0f)
        //                 {
        //                     ChangeText(_partOfSideIndex + 1);
        //                     _isCompute = true;
        //                 }
        //                 else
        //                     _popCoroutine = StartCoroutine(PopDialogue(_partOfSideIndex + 1));
        //             }
        //         }
        //     }
        //     else if (Input.GetKey(KeyCode.Return) && _canSkip)
        //     {
        //         _currentPopSpeed = _popDuration * 0.3f;
        //     }

        //     if (Input.GetKeyUp(KeyCode.Return))
        //     {
        //         if (!_canSkip)
        //             _canSkip = true;
        //         _currentPopSpeed = _popDuration;
        //     }
        // }

        #region VertexModifier

        protected void UpdateTextModifier(int vertexIndex, AnimatedTextConfig d, int currentIndex)
        {
            if (d.m_textModifier == TextModifier.WODDLE)
            {
                Vector3 offset = Wobble(Time.time + currentIndex, TMP_AnimatedTextController.Instance.m_woodleSpeed, TMP_AnimatedTextController.Instance.m_woodleAmplitude);
                m_vertices[vertexIndex] += offset;
                m_vertices[vertexIndex + 1] += offset;
                m_vertices[vertexIndex + 2] += offset;
                m_vertices[vertexIndex + 3] += offset;
            }
            else if (d.m_textModifier == TextModifier.WAVE)
            {
                float offset = CosWave(Time.time + currentIndex);
                m_vertices[vertexIndex].y += offset;
                m_vertices[vertexIndex + 1].y += offset;
                m_vertices[vertexIndex + 2].y += offset;
                m_vertices[vertexIndex + 3].y += offset;
            }
            else if (d.m_textModifier == TextModifier.RAND_BOUNCE)
            {
                Vector3 offset = Wobble(Time.time + currentIndex, TMP_AnimatedTextController.Instance.m_bounceSpeed, TMP_AnimatedTextController.Instance.m_bounceAmplitude);
                m_vertices[vertexIndex].x -= offset.x;
                m_vertices[vertexIndex].y -= offset.y;
                m_vertices[vertexIndex + 1].x -= offset.x;
                m_vertices[vertexIndex + 1].y += offset.y;
                m_vertices[vertexIndex + 2].x += offset.x;
                m_vertices[vertexIndex + 2].y += offset.y;
                m_vertices[vertexIndex + 3].x += offset.x;
                m_vertices[vertexIndex + 3].y -= offset.y;
            }
            else if (d.m_textModifier == TextModifier.BOUNCE)
            {
                Vector3 offset = Wobble(Time.time, TMP_AnimatedTextController.Instance.m_bounceSpeed, TMP_AnimatedTextController.Instance.m_bounceAmplitude);
                m_vertices[vertexIndex].x -= offset.x;
                m_vertices[vertexIndex].y -= offset.y;
                m_vertices[vertexIndex + 1].x -= offset.x;
                m_vertices[vertexIndex + 1].y += offset.y;
                m_vertices[vertexIndex + 2].x += offset.x;
                m_vertices[vertexIndex + 2].y += offset.y;
                m_vertices[vertexIndex + 3].x += offset.x;
                m_vertices[vertexIndex + 3].y -= offset.y;
            }
        }

        protected void UpdateColor(int vertexIndex, AnimatedTextConfig d, List<float> randomSpeed, int colorIndex)
        {
            if (d.m_colorModifier == TextColor.SOLID)
            {
                m_colors[vertexIndex] = d.m_solidColor;
                m_colors[vertexIndex + 1] = d.m_solidColor;
                m_colors[vertexIndex + 2] = d.m_solidColor;
                m_colors[vertexIndex + 3] = d.m_solidColor;
            }
            else if (d.m_colorModifier == TextColor.HORIZONTAL_GRADIENT || d.m_colorModifier == TextColor.VERTICAL_GRADIENT)
            {
                ColorGradient(vertexIndex, d, randomSpeed, colorIndex);
            }
        }

        private void ColorGradient(int vertexIndex, AnimatedTextConfig d, List<float> randomSpeed, int colorIndex)
        {
            if (d.m_colorModifier == TextColor.SOLID)
                return;
            m_colors[vertexIndex] = Color32.Lerp(d.m_solidColor, d.m_gradientColor, Mathf.PingPong(Time.time * randomSpeed[colorIndex] - (d.m_colorModifier == TextColor.HORIZONTAL_GRADIENT ? m_vertices[vertexIndex].x : m_vertices[vertexIndex].y) * 0.01f, 1.0f));
            m_colors[vertexIndex + 1] = Color32.Lerp(d.m_solidColor, d.m_gradientColor, Mathf.PingPong(Time.time * randomSpeed[colorIndex] - (d.m_colorModifier == TextColor.HORIZONTAL_GRADIENT ? m_vertices[vertexIndex + 1].x : m_vertices[vertexIndex + 1].y)* 0.01f, 1.0f));
            m_colors[vertexIndex + 2] = Color32.Lerp(d.m_solidColor, d.m_gradientColor, Mathf.PingPong(Time.time * randomSpeed[colorIndex] - (d.m_colorModifier == TextColor.HORIZONTAL_GRADIENT ? m_vertices[vertexIndex + 2].x : m_vertices[vertexIndex + 2].y)* 0.01f, 1.0f));
            m_colors[vertexIndex + 3] = Color32.Lerp(d.m_solidColor, d.m_gradientColor, Mathf.PingPong(Time.time * randomSpeed[colorIndex] - (d.m_colorModifier == TextColor.HORIZONTAL_GRADIENT ? m_vertices[vertexIndex + 3].x : m_vertices[vertexIndex + 3].y)* 0.01f, 1.0f));
        }

        private bool PopCharacter(int vertexIndex, float time, TMP_CharacterInfo info)
        {
            Vector3 center = new Vector3(info.topLeft.x + (info.topRight.x - info.topLeft.x) * 0.5f, info.bottomLeft.y + (info.topRight.y - info.bottomRight.y) * 0.5f, 1);

            m_vertices[vertexIndex] = Vector3.Lerp(center, info.bottomLeft, time);
            m_vertices[vertexIndex + 1] = Vector3.Lerp(center, info.topLeft, time);
            m_vertices[vertexIndex + 2] = Vector3.Lerp(center, info.topRight, time);
            m_vertices[vertexIndex + 3] = Vector3.Lerp(center, info.bottomRight, time);
                
            if (m_vertices[vertexIndex] == info.bottomLeft && m_vertices[vertexIndex + 1] == info.topLeft
                && m_vertices[vertexIndex + 2] == info.topRight && m_vertices[vertexIndex + 3] == info.bottomRight)
            {
                return true;
            }
            return false;
        }

        #endregion

        #region Utils

        private float CosWave(float time)
        {
            return Mathf.Cos(time * TMP_AnimatedTextController.Instance.m_waveSpeed) * TMP_AnimatedTextController.Instance.m_waveAmplitude;
        }

        private Vector2 Wobble(float time, Vector2 speed, Vector2 amplitude)
        {
            return new Vector2(Mathf.Sin(time * speed.x) * amplitude.x, Mathf.Cos(time * speed.y) * amplitude.y);
        }

        #endregion
    }
}