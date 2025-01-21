using System;
using System.Collections.Generic;

namespace Comic
{
    [System.Serializable]
    public class SettingDatas
    {
        public float m_musicVolume = 1f;
        public float m_effectVolume = 1f;
    }

    public class Setting
    {
        public SettingDatas                             m_settingDatas = null;
        private readonly SaveUtilitary<SettingDatas>    m_saveUtilitary;

        public Setting()
        {
            m_saveUtilitary = new SaveUtilitary<SettingDatas>("SettingDatas", FileType.SaveFile);

            m_settingDatas = m_saveUtilitary.Load();
        }
    }
}