using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/* ===================================================================================
 * Timer -
 * DESCRIPTION - Timer Classes. 
 * Use Timer<T>, TImer<T1, T2> and Timer<T1, T2, T3> to keep the reference/value of an object 
 * without it changing between constructing the timer and the invocation. 
 * =================================================================================== */


namespace Utils
{
    /// <summary>
    /// One liner for invoking a <see cref="System.Action"/> after a given amount of time. 
    /// </summary>
    public class Timer : Ticker.ITickable
    {
        public bool Ended = false;

        protected event System.Action OnTimerEnd = () => { };
        public float TotalTime { get; set; }
        public float TimeLeft { get; private set; }

        protected bool running;
        protected bool subscribed;

        public bool Running
        {
            get
            {
                return running;
            }
        }

        /// <summary>
        /// Create a new Timer that runs for <paramref name="TotalTime"/> seconds before invoking <paramref name="OnTimerEnd"/>(). 
        /// </summary>
        /// <param name="TotalTime">Total time before callback is activated. </param>
        /// <param name="OnTimerEnd">Callback function when the timer reaches 0 seconds. </param>
        public Timer(float TotalTime, System.Action OnTimerEnd)
        {
            this.TotalTime = TotalTime;
            TimeLeft = TotalTime;
            this.OnTimerEnd += () => { Ended = true; Unsubscribe(); };
            this.OnTimerEnd += OnTimerEnd;
            running = true;
            Subscribe();
        }

        /// <summary>
        /// Resets the timer without pausing or resuming.
        /// </summary>
        public void Reset()
        {
            TimeLeft = TotalTime;
            Ended = false;
        }

        /// <summary>
        /// Starts the timer from the top. 
        /// </summary>
        public void ResetAndResume()
        {
            Reset();
            Resume();
        }

        /// <summary>
        /// Pauses the timer without changing how much time is left. 
        /// </summary>
        public void Pause()
        {
            running = false;
        }

        /// <summary>
        /// Resumes the timer without changing how much time is left. 
        /// </summary>
        public void Resume()
        {
            Subscribe();
            running = true;
        }

        /// <summary>
        /// Forces the timer to end and invokes OnTimerEnd(). Call Abort() to end the timer immediately without invoking.
        /// </summary>
        public void End()
        {
            OnTimerEnd();
        }

        /// <summary>
        /// Forces the timer to end without invoking OnTimerEnd(). call End() to end the timer immediately and invoke. 
        /// </summary>
        /// <remarks>
        /// NOTE: This will delete the timer if no other reference to it is saved. 
        /// </remarks>
        public void Abort()
        {
            Unsubscribe();
            running = false;
            Ended = true;
            TimeLeft = 0;
        }

        void Ticker.ITickable.Tick()
        {
            if (running)
            {
                TimeLeft -= Time.deltaTime;
                if (TimeLeft < 0)
                    OnTimerEnd();
            }
        }

        protected void Subscribe()
        {
            if (!subscribed)
            {
                Ticker.Subscribe(this);
                subscribed = true;
            }
        }
        protected void Unsubscribe()
        {
            if (subscribed)
            {
                Ticker.Unsubscribe(this);
                subscribed = false;
            }
        }
    }

    /// <summary>
    /// One liner for invoking a <see cref="System.Action{T}"/> after a given amount of time. 
    /// </summary>
    /// <typeparam name="T">The Type of object that will be passed into the callback. </typeparam>
    public class Timer<T> : Ticker.ITickable
    {
        public bool Ended = false;

        protected event System.Action<T> OnTimerEnd = (obj) => { };
        public float TotalTime { get; set; }
        public float TimeLeft { get; private set; }

        public T Arg;

        protected bool running;
        protected bool subscribed;

        public bool Running
        {
            get
            {
                return running;
            }
        }


        /// <summary>
        /// Create a new Timer that runs for <paramref name="TotalTime"/> seconds before invoking <paramref name="OnTimerEnd"/>(Arg1). 
        /// </summary>
        /// <param name="TotalTime">Total time before callback is activated. </param>
        /// <param name="OnTimerEnd">Callback function when the timer reaches 0 seconds. </param>
        /// <param name="Arg">Argument of type <see cref="T"/> that will be passed into OnTimerEnd. </param>
        public Timer(float TotalTime, System.Action<T> OnTimerEnd, T Arg)
        {
            this.TotalTime = TotalTime;
            TimeLeft = TotalTime;
            this.OnTimerEnd += (obj) => { Ended = true; Unsubscribe(); };
            this.OnTimerEnd += OnTimerEnd;
            this.Arg = Arg;
            running = true;
            Subscribe();
        }

        /// <summary>
        /// Resets the timer without pausing or resuming.
        /// </summary>
        public void Reset()
        {
            TimeLeft = TotalTime;
            Ended = false;
        }

        /// <summary>
        /// Starts the timer from the top. 
        /// </summary>
        public void ResetAndResume()
        {
            Reset();
            Resume();
        }

        /// <summary>
        /// Pauses the timer without changing how much time is left. 
        /// </summary>
        public void Pause()
        {
            running = false;
        }

        /// <summary>
        /// Resumes the timer without changing how much time is left. 
        /// </summary>
        public void Resume()
        {
            Subscribe();
            running = true;
        }

        /// <summary>
        /// Forces the timer to end and invokes OnTimerEnd(). Call Abort() to end the timer immediately without invoking.
        /// </summary>
        public void End()
        {
            OnTimerEnd(Arg);
        }

        /// <summary>
        /// Forces the timer to end without invoking OnTimerEnd(). call End() to end the timer immediately and invoke. 
        /// </summary>
        /// <remarks>
        /// NOTE: This will delete the timer if no other reference to it is saved. 
        /// </remarks>
        public void Abort()
        {
            Unsubscribe();
            running = false;
            Ended = true;
            TimeLeft = 0;
        }

        void Ticker.ITickable.Tick()
        {
            if (running)
            {
                TimeLeft -= Time.deltaTime;
                if (TimeLeft < 0)
                    OnTimerEnd(Arg);
            }
        }

        protected void Subscribe()
        {
            if (!subscribed)
            {
                Ticker.Subscribe(this);
                subscribed = true;
            }
        }
        protected void Unsubscribe()
        {
            if (subscribed)
            {
                Ticker.Unsubscribe(this);
                subscribed = false;
            }
        }
    }

    /// <summary>
    /// One liner for invoking a <see cref="System.Action{T1, T2}"/> after a given amount of time. 
    /// </summary>
    /// <typeparam name="T1">The Type of object that will be passed into the callback as the first parameter.</typeparam>
    /// <typeparam name="T2">The Type of object that will be passed into the callback as the second parameter.</typeparam>
    public class Timer<T1, T2> : Ticker.ITickable
    {
        public bool Ended = false;

        protected event System.Action<T1, T2> OnTimerEnd = (obj1, obj2) => { };
        public float TotalTime { get; set; }
        public float TimeLeft { get; private set; }

        public T1 Arg1;
        public T2 Arg2;

        protected bool running;
        protected bool subscribed;

        public bool Running
        {
            get
            {
                return running;
            }
        }


        /// <summary>
        /// Create a new Timer that runs for <paramref name="TotalTime"/> seconds before invoking <paramref name="OnTimerEnd"/>(Arg1, Arg2). 
        /// </summary>
        /// <param name="TotalTime">Total time before callback is activated. </param>
        /// <param name="OnTimerEnd">Callback function when the timer reaches 0 seconds. </param>
        /// <param name="Arg1">Argument of type <see cref="T1"/> that will be passed into OnTimerEnd as the first parameter. </param>
        /// <param name="Arg2">Argument of type <see cref="T2"/> that will be passed into OnTimerEnd as the second parameter. </param>
        public Timer(float TotalTime, System.Action<T1, T2> OnTimerEnd, T1 Arg1, T2 Arg2)
        {
            this.TotalTime = TotalTime;
            TimeLeft = TotalTime;
            this.OnTimerEnd += (obj1, obj2) => { Ended = true; Unsubscribe(); };
            this.OnTimerEnd += OnTimerEnd;
            this.Arg1 = Arg1;
            this.Arg2 = Arg2;
            running = true;
            Subscribe();
        }

        /// <summary>
        /// Resets the timer without pausing or resuming.
        /// </summary>
        public void Reset()
        {
            TimeLeft = TotalTime;
            Ended = false;
        }

        /// <summary>
        /// Starts the timer from the top. 
        /// </summary>
        public void ResetAndResume()
        {
            Reset();
            Resume();
        }

        /// <summary>
        /// Pauses the timer without changing how much time is left. 
        /// </summary>
        public void Pause()
        {
            running = false;
        }

        /// <summary>
        /// Resumes the timer without changing how much time is left. 
        /// </summary>
        public void Resume()
        {
            Subscribe();
            running = true;
        }

        /// <summary>
        /// Forces the timer to end and invokes OnTimerEnd(). Call Abort() to end the timer immediately without invoking.
        /// </summary>
        public void End()
        {
            OnTimerEnd(Arg1, Arg2);
        }

        /// <summary>
        /// Forces the timer to end without invoking OnTimerEnd(). call End() to end the timer immediately and invoke. 
        /// </summary>
        /// <remarks>
        /// NOTE: This will delete the timer if no other reference to it is saved. 
        /// </remarks>
        public void Abort()
        {
            Unsubscribe();
            running = false;
            Ended = true;
            TimeLeft = 0;
        }

        void Ticker.ITickable.Tick()
        {
            if (running)
            {
                TimeLeft -= Time.deltaTime;
                if (TimeLeft < 0)
                    OnTimerEnd(Arg1, Arg2);
            }
        }

        protected void Subscribe()
        {
            if (!subscribed)
            {
                Ticker.Subscribe(this);
                subscribed = true;
            }
        }
        protected void Unsubscribe()
        {
            if (subscribed)
            {
                Ticker.Unsubscribe(this);
                subscribed = false;
            }
        }
    }

    /// <summary>
    /// One liner for invoking a <see cref="System.Action{T1, T2, T3}"/> after a given amount of time. 
    /// </summary>
    /// <typeparam name="T1">The Type of object that will be passed into the callback as the first parameter.</typeparam>
    /// <typeparam name="T2">The Type of object that will be passed into the callback as the second parameter.</typeparam>
    /// <typeparam name="T3">The Type of object that will be passed into the callback as the third parameter.</typeparam>
    public class Timer<T1, T2, T3> : Ticker.ITickable
    {
        public bool Ended = false;

        protected event System.Action<T1, T2, T3> OnTimerEnd = (obj1, obj2, obj3) => { };
        public float TotalTime { get; set; }
        public float TimeLeft { get; private set; }

        public T1 Arg1;
        public T2 Arg2;
        public T3 Arg3;

        protected bool running;
        protected bool subscribed;

        public bool Running
        {
            get
            {
                return running;
            }
        }


        /// <summary>
        /// Create a new Timer that runs for <paramref name="TotalTime"/> seconds before invoking <paramref name="OnTimerEnd"/>(Arg1, Arg2, Arg3). 
        /// </summary>
        /// <param name="TotalTime">Total time before callback is activated. </param>
        /// <param name="OnTimerEnd">Callback function when the timer reaches 0 seconds. </param>
        /// <param name="Arg1">Argument of type <see cref="T1"/> that will be passed into OnTimerEnd as the first parameter. </param>
        /// <param name="Arg2">Argument of type <see cref="T2"/> that will be passed into OnTimerEnd as the second parameter. </param>
        /// <param name="Arg3">Argument of type <see cref="T3"/> that will be passed into OnTimerEnd as the third parameter. </param>
        public Timer(float TotalTime, System.Action<T1, T2, T3> OnTimerEnd, T1 Arg1, T2 Arg2, T3 Arg3)
        {
            this.TotalTime = TotalTime;
            TimeLeft = TotalTime;
            this.OnTimerEnd += (obj1, obj2, obj3) => { Ended = true; Unsubscribe(); };
            this.OnTimerEnd += OnTimerEnd;
            this.Arg1 = Arg1;
            this.Arg2 = Arg2;
            this.Arg3 = Arg3;
            running = true;
            Subscribe();
        }

        /// <summary>
        /// Resets the timer without pausing or resuming.
        /// </summary>
        public void Reset()
        {
            TimeLeft = TotalTime;
            Ended = false;
        }

        /// <summary>
        /// Starts the timer from the top. 
        /// </summary>
        public void ResetAndResume()
        {
            Reset();
            Resume();
        }

        /// <summary>
        /// Pauses the timer without changing how much time is left. 
        /// </summary>
        public void Pause()
        {
            running = false;
        }

        /// <summary>
        /// Resumes the timer without changing how much time is left. 
        /// </summary>
        public void Resume()
        {
            Subscribe();
            running = true;
        }

        /// <summary>
        /// Forces the timer to end and invokes OnTimerEnd(). Call Abort() to end the timer immediately without invoking.
        /// </summary>
        public void End()
        {
            OnTimerEnd(Arg1, Arg2, Arg3);
        }

        /// <summary>
        /// Forces the timer to end without invoking OnTimerEnd(). call End() to end the timer immediately and invoke. 
        /// </summary>
        /// <remarks>
        /// NOTE: This will delete the timer if no other reference to it is saved. 
        /// </remarks>
        public void Abort()
        {
            Unsubscribe();
            running = false;
            Ended = true;
            TimeLeft = 0;
        }

        void Ticker.ITickable.Tick()
        {
            if (running)
            {
                TimeLeft -= Time.deltaTime;
                if (TimeLeft < 0)
                    OnTimerEnd(Arg1, Arg2, Arg3);
            }
        }

        protected void Subscribe()
        {
            if (!subscribed)
            {
                Ticker.Subscribe(this);
                subscribed = true;
            }
        }
        protected void Unsubscribe()
        {
            if (subscribed)
            {
                Ticker.Unsubscribe(this);
                subscribed = false;
            }
        }
    }

    /// <summary>
    /// One liner for invoking a <see cref="System.Action{T1, T2, T3, T4}"/> after a given amount of time. 
    /// </summary>
    /// <typeparam name="T1">The Type of object that will be passed into the callback as the first parameter.</typeparam>
    /// <typeparam name="T2">The Type of object that will be passed into the callback as the second parameter.</typeparam>
    /// <typeparam name="T3">The Type of object that will be passed into the callback as the third parameter.</typeparam>
    /// <typeparam name="T4">The Type of object that will be passed into the callback as the fourth parameter.</typeparam>
    public class Timer<T1, T2, T3, T4> : Ticker.ITickable
    {
        public bool Ended = false;

        protected event System.Action<T1, T2, T3, T4> OnTimerEnd = (obj1, obj2, obj3, obj4) => { };
        public float TotalTime { get; set; }
        public float TimeLeft { get; private set; }

        public T1 Arg1;
        public T2 Arg2;
        public T3 Arg3;
        public T4 Arg4;

        protected bool running;
        protected bool subscribed;

        public bool Running
        {
            get
            {
                return running;
            }
        }


        /// <summary>
        /// Create a new Timer that runs for <paramref name="TotalTime"/> seconds before invoking <paramref name="OnTimerEnd"/>(Arg1, Arg2, Arg3, Arg4). 
        /// </summary>
        /// <param name="TotalTime">Total time before callback is activated. </param>
        /// <param name="OnTimerEnd">Callback function when the timer reaches 0 seconds. </param>
        /// <param name="Arg1">Argument of type <see cref="T1"/> that will be passed into OnTimerEnd as the first parameter. </param>
        /// <param name="Arg2">Argument of type <see cref="T2"/> that will be passed into OnTimerEnd as the second parameter. </param>
        /// <param name="Arg3">Argument of type <see cref="T3"/> that will be passed into OnTimerEnd as the third parameter. </param>
        /// <param name="Arg4">Argument of type <see cref="T4"/> that will be passed into OnTimerEnd as the fourth parameter. </param>
        public Timer(float TotalTime, System.Action<T1, T2, T3, T4> OnTimerEnd, T1 Arg1, T2 Arg2, T3 Arg3, T4 Arg4)
        {
            this.TotalTime = TotalTime;
            TimeLeft = TotalTime;
            this.OnTimerEnd += (obj1, obj2, obj3, obj4) => { Ended = true; Unsubscribe(); };
            this.OnTimerEnd += OnTimerEnd;
            this.Arg1 = Arg1;
            this.Arg2 = Arg2;
            this.Arg3 = Arg3;
            this.Arg4 = Arg4;
            running = true;
            Subscribe();
        }

        /// <summary>
        /// Resets the timer without pausing or resuming.
        /// </summary>
        public void Reset()
        {
            TimeLeft = TotalTime;
            Ended = false;
        }

        /// <summary>
        /// Starts the timer from the top. 
        /// </summary>
        public void ResetAndResume()
        {
            Reset();
            Resume();
        }

        /// <summary>
        /// Pauses the timer without changing how much time is left. 
        /// </summary>
        public void Pause()
        {
            running = false;
        }

        /// <summary>
        /// Resumes the timer without changing how much time is left. 
        /// </summary>
        public void Resume()
        {
            Subscribe();
            running = true;
        }

        /// <summary>
        /// Forces the timer to end and invokes OnTimerEnd(). Call Abort() to end the timer immediately without invoking.
        /// </summary>
        public void End()
        {
            OnTimerEnd(Arg1, Arg2, Arg3, Arg4);
        }

        /// <summary>
        /// Forces the timer to end without invoking OnTimerEnd(). call End() to end the timer immediately and invoke. 
        /// </summary>
        /// <remarks>
        /// NOTE: This will delete the timer if no other reference to it is saved. 
        /// </remarks>
        public void Abort()
        {
            Unsubscribe();
            running = false;
            Ended = true;
            TimeLeft = 0;
        }

        void Ticker.ITickable.Tick()
        {
            if (running)
            {
                TimeLeft -= Time.deltaTime;
                if (TimeLeft < 0)
                    OnTimerEnd(Arg1, Arg2, Arg3, Arg4);
            }
        }

        protected void Subscribe()
        {
            if (!subscribed)
            {
                Ticker.Subscribe(this);
                subscribed = true;
            }
        }
        protected void Unsubscribe()
        {
            if (subscribed)
            {
                Ticker.Unsubscribe(this);
                subscribed = false;
            }
        }
    }
}