﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestConsole
{
    class CounterWatch
    {

        public long ElapsedMilliseconds;
        double UsedMemory;
        int count;
        string name;
        public void Start(string _name,Action action, int loopCount=1)
        {
            action();
            //GC.Collect();

            var m1 = GC.GetTotalMemory(false);
            count = loopCount;
            name = _name;
            ElapsedMilliseconds = 0;
            UsedMemory = 0;
            var sw = new Stopwatch();
            sw.Start();
        
            for (int i = 0; i < loopCount; i++)
            {
                action();
            }
            sw.Stop();
            var m2 = GC.GetTotalMemory(false);

            ElapsedMilliseconds = sw.ElapsedMilliseconds;
            UsedMemory = Math.Round((m2 - m1) / 1024.0, 4);
          
            Console.WriteLine(this.ToString());
            System.Threading.Thread.Sleep(500);
            GC.Collect();
        }
        public override string ToString()
        {
            return $"{name} {count}次 用时:{ElapsedMilliseconds}ms";
        }
    }
}
