using System;

namespace CSharpToTypescript.Helpers
{
    public class Stopwatch
    {
        long timeStart;
        public double SecondResult { get; private set; }
        public Stopwatch Start()
        {
            timeStart = DateTime.Now.Ticks;
            return this;
        }
        public Stopwatch End()
        {
            SecondResult = new TimeSpan(DateTime.Now.Ticks - timeStart).TotalSeconds;
            return this;
        }
    }
}
