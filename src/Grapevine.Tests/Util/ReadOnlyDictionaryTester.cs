using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Grapevine.Util;
using Shouldly;
using Xunit;

namespace Grapevine.Tests.Util
{
    public class ReadOnlyDictionaryTester
    {
        [Fact]
        public void dictionary_operations_not_supported()
        {
            var dict = new Dictionary<string,string>();
            var rodic = new ReadOnlyDictionary<string,string>(dict);

            Should.Throw<ArgumentNullException>(() => new ReadOnlyDictionary<string,string>(null));
            Should.Throw<NotSupportedException>(() => rodic.Add(new KeyValuePair<string, string>("key", "value")));
            Should.Throw<NotSupportedException>(() => rodic.Add("key", "value"));
            Should.Throw<NotSupportedException>(() => rodic.Clear());
            Should.Throw<NotSupportedException>(() => rodic.Remove(new KeyValuePair<string, string>("key", "value")));
            Should.Throw<NotSupportedException>(() => rodic.Remove("key"));
            Should.Throw<NotSupportedException>(() => rodic["key"] = "value");
        }

        [Fact]
        public void dictionary_pass_though_operations()
        {
            var dict = new Dictionary<string, string>();
            dict.Add("key1", "value1");
            dict.Add("key2", "value2");

            var rodic = new ReadOnlyDictionary<string, string>(dict);
            rodic.IsReadOnly.ShouldBeTrue();
            rodic.IsSynchronized.ShouldBeFalse();
            ((ICollection)rodic).Count.ShouldBe(2);
            ((ICollection<KeyValuePair<string,string>>)rodic).Count.ShouldBe(2);

            rodic.Keys.Count.ShouldBe(2);
            rodic.Values.Count.ShouldBe(2);
            rodic["key1"].ShouldBe("value1");

            string value2;
            rodic.TryGetValue("key2", out value2);
            value2.ShouldBe("value2");

            rodic.Contains(new KeyValuePair<string, string>("key1", "value1")).ShouldBeTrue();
            rodic.ContainsKey("key2").ShouldBeTrue();

            var enumerator1 = rodic.GetEnumerator();
            enumerator1.ShouldNotBeNull();

            var enumerator2 = ((IEnumerable) rodic).GetEnumerator();
            enumerator2.ShouldNotBeNull();

            var copy1 = new KeyValuePair<string, string>[2];
            rodic.CopyTo(copy1, 0);
            copy1[0].Key.ShouldBe("key1");
            copy1[0].Value.ShouldBe("value1");
            copy1[1].Key.ShouldBe("key2");
            copy1[1].Value.ShouldBe("value2");

            var copy2 = new object[2];
            rodic.CopyTo(copy2, 0);
            copy2.Length.ShouldBe(2);
            copy2.GetValue(0).IsA<KeyValuePair<string,string>>().ShouldBeTrue();

            var syncroot = rodic.SyncRoot;
            syncroot.ShouldNotBeNull();
        }
    }
}
