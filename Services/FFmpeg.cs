using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FFmpeg.NET;

namespace MerpBot.Services;
public class FFMpeg
{
    public Engine Engine { get; set; }
    public FFMpeg()
    {
        Engine = new Engine(MakeDirectoryFromWorking("ffmpeg/bin/ffmpeg.exe"));
    }

    private string MakeDirectoryFromWorking(string fileLocation) => Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location)!, fileLocation);
}
