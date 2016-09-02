using System;
using Grapevine.Util;
using Shouldly;
using Xunit;

namespace Grapevine.Tests.Util
{
    public class InternalExtensionsTester
    {
        [Fact]
        public void converts_string_from_camel_case_to_title_case()
        {
            const string original = "EnhanceYourCalm";
            const string expected = "Enhance Your Calm";
            original.ConvertCamelCase().ShouldBe(expected);
        }

        [Fact]
        public void implements_is_true_if_type_implements_interface()
        {
            typeof(FakeClass).Implements<IFakeInterfase>().ShouldBeTrue();
        }

        [Fact]
        public void implements_is_false_if_type_does_not_implement_interface()
        {
            typeof(NotFakeClass).Implements<IFakeInterfase>().ShouldBeFalse();
        }

        [Fact]
        public void guid_truncation_return_last_part_of_guid()
        {
            var guid = Guid.NewGuid();
            var trunc = guid.Truncate();
            guid.ToString().EndsWith($"-{trunc}").ShouldBeTrue();
        }
    }

    public interface IFakeInterfase
    {
        int Id { get; set; }
    }

    public class FakeClass : IFakeInterfase
    {
        public int Id { get; set; }
    }

    public class NotFakeClass
    {
        public int Id { get; set; }
    }
}
