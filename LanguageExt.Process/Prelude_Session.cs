﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using static LanguageExt.Prelude;
using LanguageExt.UnitsOfMeasure;

namespace LanguageExt
{
    /// <summary>
    /// 
    ///     Process: Session functions
    /// 
    ///     These functions facilitate the use of sessions that live from
    ///     Process to Process.  Essentially if there's an active Session
    ///     ID then it will be packaged with each message that is sent via
    ///     tell or ask.  
    /// 
    /// </summary>
    public static partial class Process
    {
        /// <summary>
        /// Starts a new session in the Process system with the specified
        /// session ID
        /// </summary>
        /// <param name="sid">Session ID</param>
        public static Unit sessionStart(string sid, Time timeout)
        {
            SessionManager.Start(sid, (int)timeout.Seconds);
            ActorContext.SessionId = sid;
            return unit;
        }

        /// <summary>
        /// Ends a session in the Process system with the specified
        /// session ID
        /// </summary>
        /// <param name="sid">Session ID</param>
        public static Unit sessionStop(string sid) =>
            SessionManager.Stop(sid);

        /// <summary>
        /// Touch a session
        /// Time-stamps the session so that its time-to-expiry is reset
        /// </summary>
        /// <param name="sid">Session ID</param>
        public static Unit sessionTouch(string sid) =>
            SessionManager.Touch(sid);

        /// <summary>
        /// Gets the current session ID
        /// </summary>
        /// <remarks>Also touches the session so that its time-to-expiry 
        /// is reset</remarks>
        /// <returns>Optional session ID</returns>
        public static Option<string> sessionId() 
        {
            ActorContext.SessionId.IfSome(SessionManager.Touch);
            return ActorContext.SessionId;
        }

        /// <summary>
        /// Set the meta-data to store with the session, this is typically
        /// user credentials when they've logged in.  But can be anything.
        /// </summary>
        /// <param name="sid">Session ID</param>
        /// <param name="data">Data to store</param>
        public static void sessionSetData(string sid, object data) =>
            SessionManager.SetSessionData(sid, data);

        /// <summary>
        /// Clear the meta-data stored with the session
        /// </summary>
        /// <param name="sid">Session ID</param>
        public static void sessionClearData(string sid) =>
            SessionManager.ClearSessionData(sid);

        /// <summary>
        /// Get the meta-data stored with the session, this is typically
        /// user credentials when they've logged in.  But can be anything.
        /// </summary>
        /// <param name="sid">Session ID</param>
        public static Option<T> sessionGetData<T>(string sid) =>
            ActorContext.GetSessionData<T>(sid);

        /// <summary>
        /// Returns True if there is an active session
        /// </summary>
        /// <returns></returns>
        public static bool hasSession() =>
            ActorContext.SessionId.IsSome;

        /// <summary>
        /// Acquires a session for the duration of invocation of the 
        /// provided function
        /// </summary>
        /// <param name="sid">Session ID</param>
        /// <param name="f">Function to invoke</param>
        /// <returns>Result of the function</returns>
        public static R withSession<R>(string sid, Func<R> f) =>
            ActorContext.WithContext<R>(
                ActorContext.SelfProcess,
                ActorContext.SelfProcess.Actor.Parent,
                Process.Sender, 
                ActorContext.CurrentRequest, 
                ActorContext.CurrentMsg, 
                Some(sid), 
                f);

        /// <summary>
        /// Acquires a session for the duration of invocation of the 
        /// provided action
        /// </summary>
        /// <param name="sid">Session ID</param>
        /// <param name="f">Action to invoke</param>
        public static Unit withSession(string sid, Action f) =>
            withSession(sid, fun(f));
    }
}
