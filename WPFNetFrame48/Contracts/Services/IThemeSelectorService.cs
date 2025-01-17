using System;

using WPFNetFrame48.Models;

namespace WPFNetFrame48.Contracts.Services
{
    public interface IThemeSelectorService
    {
        bool SetTheme(AppTheme? theme = null);

        AppTheme GetCurrentTheme();
    }
}
