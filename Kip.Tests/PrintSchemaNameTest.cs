using Xunit;

namespace Kip.Tests
{
    public class PrintSchemaNameTest
    {
        PrintSchemaName basicValue = new PrintSchemaName(Exp.Namespace + "SomeName");

        [Fact]
        public void EqualsReturnTrueWhenCallsWithSameValueObject()
        {
            PrintSchemaName sameValue = new PrintSchemaName(Exp.Namespace + "SomeName");
            Assert.True(basicValue == sameValue);
            Assert.True(basicValue.Equals(sameValue));
        }

        [Fact]
        public void EqualsReturnTrueWhenCallsWithSameReference()
        {
            PrintSchemaName sameReference = basicValue;
            Assert.True(basicValue == sameReference);
            Assert.True(basicValue.Equals(sameReference));
        }

        [Fact]
        public void EqualsReturnFalseWhenCallsWithNullReference()
        {
            PrintSchemaName nullReference = null;
            Assert.False(basicValue == nullReference);
            Assert.False(basicValue.Equals(nullReference));

            Assert.True(nullReference == null);
            // Assert.True(nullReference.Equals(null));
        }

        [Fact]
        public void EqualsReturnFalseWhenCallsWithOtherValueObject()
        {
            PrintSchemaName otherValue = new PrintSchemaName(Exp.Namespace + "OtherName");
            Assert.False(basicValue == otherValue);
            Assert.False(basicValue.Equals(otherValue));
        }
    }

    public class FeatureNameTest
    {
        FeatureName basicValue = new FeatureName(Exp.Namespace + "SomeName");

        [Fact]
        public void EqualsReturnTrueWhenCallsWithSameValueObject()
        {
            FeatureName sameValue = new FeatureName(Exp.Namespace + "SomeName");
            Assert.True(basicValue == sameValue);
            Assert.True(basicValue.Equals(sameValue));
        }


        [Fact]
        public void EqualsReturnTrueWhenCallsWithSameReference()
        {
            FeatureName sameReference = basicValue;
            Assert.True(basicValue == sameReference);
            Assert.True(basicValue.Equals(sameReference));
        }

        [Fact]
        public void EqualsReturnFalseWhenCallsWithNullReference()
        {
            FeatureName nullReference = null;
            Assert.False(basicValue == nullReference);
            Assert.False(basicValue.Equals(nullReference));

            Assert.True(nullReference == null);
            // Assert.True(nullReference.Equals(null));
        }

        [Fact]
        public void EqualsReturnFalseWhenCallsWithOtherValueObject()
        {
            FeatureName otherValue = new FeatureName(Exp.Namespace + "OtherName");
            Assert.False(basicValue == otherValue);
            Assert.False(basicValue.Equals(otherValue));
        }
    }

    public class ParameterNameTest
    {
        ParameterName basicValue = new ParameterName(Exp.Namespace + "SomeName");

        [Fact]
        public void EqualsReturnTrueWhenCallsWithSameValueObject()
        {
            ParameterName sameValue = new ParameterName(Exp.Namespace + "SomeName");
            Assert.True(basicValue == sameValue);
            Assert.True(basicValue.Equals(sameValue));
        }


        [Fact]
        public void EqualsReturnTrueWhenCallsWithSameReference()
        {
            ParameterName sameReference = basicValue;
            Assert.True(basicValue == sameReference);
            Assert.True(basicValue.Equals(sameReference));
        }

        [Fact]
        public void EqualsReturnFalseWhenCallsWithNullReference()
        {
            ParameterName nullReference = null;
            Assert.False(basicValue == nullReference);
            Assert.False(basicValue.Equals(nullReference));

            Assert.True(nullReference == null);
            // Assert.True(nullReference.Equals(null));
        }

        [Fact]
        public void EqualsReturnFalseWhenCallsWithOtherValueObject()
        {
            ParameterName otherValue = new ParameterName(Exp.Namespace + "OtherName");
            Assert.False(basicValue == otherValue);
            Assert.False(basicValue.Equals(otherValue));
        }
    }

    public class ScoredPropertyNameTest
    {
        ScoredPropertyName basicValue = new ScoredPropertyName(Exp.Namespace + "SomeName");

        [Fact]
        public void EqualsReturnTrueWhenCallsWithSameValueObject()
        {
            ScoredPropertyName sameValue = new ScoredPropertyName(Exp.Namespace + "SomeName");
            Assert.True(basicValue == sameValue);
            Assert.True(basicValue.Equals(sameValue));
        }


        [Fact]
        public void EqualsReturnTrueWhenCallsWithSameReference()
        {
            ScoredPropertyName sameReference = basicValue;
            Assert.True(basicValue == sameReference);
            Assert.True(basicValue.Equals(sameReference));
        }

        [Fact]
        public void EqualsReturnFalseWhenCallsWithNullReference()
        {
            ScoredPropertyName nullReference = null;
            Assert.False(basicValue == nullReference);
            Assert.False(basicValue.Equals(nullReference));

            Assert.True(nullReference == null);
            // Assert.True(nullReference.Equals(null));
        }

        [Fact]
        public void EqualsReturnFalseWhenCallsWithOtherValueObject()
        {
            ScoredPropertyName otherValue = new ScoredPropertyName(Exp.Namespace + "OtherName");
            Assert.False(basicValue == otherValue);
            Assert.False(basicValue.Equals(otherValue));
        }
    }

    public class PropertyNameTest
    {
        PropertyName basicValue = new PropertyName(Exp.Namespace + "SomeName");

        [Fact]
        public void EqualsReturnTrueWhenCallsWithSameValueObject()
        {
            PropertyName sameValue = new PropertyName(Exp.Namespace + "SomeName");
            Assert.True(basicValue == sameValue);
            Assert.True(basicValue.Equals(sameValue));
        }


        [Fact]
        public void EqualsReturnTrueWhenCallsWithSameReference()
        {
            PropertyName sameReference = basicValue;
            Assert.True(basicValue == sameReference);
            Assert.True(basicValue.Equals(sameReference));
        }

        [Fact]
        public void EqualsReturnFalseWhenCallsWithNullReference()
        {
            PropertyName nullReference = null;
            Assert.False(basicValue == nullReference);
            Assert.False(basicValue.Equals(nullReference));

            Assert.True(nullReference == null);
            // Assert.True(nullReference.Equals(null));
        }

        [Fact]
        public void EqualsReturnFalseWhenCallsWithOtherValueObject()
        {
            PropertyName otherValue = new PropertyName(Exp.Namespace + "OtherName");
            Assert.False(basicValue == otherValue);
            Assert.False(basicValue.Equals(otherValue));
        }
    }
}
