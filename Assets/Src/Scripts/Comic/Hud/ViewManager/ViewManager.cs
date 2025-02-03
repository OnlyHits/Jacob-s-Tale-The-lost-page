using System.Collections.Generic;
using UnityEngine;
using CustomArchitecture;

namespace Comic
{
    public class ViewManager : BaseBehaviour
    {
        private static ViewManager m_instance;
        [SerializeField] private AView m_startingView;
        [SerializeField] private AView[] m_views;
        private AView m_currentView;
        private readonly Stack<AView> m_history = new Stack<AView>();

        public T GetView<T>() where T : AView
        {
            for (int i = 0; i < m_views.Length; i++)
            {
                if (m_views[i] is T tView)
                {
                    return tView;
                }
            }
            return null;
        }

        public void Show<T>(bool remember = false) where T : AView
        {
            for (int i = 0; i < m_views.Length; i++)
            {
                if (m_views[i] is T)
                {
                    if (m_currentView != null)
                    {
                        if (remember)
                        {
                            m_history.Push(m_currentView);
                        }
                        m_currentView.Hide();
                    }
                    m_views[i].Show();
                    m_currentView = m_views[i];
                }
            }
        }

        public void Show(AView view, bool remember = false)
        {
            if (m_currentView != null)
            {
                if (remember)
                {
                    m_history.Push(m_currentView);
                }
                m_currentView.Hide();
            }
            view.Show();
            m_currentView = view;
        }

        public void ShowLast()
        {
            if (m_history.Count != 0)
            {
                Show(m_history.Pop(), false);
            }
        }

        public override void Pause(bool pause)
        {
            base.Pause(pause);

            if (m_currentView != null)
                m_currentView.Pause(pause);
        }

        public void Init()
        {
            for (int i = 0; i < m_views.Length; i++)
            {
                m_views[i].Init();
                m_views[i].Hide();
            }

            if (m_startingView != null)
                Show(m_startingView, true);
        }
    }
}