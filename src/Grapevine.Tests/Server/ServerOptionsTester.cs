using System;
using Grapevine.Server;
using Grapevine.Util;
using Grapevine.Util.Loggers;
using Shouldly;
using Xunit;

namespace Grapevine.Tests.Server
{
    public class ServerOptionsTester
    {
        [Fact]
        public void generic_configuration()
        {
            var options = new ServerSettings();

            options.Logger.ShouldNotBeNull();
            options.Logger.ShouldBeOfType<NullLogger>();

            options.Router.ShouldNotBeNull();
            options.Router.ShouldBeOfType<Router>();
            
            options.PublicFolder.ShouldNotBeNull();
            options.PublicFolder.ShouldBeOfType<PublicFolder>();

            options.UseHttps.ShouldBeFalse();
            options.Host.ShouldBe("localhost");
            options.Port.ShouldBe("1234");
            options.Connections.ShouldBe(50);
            options.EnableThrowingExceptions.ShouldBeFalse();

            options.OnAfterStart.ShouldBeNull();
            options.OnBeforeStart.ShouldBeNull();

            options.OnAfterStop.ShouldBeNull();
            options.OnBeforeStop.ShouldBeNull();

            options.OnStart.ShouldBeNull();
            options.OnStop.ShouldBeNull();
        }

        [Fact]
        public void on_start_is_synonym_for_on_after_start()
        {
            var options = new ServerSettings();

            Action action1 = () => {  };
            Action action2 = () => {  };

            options.OnStart = action1;

            options.OnBeforeStart.ShouldBeNull();
            options.OnAfterStart.ShouldBe(action1);
            options.OnStart.ShouldBe(action1);

            options.OnAfterStart = action2;

            options.OnBeforeStart.ShouldBeNull();
            options.OnAfterStart.ShouldBe(action2);
            options.OnStart.ShouldBe(action2);
        }

        [Fact]
        public void on_stop_is_synonym_for_on_after_stop()
        {
            var options = new ServerSettings();

            Action action1 = () => {  };
            Action action2 = () => {  };

            options.OnStop = action1;

            options.OnBeforeStop.ShouldBeNull();
            options.OnAfterStop.ShouldBe(action1);
            options.OnStop.ShouldBe(action1);

            options.OnAfterStop = action2;

            options.OnBeforeStop.ShouldBeNull();
            options.OnAfterStop.ShouldBe(action2);
            options.OnStop.ShouldBe(action2);
        }
    }
}
