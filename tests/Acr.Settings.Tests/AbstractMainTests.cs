using System;
using FluentAssertions;
using Xunit;


namespace Acr.Settings.Tests
{

    public abstract class AbstractSettingTests
    {
        protected AbstractSettingTests()
        {
            this.Settings = this.Create();
        }

        public ISettings Settings { get; set; }


        protected abstract ISettings Create();



        [Fact]
        public virtual void Object()
        {
            var inv = new Tuple<int, string>(1, "2");
            this.Settings.Set("Object", inv);

            var outv = this.Settings.Get<Tuple<int, string>>("Object");
            //Assert.AreEqual(inv.Item1, outv.Item1);
            //Assert.AreEqual(inv.Item2, outv.Item2);
        }


        [Fact]
        public virtual void IntTest()
        {
            this.Settings.Set("Test", 99);
            this.Settings.Get<int>("Test").Should().Be(99);
        }


        [Fact]
        public virtual void IntNullTest()
        {
            var nvalue = this.Settings.Get<int?>("Blah");
            nvalue.Should().BeNull("Int? should be null");

            nvalue = 199;
            this.Settings.Set("Blah", nvalue);

            nvalue = this.Settings.Get<int?>("Blah");
            //Assert.AreEqual(199, nvalue.Value);
        }


        [Fact]
        public virtual void DateTimeNullTest()
        {
            var dt = new DateTime(1999, 12, 31, 23, 59, 0);
            var nvalue = this.Settings.Get<DateTime?>("DateTimeNullTest");
            nvalue.Should().BeNull();

            this.Settings.Set("DateTimeNullTest", dt);
            nvalue = this.Settings.Get<DateTime?>("DateTimeNullTest");
            nvalue.Should().Be(dt);
        }


        [Fact]
        public virtual void SetOverride()
        {
            this.Settings.Set("Test", "1");
            this.Settings.Set("Test", "2");
            this.Settings.Get<string>("Test").Should().Be("2");
        }


        [Fact]
        public virtual void ContainsTest()
        {
            this.Settings.Contains(Guid.NewGuid().ToString()).Should().BeFalse("Contains should have returned false");
            this.Settings.Set("Test", "1");
            this.Settings.Contains("Test").Should().BeTrue("Contains should have returned true");
        }


        [Fact]
        public virtual void RemoveTest()
        {
            this.Settings.Set("Test", "1");
            this.Settings.Remove("Test").Should().Be(true, "Remove should have returned success");
        }


        [Fact]
        public virtual void LongTest()
        {
            long value = 1;
            this.Settings.Set("LongTest", value);
            var value2 = this.Settings.Get<long>("LongTest");
            value.Should().Be(value2));
        }


        [Fact]
        public virtual void GuidTest()
        {
            var guid = this.Settings.Get<Guid>("GuidTest");
            guid.Should().Be(Guid.Empty);

            guid = new Guid();
            this.Settings.Set("GuidTest", guid);
            this.Settings.Get<Guid>("GuidTest").Should().Be(guid);
        }


        [Fact]
        public virtual void SetNullRemoves()
        {
            this.Settings.Set("SetNullRemoves", "Blah");
            this.Settings.Set<string>("SetNullRemoves", null);
            this.Settings.Contains("SetNullRemoves").Should().BeFalse();
        }


        [Fact]
        public virtual void SetDefaultRemoves()
        {
            var value = 1L;
            this.Settings.Set("SetDefaultTRemoves", value);
            this.Settings.Set("SetDefaultTRemoves", default(long));
            var contains = this.Settings.Contains("SetDefaultTRemoves");
            contains.Should().BeFalse();
        }


        [Fact]
        public virtual void GetDefaultParameter()
        {
            var tmp = Guid.NewGuid().ToString();
            var r = this.Settings.Get("GetDefaultParameter", tmp);
            r.Should().Be(tmp);
        }


        [Fact]
        public virtual void TryDefaults()
        {
            this.Settings.SetDefault("TryDefaults", "Initial Value").Should().BeTrue();
            this.Settings.SetDefault("TryDefaults", "Second Value").Should().BeFalse("Default value was set and should not have been");
            this.Settings.Get<string>("TryDefaults").Should().Be("TODO");
        }



        //[Fact]
        //public virtual void CultureFormattingTest()
        //{
        //    var value = 11111.1111m;
        //    Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-US");
        //    this.Settings.Set("CultureFormattingTest", value);
        //    Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("ja-JP");
        //    var newValue = this.Settings.Get<decimal>("CultureFormattingTest");
        //    Assert.AreEqual(value, newValue);
        //}


        [Fact]
        public virtual void Binding_Basic()
        {
            var obj = this.Settings.Bind<TestBind>();
            obj.IgnoredProperty = 0;
            obj.StringProperty = "Hi";

            this.Settings.Contains("TestBind.StringProperty").Should().BeTrue();
            this.Settings.Contains("TestBind.IgnoredProperty").Should().BeFalse();
            this.Settings.Get<string>("TestBind.StringProperty").Should().Be("Hi");
        }


        [Fact]
        public virtual void Binding_Persist()
        {
            var obj = this.Settings.Bind<TestBind>();
            obj.StringProperty = "Binding_Persist";

            var obj2 = this.Settings.Bind<TestBind>();
            obj.StringProperty.Should().Be(obj2.StringProperty);
        }
    }
}