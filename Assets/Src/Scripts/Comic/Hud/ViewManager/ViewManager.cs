using System.Collections.Generic;
using UnityEngine;

public class ViewManager : MonoBehaviour {
    private static ViewManager m_instance;
    [SerializeField] private AView m_startingView;
    [SerializeField] private AView[] m_views;
    private AView m_currentView;
    private readonly Stack<AView> m_history = new Stack<AView>();
    private void Awake() => m_instance = this;

    public static T GetView<T>() where T : AView
    {
        for (int i = 0; i < m_instance.m_views.Length; i++) {
            if (m_instance.m_views[i] is T tView) {
                return tView;
            }
        }
        return null;
    }

    public static void Show<T>(bool remember = false) where T : AView
    {
        for (int i = 0; i < m_instance.m_views.Length; i++) {
            if (m_instance.m_views[i] is T) {
                if (m_instance.m_currentView != null) {
                    if (remember) {
                        m_instance.m_history.Push(m_instance.m_currentView);
                    }
                    m_instance.m_currentView.Hide();
                }
                m_instance.m_views[i].Show();
                m_instance.m_currentView = m_instance.m_views[i];
            }
        }
    }

    public static void Show(AView view, bool remember = false) {
        if (m_instance.m_currentView != null) {
            if (remember) {
                m_instance.m_history.Push(m_instance.m_currentView);
            }
            m_instance.m_currentView.Hide();
        }
        view.Show();
        m_instance.m_currentView = view;
    }

    public static void ShowLast() {
        if (m_instance.m_history.Count != 0) {
            Show(m_instance.m_history.Pop(), false);
        }
    }

    private void Start() {
        for (int i = 0; i < m_views.Length; i++) {
           m_views[i].Init();
            m_views[i].Hide();
        }

        if (m_startingView != null)
            Show(m_startingView, true);
    }
}