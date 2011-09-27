namespace Freetime.Base.Component
{
    public class LanguageManager : ILanguageManager
    {

        #region Instance
        private static ILanguageManager s_instance;

        public static ILanguageManager Current
        {
            get 
            { 
                s_instance = s_instance ?? new LanguageManager();
                return s_instance;
            }
        }

        public static void SetLanguageManager(ILanguageManager manager)
        {
            s_instance = manager;
        }
        #endregion
    }
}
