namespace TDGachaGame
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        ///
        ///  Screen flow:
        ///    Form2 (title)  ->  Form3 (home)  ->  Form1 (the tower)  ->  back to Form3
        ///  Each screen is a normal Form shown with ShowDialog(), so the next line only runs
        ///  once the player has closed that screen.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            PlayerProfile profile = SaveManager.LoadOrCreate();
            profile.Changed += () => SaveManager.Save(profile);
            MusicPlayer.Start();

            if (!ShowTitleScreen())
            {
                SaveManager.Save(profile);
                return;
            }

            bool running = true;

            while (running)
            {
                using (var menu = new Form3(profile))
                {
                    menu.ShowDialog();

                    switch (menu.Choice)
                    {
                        case MenuChoice.EnterTower:
                            using (var tower = new Form1(profile))
                            {
                                tower.ShowDialog();
                            }
                            break;

                        case MenuChoice.BackToTitle:
                            running = ShowTitleScreen();
                            break;

                        default:
                            running = false;
                            break;
                    }
                }
            }
            SaveManager.Save(profile);

            MusicPlayer.Stop();
        }
        private static bool ShowTitleScreen()
        {
            using (var start = new Form2())
            {
                return start.ShowDialog() == DialogResult.OK;
            }
        }
    }
}