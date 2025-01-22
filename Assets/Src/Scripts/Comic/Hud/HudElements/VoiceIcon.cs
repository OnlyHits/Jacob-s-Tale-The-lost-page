using CustomArchitecture;

namespace Comic
{
    public class VoiceIcon : BaseBehaviour
    {
        private VoiceType       m_type;

        public void Init(VoiceType type)
        {
            m_type = type;
        }

        public VoiceType Type
        {
            get { return m_type; }
            protected set {}
        }
    }
}