using System.Net;
using Grapevine.Interfaces.Server;
using Grapevine.Server;
using NSubstitute;
using Shouldly;
using Xunit;

namespace Grapevine.Tests.Server
{
    public class AdvancedRestServerFacts
    {
        [Fact]
        public void AuthenticationSchemeSelectorDelegateProperty()
        {
            var listener = Substitute.For<IHttpListener>();
            var advanced = new AdvancedRestServer(listener);
            
            var val = advanced.AuthenticationSchemeSelectorDelegate;
            advanced.AuthenticationSchemeSelectorDelegate = val;

            var temp = listener.Received().AuthenticationSchemeSelectorDelegate;
            listener.Received().AuthenticationSchemeSelectorDelegate = val;
        }

        [Fact]
        public void AuthenticationSchemesProperty()
        {
            var listener = Substitute.For<IHttpListener>();
            var advanced = new AdvancedRestServer(listener);

            var val = advanced.AuthenticationSchemes;
            advanced.AuthenticationSchemes = val;

            var temp = listener.Received().AuthenticationSchemes;
            listener.Received().AuthenticationSchemes = val;
        }

        //[Fact]
        //public void ExtendedProtectionPolicyProperty()
        //{
        //    var listener = Substitute.For<IHttpListener>();
        //    var advanced = new AdvancedRestServer(listener);

        //    var val = advanced.ExtendedProtectionPolicy;
        //    advanced.ExtendedProtectionPolicy = val;

        //    var temp = listener.Received().ExtendedProtectionPolicy;
        //    listener.Received().ExtendedProtectionPolicy = val;
        //}

        //[Fact]
        //public void ExtendedProtectionSelectorDelegateProperty()
        //{
        //    var listener = Substitute.For<IHttpListener>();
        //    var advanced = new AdvancedRestServer(listener);

        //    var val = advanced.ExtendedProtectionSelectorDelegate;
        //    advanced.ExtendedProtectionSelectorDelegate = val;

        //    var temp = listener.Received().ExtendedProtectionSelectorDelegate;
        //    listener.Received().ExtendedProtectionSelectorDelegate = val;
        //}

        [Fact]
        public void IgnoreWriteExceptionsProperty()
        {
            var listener = Substitute.For<IHttpListener>();
            var advanced = new AdvancedRestServer(listener);

            var val = advanced.IgnoreWriteExceptions;
            advanced.IgnoreWriteExceptions = val;

            var temp = listener.Received().IgnoreWriteExceptions;
            listener.Received().IgnoreWriteExceptions = val;
        }

        [Fact]
        public void RealmProperty()
        {
            var listener = Substitute.For<IHttpListener>();
            var advanced = new AdvancedRestServer(listener);

            var val = advanced.Realm;
            advanced.Realm = val;

            var temp = listener.Received().Realm;
            listener.Received().Realm = val;
        }

        [Fact]
        public void IsSupportedProperty()
        {
            var listener = Substitute.For<IHttpListener>();
            var advanced = new AdvancedRestServer(listener);

            var val = advanced.IsSupported;
            val.Equals(System.Net.HttpListener.IsSupported).ShouldBeTrue();
        }

        [Fact]
        public void UnsafeConnectionNtlmAuthenticationProperty()
        {
            var listener = Substitute.For<IHttpListener>();
            var advanced = new AdvancedRestServer(listener);

            var val = advanced.UnsafeConnectionNtlmAuthentication;
            advanced.UnsafeConnectionNtlmAuthentication = val;

            var temp = listener.Received().UnsafeConnectionNtlmAuthentication;
            listener.Received().UnsafeConnectionNtlmAuthentication = val;
        }

        [Fact]
        public void AbortMethod()
        {
            var listener = Substitute.For<IHttpListener>();
            var advanced = new AdvancedRestServer(listener);

            advanced.Abort();
            listener.Received().Abort();
        }

        [Fact]
        public void CloseMethod()
        {
            var listener = Substitute.For<IHttpListener>();
            var advanced = new AdvancedRestServer(listener);

            advanced.Close();
            listener.Received().Close();
        }

        [Fact]
        public void StartMethod()
        {
            var listener = Substitute.For<IHttpListener>();
            var advanced = new AdvancedRestServer(listener);

            advanced.Start();
            listener.Received().Start();
        }

        [Fact]
        public void StopMethod()
        {
            var listener = Substitute.For<IHttpListener>();
            var advanced = new AdvancedRestServer(listener);

            advanced.Stop();
            listener.Received().Stop();
        }
    }
}
