using System;
using Grapevine.Util;
using Shouldly;
using Xunit;

namespace Grapevine.Tests.Util
{
    public class InternalExtensionsTester
    {
        [Fact]
        public void converts_httpstatuscode_description_from_camel_case()
        {
            var description = HttpStatusCode.EnhanceYourCalm.ConvertToString();
            description.ShouldBe("Enhance Your Calm");
        }

        //[Fact]
        //public void is_a_returns_true_when_object_is_of_type()
        //{
        //    var fake = new FakeClass();
        //    fake.IsA<IFakeInterfase>().ShouldBeTrue();
        //    fake.IsA<FakeClass>().ShouldBeTrue();
        //}

        //[Fact]
        //public void is_a_returns_false_when_object_is_not_of_type()
        //{
        //    var notfake = new NotFakeClass();
        //    notfake.IsNot<IFakeInterfase>().ShouldBeTrue();
        //    notfake.IsNot<FakeClass>().ShouldBeTrue();
        //}

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
