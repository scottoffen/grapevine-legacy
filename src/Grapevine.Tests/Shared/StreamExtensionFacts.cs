using System.IO;
using System.Linq;
using System.Text;
using Grapevine.Shared;
using Shouldly;
using Xunit;

namespace Grapevine.Tests.Shared
{
    public class StreamExtensionFacts
    {
        public static string Basepath = Path.Combine(Directory.GetCurrentDirectory(), "test-get-bytes");

        public class GetTextBytesMethod
        {
            private static readonly byte[] ExpectedBytes = {
                84, 104, 105, 115, 32, 105, 115, 32, 115,
                105, 109, 112, 108, 101, 32, 116, 101,
                120, 116, 32, 102, 105, 108, 101, 32, 116,
                111, 32, 116, 101, 115, 116, 32, 103, 101,
                116, 116, 105, 110, 103, 32, 116, 104,
                101, 32, 98, 121, 116, 101, 115, 32, 111,
                102, 32, 97, 32, 116, 101, 120, 116, 32,
                102, 105, 108, 101, 46
            };

            [Fact]
            public void ReturnsCorrectByteArray()
            {
                var filestream = new FileStream(Basepath + ".txt", FileMode.Open);
                filestream.GetTextBytes(Encoding.ASCII).SequenceEqual(ExpectedBytes).ShouldBeTrue();
            }
        }

        public class GetBinaryBytesMethod
        {
            private static readonly byte[] ExpectedBytes = {
                137, 80, 78, 71, 13, 10, 26, 10, 0,
                0, 0, 13, 73, 72, 68, 82, 0, 0, 0,
                10, 0, 0, 0, 10, 8, 2, 0, 0, 0,
                2, 80, 88, 234, 0, 0, 0, 1, 115,
                82, 71, 66, 0, 174, 206, 28, 233, 0,
                0, 0, 4, 103, 65, 77, 65, 0, 0,
                177, 143, 11, 252, 97, 5, 0, 0, 0,
                9, 112, 72, 89, 115, 0, 0, 14, 195,
                0, 0, 14, 195, 1, 199, 111, 168, 100,
                0, 0, 0, 24, 116, 69, 88, 116, 83,
                111, 102, 116, 119, 97, 114, 101, 0, 112,
                97, 105, 110, 116, 46, 110, 101, 116, 32,
                52, 46, 48, 46, 54, 252, 140, 99, 223,
                0, 0, 0, 41, 73, 68, 65, 84, 40,
                83, 99, 248, 207, 128, 130, 208, 249, 104,
                8, 157, 143, 134, 208, 249, 104, 8, 157,
                143, 134, 208, 249, 29, 168, 0, 93, 30,
                42, 12, 3, 244, 147, 102, 248, 15, 0,
                74, 126, 114, 142, 14, 108, 150, 112, 0,
                0, 0, 0, 73, 69, 78, 68, 174, 66,
                96, 130
            };

            [Fact]
            public void ReturnsCorrectByteArray()
            {
                var filestream = new FileStream(Basepath + ".png", FileMode.Open);
                var bytes = filestream.GetBinaryBytes();
                bytes.SequenceEqual(ExpectedBytes).ShouldBeTrue();
            }
        }
    }
}
