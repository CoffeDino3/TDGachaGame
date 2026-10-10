using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace TDGachaGame
{
    public static class MusicPlayer
    {
        private const double MasterVolume = 0.08;
        private const double CrossfadeSeconds = 3.0;

        private const int TickMs = 100;

        private static readonly string[] Extensions = { ".mp3", ".wav", ".wma", ".m4a" };
        private static readonly Random rng = new Random();

        private static System.Windows.Media.MediaPlayer current;
        private static System.Windows.Media.MediaPlayer incoming;
        private static System.Windows.Forms.Timer timer;

        private static List<string> playlist = new List<string>();
        private static int index = -1;
        private static int failedInARow;
        private static bool started;
        private static bool fading;
        private static double fadeElapsed;
        public static bool Muted
        {
            get;
            private set;
        }

        public static string MusicFolder => Path.Combine(AppContext.BaseDirectory, "music");

        public static void Start()
        {
            if (started)
            {
                return;
            }

            started = true;

            string folder = MusicFolder;
            if (!Directory.Exists(folder))
            {
                return;
            }

            playlist = Directory
                .GetFiles(folder, "*.*", SearchOption.AllDirectories)
                .Where(file => Extensions.Contains(Path.GetExtension(file), StringComparer.OrdinalIgnoreCase))
                .ToList();

            if (playlist.Count == 0)
            {
                return;
            }

            Shuffle();

            current = CreatePlayer();
            OpenNextTrack(current, MasterVolume);

            timer = new System.Windows.Forms.Timer { Interval = TickMs };
            timer.Tick += OnTick;
            timer.Start();
        }

        public static void ToggleMute()
        {
            Muted = !Muted;

            if (current == null)
            {
                return;
            }

            if (Muted)
            {
                current.Pause();
                incoming?.Pause();
            }
            else
            {
                current.Play();
                incoming?.Play();
            }
        }

        public static void Stop()
        {
            if (timer != null)
            {
                timer.Stop();
                timer.Dispose();
                timer = null;
            }

            current?.Close();
            incoming?.Close();
            current = null;
            incoming = null;
            fading = false;
        }
        private static void OnTick(object sender, EventArgs e)
        {
            if (current == null || Muted)
            {
                return;
            }

            if (fading)
            {
                fadeElapsed += TickMs / 1000.0;
                double t = Math.Min(1.0, fadeElapsed / CrossfadeSeconds);

                current.Volume = MasterVolume * (1.0 - t);
                incoming.Volume = MasterVolume * t;

                if (t >= 1.0)
                {
                    FinishFade();
                }

                return;
            }

            var duration = current.NaturalDuration;
            if (!duration.HasTimeSpan)
            {
                return;
            }

            double total = duration.TimeSpan.TotalSeconds;
            if (total < CrossfadeSeconds * 2.0)
            {
                return;
            }

            double remaining = total - current.Position.TotalSeconds;
            if (remaining <= CrossfadeSeconds)
            {
                StartFade();
            }
        }

        private static void StartFade()
        {
            incoming = CreatePlayer();
            OpenNextTrack(incoming, 0.0);
            fading = true;
            fadeElapsed = 0.0;
        }

        private static void FinishFade()
        {
            var old = current;
            current = incoming;
            incoming = null;
            fading = false;

            current.Volume = MasterVolume;
            old.Close();
        }
        private static void SwitchNow()
        {
            var old = current;
            current = CreatePlayer();
            OpenNextTrack(current, MasterVolume);
            old.Close();
        }

        private static System.Windows.Media.MediaPlayer CreatePlayer()
        {
            var p = new System.Windows.Media.MediaPlayer();
            p.MediaOpened += (sender, e) => failedInARow = 0;
            p.MediaEnded += (sender, e) => OnTrackEnded(p);
            p.MediaFailed += (sender, e) => OnTrackFailed(p);
            return p;
        }

        private static void OpenNextTrack(System.Windows.Media.MediaPlayer p, double volume)
        {
            index++;
            if (index >= playlist.Count)
            {
                string last = playlist[playlist.Count - 1];
                Shuffle();
                if (playlist.Count > 1 && playlist[0] == last)
                {
                    (playlist[0], playlist[playlist.Count - 1]) = (playlist[playlist.Count - 1], playlist[0]);
                }

                index = 0;
            }

            p.Volume = volume;
            p.Open(new Uri(playlist[index]));

            if (!Muted)
            {
                p.Play();
            }
        }

        private static void OnTrackEnded(System.Windows.Media.MediaPlayer p)
        {
            if (p != current)
            {
                return;
            }

            if (fading)
            {
                FinishFade();
            }
            else
            {
                SwitchNow();
            }
        }
        private static void OnTrackFailed(System.Windows.Media.MediaPlayer p)
        {
            failedInARow++;

            if (failedInARow >= playlist.Count)
            {
                Stop();
                return;
            }

            if (p == incoming)
            {
                incoming.Close();
                incoming = null;
                fading = false;
            }
            else if (p == current)
            {
                if (fading)
                {
                    FinishFade();
                }
                else
                {
                    SwitchNow();
                }
            }
        }

        private static void Shuffle()
        {
            for (int i = playlist.Count - 1; i > 0; i--)
            {
                int j = rng.Next(i + 1);
                (playlist[i], playlist[j]) = (playlist[j], playlist[i]);
            }
        }
    }
}