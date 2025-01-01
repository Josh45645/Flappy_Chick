using NAudio.Wave;

namespace Flappy_Chick
{
    internal static class SoundManager
    {
        private static WaveOutEvent waveOut;
        private static AudioFileReader audioFileReader;
        private static bool isLooping;
        private static float currentVolume = 0.5f; 
        private static readonly object lockObject = new();


        public static void PlaySoundEffect(string filePath)
        {
            try
            {
                
                var audioFile = new AudioFileReader(filePath);
                var outputDevice = new WaveOutEvent();

                outputDevice.Init(audioFile);
                outputDevice.Volume = currentVolume;
                outputDevice.Play();

               
                while (outputDevice.PlaybackState == PlaybackState.Playing)
                {
                    Thread.Sleep(100);
                }

               
                outputDevice.Dispose();
                audioFile.Dispose();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error playing sound effect: {ex.Message}");
            }
        }


        public static void PlayBackgroundMusic(string filePath, bool loop = false)
        {
            lock (lockObject)
            {
                try
                {
                    StopBackgroundMusic();

                    audioFileReader = new AudioFileReader(filePath);
                    waveOut = new WaveOutEvent();
                    waveOut.Init(audioFileReader);
                    waveOut.Volume = currentVolume; 
                    isLooping = loop;

                    if (isLooping)
                    {
                        waveOut.PlaybackStopped += HandlePlaybackStopped;
                    }

                    waveOut.Play();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error playing background music: {ex.Message}");
                }
            }
        }

        private static void HandlePlaybackStopped(object sender, EventArgs args)
        {
            lock (lockObject)
            {
                if (isLooping && waveOut != null && audioFileReader != null)
                {
                    audioFileReader.Position = 0;
                    waveOut.Play();
                }
            }
        }
 
        public static void StopBackgroundMusic()
        {
            lock (lockObject)
            {
                try
                {
                    if (waveOut != null)
                    {
                        waveOut.PlaybackStopped -= HandlePlaybackStopped;
                        if (waveOut.PlaybackState == PlaybackState.Playing)
                        {
                            waveOut.Stop();
                        }

                        waveOut.Dispose();
                        waveOut = null;
                    }

                    if (audioFileReader != null)
                    {
                        audioFileReader.Dispose();
                        audioFileReader = null;
                    }

                    isLooping = false;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error stopping background music: {ex.Message}");
                }
            }
        }


        public static void SetVolume(float volume)
        {
            lock (lockObject)
            {
                currentVolume = Math.Max(0.0f, Math.Min(volume, 1.0f));

                if (waveOut != null)
                {
                    waveOut.Volume = currentVolume; 
                }
            }
        }



        public static float GetVolume()
        {
            return currentVolume;
        }


    }
}
