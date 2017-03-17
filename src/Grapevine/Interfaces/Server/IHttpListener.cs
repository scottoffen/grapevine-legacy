using System;
using System.Net;
using System.Security.Authentication.ExtendedProtection;
using System.Security.Permissions;

namespace Grapevine.Interfaces.Server
{
    /// <summary>
    /// Interface for a simple, programmatically controlled HTTP protocol InnerListener.
    /// </summary>
    public interface IHttpListener : IDisposable
    {
        /// <summary>
        /// Gets or sets the scheme used to authenticate clients.
        /// </summary>
        AuthenticationSchemes AuthenticationSchemes { get; set; }

        /// <summary>
        /// Gets or sets the delegate called to determine the protocol used to authenticate clients.
        /// </summary>
        AuthenticationSchemeSelector AuthenticationSchemeSelectorDelegate { get; set; }

        /// <summary>
        /// Gets a default list of Service Provider Names (SPNs) as determined by registered prefixes.
        /// </summary>
        ServiceNameCollection DefaultServiceNames { get; }

        /// <summary>
        /// Get or set the ExtendedProtectionPolicy to use for extended protection for a session.
        /// </summary>
        //ExtendedProtectionPolicy ExtendedProtectionPolicy { get; set; }

        /// <summary>
        /// Get or set the delegate called to determine the ExtendedProtectionPolicy to use for each request.
        /// </summary>
        //System.Net.HttpListener.ExtendedProtectionSelector ExtendedProtectionSelectorDelegate { get; set; }

        /// <summary>
        /// Gets or sets a Boolean value that specifies whether your application receives exceptions that occur when an HttpListener sends the response to the client.
        /// </summary>
        bool IgnoreWriteExceptions { get; set; }

        /// <summary>
        /// Gets a value that indicates whether HttpListener has been started.
        /// </summary>
        bool IsListening { get; }

        /// <summary>
        /// Gets the Uniform Resource Identifier (URI) prefixes handled by this HttpListener object.
        /// </summary>
        HttpListenerPrefixCollection Prefixes { get; }

        /// <summary>
        /// Gets or sets the realm, or resource partition, associated with this HttpListener object.
        /// </summary>
        string Realm { get; set; }

        // .NET > 4.0
        /// <summary>
        /// The timeout manager for this HttpListener instance.
        /// </summary>
        //HttpListenerTimeoutManager TimeoutManager { get; }

        /// <summary>
        /// Gets or sets a Boolean value that controls whether, when NTLM is used, additional requests using the same Transmission Control Protocol (TCP) connection are required to authenticate.
        /// </summary>
        bool UnsafeConnectionNtlmAuthentication { get; set; }

        ///<summary>
        ///Shuts down the HttpListener object immediately, discarding all currently queued requests.
        ///</summary>
        void Abort();

        ///<summary>
        ///Begins asynchronously retrieving an incoming request.
        ///</summary>
        [HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
        IAsyncResult BeginGetContext(AsyncCallback callback, object state);

        ///<summary>
        ///Shuts down the HttpListener.
        ///</summary>
        void Close();

        ///<summary>
        ///Completes an asynchronous operation to retrieve an incoming client request.
        ///</summary>
        HttpListenerContext EndGetContext(IAsyncResult asyncResult);

        ///<summary>
        ///Waits for an incoming request and returns when one is received.
        ///</summary>
        HttpListenerContext GetContext();

        // .NET > 4.0
        /// <summary>
        /// Waits for an incoming request as an asynchronous operation.
        /// </summary>
        /// <returns>Task</returns>
        //[HostProtectionAttribute(SecurityAction.LinkDemand, ExternalThreading = true)]
        //Task<HttpListenerContext> GetContextAsync();

        ///<summary>
        ///Allows this instance to receive incoming requests.
        ///</summary>
        void Start();

        ///<summary>
        ///Causes this instance to stop receiving incoming requests.
        ///</summary>
        void Stop();
    }

    public class HttpListener : IHttpListener
    {
        protected internal readonly System.Net.HttpListener InnerListener;

        public HttpListener(System.Net.HttpListener listener)
        {
            InnerListener = listener;
        }

        public AuthenticationSchemes AuthenticationSchemes
        {
            get { return InnerListener.AuthenticationSchemes; }
            set { InnerListener.AuthenticationSchemes = value; }
        }

        public AuthenticationSchemeSelector AuthenticationSchemeSelectorDelegate
        {
            get { return InnerListener.AuthenticationSchemeSelectorDelegate; }
            set { InnerListener.AuthenticationSchemeSelectorDelegate = value; }
        }

        public ServiceNameCollection DefaultServiceNames => InnerListener.DefaultServiceNames;

        //public ExtendedProtectionPolicy ExtendedProtectionPolicy
        //{
        //    get { return InnerListener.ExtendedProtectionPolicy; }
        //    set
        //    {
        //        InnerListener.ExtendedProtectionPolicy = value;
        //    }
        //}

        //public System.Net.HttpListener.ExtendedProtectionSelector ExtendedProtectionSelectorDelegate
        //{
        //    get { return InnerListener.ExtendedProtectionSelectorDelegate; }
        //    set { InnerListener.ExtendedProtectionSelectorDelegate = value; }
        //}

        public bool IgnoreWriteExceptions
        {
            get { return InnerListener.IgnoreWriteExceptions; }
            set { InnerListener.IgnoreWriteExceptions = value; }
        }

        public bool IsListening => InnerListener.IsListening;

        public HttpListenerPrefixCollection Prefixes => InnerListener.Prefixes;

        public string Realm
        {
            get { return InnerListener.Realm; }
            set { InnerListener.Realm = value; }
        }

        public bool UnsafeConnectionNtlmAuthentication
        {
            get { return InnerListener.UnsafeConnectionNtlmAuthentication; }
            set { InnerListener.UnsafeConnectionNtlmAuthentication = value; }
        }

        public void Dispose()
        {
            InnerListener.Close();
        }

        public void Abort()
        {
            InnerListener.Abort();
        }

        public IAsyncResult BeginGetContext(AsyncCallback callback, object state)
        {
            return InnerListener.BeginGetContext(callback, state);
        }

        public void Close()
        {
            InnerListener.Close();
        }

        public HttpListenerContext EndGetContext(IAsyncResult asyncResult)
        {
            return InnerListener.EndGetContext(asyncResult);
        }

        public HttpListenerContext GetContext()
        {
            return InnerListener.GetContext();
        }

        public void Start()
        {
            InnerListener.Start();
        }

        public void Stop()
        {
            InnerListener.Stop();
        }
    }
}