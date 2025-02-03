using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using ExtensionMethods;
using UnityEngine.InputSystem;

namespace CustomArchitecture
{
    public enum TMP_AnimatedText_State
    {
        State_Displaying,
        State_Idle,
        State_Uncompute,
    }

    [RequireComponent(typeof(TMP_Text))]
    public class TMP_AnimatedText : BaseBehaviour
    {
        private Coroutine                   m_dialogueCoroutine = null;

        protected TMP_Text                  m_textMeshPro = null;
        protected Mesh                      m_mesh = null;
        protected Vector3[]                 m_vertices = null;
        protected Color32[]                 m_colors = null;

        protected DialogueConfig            m_dialogueConfig;
        protected DynamicDialogueData       m_dynamicDatas;
        protected int                       m_sentenceIndex = 0;
        protected TMP_AnimatedText_State    m_state = TMP_AnimatedText_State.State_Uncompute;
        protected bool                      m_updateInCoroutine = false;
        private bool                        m_isInputPressed = false;

        public TMP_AnimatedText_State GetState() => m_state;
        public TMP_Text GetTextMeshPro() => m_textMeshPro;
        public void Validate(bool validate) => m_isInputPressed = validate;

        public void CopyText(TMP_Text text)
        {
            if (m_textMeshPro == null)
            {
                Debug.LogError("No TMP_Text component found on the GameObject.");
                return;
            }

            m_textMeshPro.text = text.text;

            text.ForceMeshUpdate();
            m_textMeshPro.ForceMeshUpdate();

            TMP_TextInfo sourceTextInfo = text.textInfo;
            TMP_TextInfo targetTextInfo = m_textMeshPro.textInfo;

            if (sourceTextInfo.characterCount != targetTextInfo.characterCount)
            {
                Debug.LogError("Character counts do not match between source and target TMP_Text objects.");
                return;
            }

            for (int i = 0; i < sourceTextInfo.meshInfo.Length; i++)
            {
                TMP_MeshInfo sourceMeshInfo = sourceTextInfo.meshInfo[i];
                TMP_MeshInfo targetMeshInfo = targetTextInfo.meshInfo[i];

                if (targetMeshInfo.vertices.Length != sourceMeshInfo.vertices.Length)
                {
                    targetTextInfo.meshInfo[i].ResizeMeshInfo(sourceMeshInfo.vertices.Length / 4);
                }

                System.Array.Copy(sourceMeshInfo.vertices, targetMeshInfo.vertices, sourceMeshInfo.vertices.Length);
                System.Array.Copy(sourceMeshInfo.colors32, targetMeshInfo.colors32, sourceMeshInfo.colors32.Length);
                System.Array.Copy(sourceMeshInfo.uvs0, targetMeshInfo.uvs0, sourceMeshInfo.uvs0.Length);
                System.Array.Copy(sourceMeshInfo.normals, targetMeshInfo.normals, sourceMeshInfo.normals.Length);
                System.Array.Copy(sourceMeshInfo.tangents, targetMeshInfo.tangents, sourceMeshInfo.tangents.Length);
            }

            for (int i = 0; i < targetTextInfo.meshInfo.Length; i++)
            {
                m_textMeshPro.UpdateGeometry(targetTextInfo.meshInfo[i].mesh, i);
            }
            Debug.Log(targetTextInfo.meshInfo.Length);
        }



        //     Debug.Log(text.text);

        //     m_textMeshPro = gameObject.GetComponent<TMP_Text>();
        //     Debug.Log(m_textMeshPro == null);
            
        //     m_textMeshPro.text = text.text;
        //     m_textMeshPro.ForceMeshUpdate();

        //     m_mesh = m_textMeshPro.mesh;
        //     m_vertices = m_mesh.vertices;
        //     m_colors = m_mesh.colors32;

        //     foreach (var info in text.textInfo.characterInfo)
        //     {
        //         int vertexIndex = info.vertexIndex;

        //         if (info.character == ' '|| info.character == '\n')
        //             continue;

        //         m_vertices[vertexIndex] = info.bottomLeft;
        //         m_vertices[vertexIndex + 1] = info.topLeft;
        //         m_vertices[vertexIndex + 2] = info.topRight;
        //         m_vertices[vertexIndex + 3] = info.bottomRight;
        //     }
                
        //     m_mesh.colors32 = m_colors;
        //     m_mesh.vertices = m_vertices;
        // }

        // protected void Update()
        // {
        //     if (m_textMeshPro.enabled && !m_isTmpEnabled)
        //     {
        //     m_textMeshPro.ForceMeshUpdate();
        //     }
            
        //     m_isTmpEnabled = m_textMeshPro.enabled;

        //     if (m_pause)
        //         return;

        //     OnUpdate(Time.deltaTime);
        // }

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

            m_state = TMP_AnimatedText_State.State_Displaying;
            m_dialogueCoroutine = StartCoroutine(DialogueCoroutine());
        }

        public void StopDialogue()
        {
            m_state = TMP_AnimatedText_State.State_Uncompute;
            m_updateInCoroutine = false;
            
            if (m_dialogueCoroutine != null)
            {
                StopCoroutine(m_dialogueCoroutine);
                m_dialogueCoroutine = null;
            }
        }

        public override void Pause(bool pause)
        {
            base.Pause(pause);
        }

        protected void SetDialogue()
        {
            m_textMeshPro.text = m_dynamicDatas.m_sentenceData[m_sentenceIndex].m_fullText;
        }

        IEnumerator WaitForEither()
        {
            bool conditionMet = false;

            // Start both coroutines in parallel
            StartCoroutine(WaitUntilInputPressed(() => conditionMet = true));
            StartCoroutine(WaitForSecondsCoroutine(() => conditionMet = true));

            yield return new WaitUntil(() => conditionMet);
        }

        IEnumerator WaitUntilInputPressed(System.Action onComplete)
        {
            yield return new WaitWhile(() => !m_isInputPressed);
            onComplete?.Invoke();
        }

        IEnumerator WaitForSecondsCoroutine(System.Action onComplete)
        {
            yield return new WaitForSeconds(m_dialogueConfig.m_durationBetweenSentence);
            onComplete?.Invoke();
        }

        protected IEnumerator DialogueCoroutine()
        {
            while (m_sentenceIndex < m_dialogueConfig.m_dialogueSentences.Length)
            {
                SetDialogue();

                m_updateInCoroutine = true;
                yield return StartCoroutine(ApparitionCoroutine());
                m_updateInCoroutine = false;
                
                yield return StartCoroutine(WaitForEither());

                yield return new WaitWhile(() => m_pause);

                ++m_sentenceIndex;
            }

            m_sentenceIndex = Mathf.Clamp(m_sentenceIndex, 0, m_dialogueConfig.m_dialogueSentences.Length - 1);
            m_state = TMP_AnimatedText_State.State_Idle;
        }

        private IEnumerator ApparitionCoroutine()
        {
            if (m_dialogueConfig.m_dialogueSentences[m_sentenceIndex].m_apparitionType == DialogueApparitionType.INCREMENTAL)
            {
                yield return StartCoroutine(IncrementalApparition());
            }
            else if (m_dialogueConfig.m_dialogueSentences[m_sentenceIndex].m_apparitionType == DialogueApparitionType.SIMULTANEOUS)
            {
                SetVertexAtCenter();
                yield return StartCoroutine(SimultaneousApparition());
            }
        }

        protected void SetVertexAtCenter()
        {
            m_textMeshPro.ForceMeshUpdate();

            m_mesh = m_textMeshPro.mesh;
            m_vertices = m_mesh.vertices;
            m_colors = m_mesh.colors32;

            foreach (var data in m_dynamicDatas.m_sentenceData[m_sentenceIndex].m_animatedTextDatas)
            {
                for (int i = data.m_firstIndex; i < data.m_lastIndex; ++i)
                {
                    TMP_CharacterInfo info = m_textMeshPro.textInfo.characterInfo[i];

                    int vertexIndex = info.vertexIndex;

                    if (info.character == ' '|| info.character == '\n')
                        continue;

                    Vector3 center = new Vector3(info.topLeft.x + (info.topRight.x - info.topLeft.x) * 0.5f, info.bottomLeft.y + (info.topRight.y - info.bottomRight.y) * 0.5f, 1);

                    m_vertices[vertexIndex] = center;
                    m_vertices[vertexIndex + 1] = center;
                    m_vertices[vertexIndex + 2] = center;
                    m_vertices[vertexIndex + 3] = center;
                }
            }
                
            m_mesh.colors32 = m_colors;
            m_mesh.vertices = m_vertices;
        }

        protected override void OnUpdate(float elapsed_time)
        {
            base.OnUpdate(elapsed_time);

            if (m_state == TMP_AnimatedText_State.State_Uncompute || m_updateInCoroutine)
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

                    UpdateTextModifier(vertexIndex,
                        m_dialogueConfig.m_dialogueSentences[m_sentenceIndex].m_animatedText[index], i);
                    UpdateColor(vertexIndex, 
                        m_dialogueConfig.m_dialogueSentences[m_sentenceIndex].m_animatedText[index],
                        d.m_colorRandomSpeed, j);

                    ++j;
                }

                ++index;
            }

            m_mesh.vertices = m_vertices;
            m_mesh.colors32 = m_colors;
        }

        #region Apparition Coroutines

        protected IEnumerator SimultaneousApparition()
        {
            bool finish = false;
            float time = 0f;

            while (!finish)
            {
                m_textMeshPro.ForceMeshUpdate();

                m_mesh = m_textMeshPro.mesh;
                m_vertices = m_mesh.vertices;
                m_colors = m_mesh.colors32;

                int index = 0;
                finish = true;

                foreach (var data in m_dynamicDatas.m_sentenceData[m_sentenceIndex].m_animatedTextDatas)
                {
                    for (int i = data.m_firstIndex, j = 0; i < data.m_lastIndex; ++i)
                    {
                        TMP_CharacterInfo info = m_textMeshPro.textInfo.characterInfo[i];
                        int vertexIndex = info.vertexIndex;

                        if (info.character == ' '|| info.character == '\n')
                            continue;

                        if (!PopCharacter(vertexIndex, time, info))
                            finish = false;

                        UpdateColor(vertexIndex,
                            m_dialogueConfig.m_dialogueSentences[m_sentenceIndex].m_animatedText[index],
                            data.m_colorRandomSpeed, j);
                        ++j;
                    }
                    
                    ++index;
                }

                time += Time.deltaTime;
                m_mesh.colors32 = m_colors;
                m_mesh.vertices = m_vertices;

                yield return new WaitWhile(() => m_pause);
            }
        }

        protected IEnumerator IncrementalApparition()
        {
            yield return null;
            // float time = 0.0f;
            // bool updateI = false;

            // for (int i = 0; i < m_textMeshPro.text.Length; updateI = false)
            // {
            //     m_textMeshPro.ForceMeshUpdate();

            //     m_mesh = m_textMeshPro.mesh;
            //     m_vertices = m_mesh.vertices;
            //     m_colors = m_mesh.colors32;

            //     for (int s = 0; s < m_dynamicDatas.m_sentenceData[m_sentenceIndex].m_animatedTextDatas.Length; ++s)
            //     {
            //         for (int inc = m_dynamicDatas.m_sentenceData[m_sentenceIndex].m_animatedTextDatas[s].m_firstIndex, j = 0;
            //             inc < m_dynamicDatas.m_sentenceData[m_sentenceIndex].m_animatedTextDatas[s].m_lastIndex; ++inc)
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
            //                 if (Time.deltaTime < .1f)
            //                 {
            //                     time += Time.deltaTime / .1f;

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
            //                 UpdateTextModifier(vertexIndex, m_dialogueConfig.m_dialogueSentences[m_sentenceIndex].m_animatedText[s], inc);
            //                 UpdateColor(vertexIndex,
            //                     m_dialogueConfig.m_dialogueSentences[m_sentenceIndex].m_animatedText[s],
            //                     m_dynamicDatas.m_sentenceData[m_sentenceIndex].m_animatedTextDatas[s].m_colorRandomSpeed, j);
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
    
        #endregion

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

            float ease = Easing.EaseInSine(Mathf.Clamp(time / TMP_AnimatedTextController.Instance.m_simultaneousApparitionDuration, 0f, 1f));

            m_vertices[vertexIndex] = Vector3.Lerp(center, info.bottomLeft, ease);
            m_vertices[vertexIndex + 1] = Vector3.Lerp(center, info.topLeft, ease);
            m_vertices[vertexIndex + 2] = Vector3.Lerp(center, info.topRight, ease);
            m_vertices[vertexIndex + 3] = Vector3.Lerp(center, info.bottomRight, ease);

            return ease == 1f;

            // if (m_vertices[vertexIndex] == info.bottomLeft && m_vertices[vertexIndex + 1] == info.topLeft
            //     && m_vertices[vertexIndex + 2] == info.topRight && m_vertices[vertexIndex + 3] == info.bottomRight)
            // {
            //     return true;
            // }
            // return false;
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