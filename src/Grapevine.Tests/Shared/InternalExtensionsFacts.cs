using System;
using Grapevine.Server;
using Xunit;
using Grapevine.Shared;
using Shouldly;

namespace Grapevine.Tests.Shared
{
    public class InternalExtensionsFacts
    {
        public class GuidExtensions
        {
            public class TruncateMethod
            {
                [Fact]
                public void ReturnsLastPartOfGuid()
                {
                    var guid = Guid.NewGuid();
                    var trunc = guid.Truncate();
                    guid.ToString().EndsWith($"-{trunc}").ShouldBeTrue();
                }
            }
        }

        public class ObjectExtensions
        {
            public class TryDisposing
            {
                [Fact]
                public void ReturnsTrueOnNonDisposableObjects()
                {
                    var ndo = new NonDisposable();
                    ndo.TryDisposing().ShouldBeTrue();
                    ndo.Disposed.ShouldBeFalse();
                }

                [Fact]
                public void ReturnsTrueOnImplicityDisposableObject()
                {
                    var ido = new ImplicityDisposable();
                    ido.TryDisposing().ShouldBeTrue();
                    ido.Disposed.ShouldBeTrue();
                }

                [Fact]
                public void ReturnsTrueOnExplicitlyDisposableObjects()
                {
                    var edo = new ExplicitlyDisposable();
                    edo.TryDisposing().ShouldBeTrue();
                    edo.Disposed.ShouldBeTrue();
                }

                public class NonDisposable
                {
                    public bool Disposed { get; protected set; }
                }

                public class ImplicityDisposable : IDisposable
                {
                    public bool Disposed { get; protected set; }

                    public void Dispose()
                    {
                        Disposed = true;
                    }
                }

                public class ExplicitlyDisposable : IDisposable
                {
                    public bool Disposed { get; protected set; }

                    void IDisposable.Dispose()
                    {
                        Disposed = true;
                    }
                }
            }
        }

        public class StringExtensions
        {
            public class ConvertCamelCaseMethod
            {
                [Fact]
                public void ConvertsToTitleCase()
                {
                    const string original = "EnhanceYourCalm";
                    const string expected = "Enhance Your Calm";
                    original.ConvertCamelCase().ShouldBe(expected);
                }
            }
        }

        public class TypeExtensions
        {
            public class ImplementsMethod
            {
                [Fact]
                public void ReturnsTrueIfTypeImplementsInterface()
                {
                    typeof(Router).Implements<IRouter>().ShouldBeTrue();
                }

                [Fact]
                public void ReturnsFalseIfTypeDoesNotImplementInterface()
                {
                    typeof(RestServer).Implements<IRouter>().ShouldBeFalse();
                }
            }
        }
    }
}
