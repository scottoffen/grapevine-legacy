// System libs
using System;
using System.IO;
using SysNet = System.Net;
using System.Text.RegularExpressions;

// third party
using NUnit.Framework;
using SocketHttpListener.Net;

// Us.
using Grapevine;
using Grapevine.Server;


[TestFixture]
public class TestFixture
{
   readonly string root = "http://localhost:8123";
   
   // Assumes project output is bin/$(Configuration). CWD is 
   // always the same dir as the DLL with NUnit.
   // 
   readonly Config config = new Config {
      Protocol = "http",
      Host = "localhost",
      Port = "8123",
      WebRoot = "../../Test/web",
      DirIndex = "myindex.html",
      MaxThreads = 3,
      AutoLoadRestResources = false
   };

   
   RESTServer _host;
   
   [TestFixtureSetUp]
   public void SetUp() 
   {
      _host = new RESTServer( config );
      _host.AddResource( new TestResource() );
      _host.Start();
   }

   [TestFixtureTearDown]
   public void TearDown()
   {
      _host.Stop();
   }
   
   [Test]
   public void DefaultIndexTest()
   {
      string body = _HttpGet( root ); // get index
      Assert.AreEqual( "dummy", body );
   }

   [Test]
   public void StaticSubdirTest()
   {
      string body = _HttpGet( root + "/subdir/file1.txt" );
      Assert.AreEqual( "file1 content", body );
   }
   
   [Test]
   public void StaticWithSpacesTest()
   {
      string body = _HttpGet( root + "/file with spaces.txt" );
      Assert.AreEqual( "file contents", body );
   }

   [Test]
   public void SimpleRouteTest()
   {
      string body = _HttpGet( root + "/test/hello" );
      Assert.AreEqual( "hello, world", body );
   }

   [Test]
   public void SpaceRouteTest()
   {
      string body = _HttpGet( root + "/test/route with spaces" );
      Assert.AreEqual( "hello with spaces", body );
   }

   [Test]
   public void QueryPathPatternTest()
   {
      string body = _HttpGet( root + "/test/query/one/two" );
      Assert.AreEqual( "one,two", body );
   }
   
   // gets a url and assumes it's text
   string _HttpGet( string url )
   {
       SysNet.HttpWebRequest request = (SysNet.HttpWebRequest)SysNet.WebRequest.Create( url );
       using (SysNet.HttpWebResponse response = (SysNet.HttpWebResponse)request.GetResponse())
       {
          using (StreamReader reader = new StreamReader( response.GetResponseStream() ))
          {
              return reader.ReadToEnd();
          }
       }
   }

   // We are not auto loading it, so it does not have to be sealed. 
   // In fact, since we are setting autoload to false, we want attempts
   // to autoload this to show up as a test failure.
   public class TestResource : RESTResource
   {
      [RESTRoute(Method = HttpMethod.GET, PathInfo = @"^/test/hello$")]
      public void HelloWorld( HttpListenerContext ctx )
      {
         SendTextResponse( ctx, "hello, world" );
      }
      
      [RESTRoute(Method = HttpMethod.GET, PathInfo = @"^/test/route with spaces$")]
      public void Space( HttpListenerContext ctx )
      {
         SendTextResponse( ctx, "hello with spaces" );
      }
      
      [RESTRoute(Method = HttpMethod.GET, PathInfo = @"^/test/query/(?<param1>.+)/(?<param2>.+)$")]
      public void Space( HttpListenerContext ctx, Match match )
      {
         string param1 = match.Groups["param1"].Value;
         string param2 = match.Groups["param2"].Value;
         
         SendTextResponse( ctx, param1 + "," + param2 );
      }
   }
}