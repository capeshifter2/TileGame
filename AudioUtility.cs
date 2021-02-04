using System;
using System.Threading;
using NAudio.Wave;
public class AudioUtility
{
	public AudioUtility()
	{
		
	}
	public void PlaySong(bool doLoop, string path)
    {
        
        using (var sound = new AudioFileReader(path))
        using (var outputDevice = new WaveOutEvent())
        {
            do
            {
                outputDevice.Init(sound);
                outputDevice.Play();
                while (outputDevice.PlaybackState == PlaybackState.Playing)
                {
                    Thread.Sleep(1000);
                }
            }
            while (doLoop);
        }
    }
    
}
