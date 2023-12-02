// See https://aka.ms/new-console-template for more information
using System;
using ConsoleApp1;
using System.IO;
using System.Threading;
using System.Security.Cryptography;
using System.Diagnostics;

namespace ConsoleApp1 // Note: actual namespace depends on the project name.
{
    internal class Program
    {
        const string SHARED_MUTEX_NAME = "my_mutex_name";
        static void Main(string[] args)
        {
            int pid = Process.GetCurrentProcess().Id;
            Thread newThread = new Thread(CheckMutex);
            newThread.Start();
            while (true)
            {
                Console.WriteLine("Process {0} is do orther thing...", pid);
                Thread.Sleep(2000);
            }

        }
        static void CheckMutex()
        {

            int pid = Process.GetCurrentProcess().Id;
            using (Mutex mutex = new Mutex(false, SHARED_MUTEX_NAME))
            {
                while (true)
                {
                    Console.WriteLine("Press any key to let process {0} acquire the shared mutex.", pid);
                    Console.ReadKey();


                    while (!mutex.WaitOne(1000))
                    {
                        Console.WriteLine("Process {0} is waiting for the shared mutex...", pid);
                    }

                    Console.WriteLine("Process {0} has acquired the shared mutex. Press any key to release it.", pid);
                    Console.ReadKey();

                    mutex.ReleaseMutex();
                    Console.WriteLine("Process {0} released the shared mutex.", pid);
                }
            }


        }
    }
}
