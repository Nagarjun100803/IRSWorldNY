using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace IRSClassLib
{
    public class irsThreadManager
    {
        int maxthead = 0;
        static int ctr = 1;
        static int i = 1;        
        static object[] tname;
        static int pos = 0;
        String message = String.Empty;        
        public int MaxThread
        {
            get { return maxthead; }
            set { maxthead = value; }
        }

        public String Message
        {
            get { return message; }
            set { message = value; }
        }
        
        public void addThread(object value)
        {
            try
            {
                if (this.releaseThreads())
                {
                    lock (this)
                    {
                        ctr = 1;                        
                        foreach (Thread t in tname)
                        {
                            if (t == null)
                            {
                                tname[ctr - 1] = value;
                                i++;
                                break;
                            }
                            ctr++;
                        }
                    }
                }
            }
            catch 
            { }
        }

        public bool KillAllThreads()
        {
            try
            {
                foreach (Thread t in tname)
                {
                    if (t != null && t.IsAlive)
                    {
                        t.Abort();
                    }
                }                
            }
            catch (ThreadAbortException e)
            {
                this.Message = e.StackTrace.ToString();
                return false;
            }
            finally { }
            return true;
        }
        public bool isThreadExists(string s)
        {   
            foreach (Thread t in tname)
            {
                if (t != null)
                {
                    if (s == t.Name.ToString())
                    {
                        if (t != null && t.IsAlive)
                        {
                            return true;
                        }
                    }
                }
                return false;            
            }
            return false;
        }
        public bool checkRunningThread()
        {
            foreach (Thread t in tname)
            {
                if (t != null && t.IsAlive)
                {
                    return true;
                }                
            }
            return false;
        }
        /*
        public bool isThreadExists(string s)
        {            
            foreach (Thread t in tname)
            {
                if (t != null)
                {
                    if (s == t.Name.ToString())
                    {
                        if (t != null && t.IsAlive)
                        {
                            Thread.Sleep(10);
                            return true;
                        }
                    }
                }
                return false;
            }
            return false;
        }
        */
        
        public bool releaseThreads()
        {
            
            try
            {
                if (i > maxthead)
                {
                    //Console.WriteLine("Max thread..");                    
                    while (i > maxthead)
                    {
                        pos = 0;
                        bool a = false;

                        foreach (Thread t in tname)
                        {
                            if (t != null && !t.IsAlive)
                            {
                                t.Abort();
                                i--;
                                tname[pos] = null;
                                a = true;
                            }
                            pos++;
                        }
                        if (a) 
                        { break; }
                    }
                }
            }
            catch
            {  }

            return true;
        }
        
        /*
        public bool releaseThreads()
        {
            if (i > maxthead)
            {
                //Console.WriteLine("Max thread..");
                int ii = 1;
                while (ii < maxthead)
                {
                    foreach (Thread t in tname)
                    {
                        if (t != null && !t.IsAlive)
                        {
                            t.Abort();
                            ii++;
                        }
                        else if (t == null)
                        {
                            ii++;
                        }
                    }
                    if (ii < maxthead)
                    {
                        ii = 1;
                        i = 1;
                    }
                }
                ClearList(tname);
                i = 1;
            }
            return true;
        }
        
        */
        public static void ClearList(IList iList)
        {
            iList.Clear();
        }
        public irsThreadManager(int n)
        {
            tname = new object[n];            
            i = 1;            
        }
        ~irsThreadManager() { }
    }

    public static class ThreadKill
    {
        private static bool thread_Kill;

        public static bool Thread_Kill
        {
            get { return thread_Kill; }
            set { thread_Kill = value; }
        }


    }
}
