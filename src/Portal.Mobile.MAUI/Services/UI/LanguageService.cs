﻿using Abp.Dependency;

namespace Portal.Mobile.MAUI.Services.UI
{
    public class LanguageService : ISingletonDependency
    {
        public event EventHandler OnLanguageChanged;

        public void ChangeLanguage()
        {
            OnLanguageChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
