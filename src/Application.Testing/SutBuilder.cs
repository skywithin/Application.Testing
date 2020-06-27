using AutoFixture;
using AutoFixture.AutoMoq;
using Moq;
using System;
using System.Collections.Generic;

namespace Application.Testing
{
    /// <summary>
    /// A lightweight AutoMoq wrapper if you don't want to use TestingContext
    /// </summary>
    public class SutBuilder<T> where T : class
    {
        private readonly Fixture _fixture = new Fixture();
        private readonly Dictionary<Type, Mock> _injectedMocks = new Dictionary<Type, Mock>();

        public SutBuilder(bool configureMembers = false)
        {
            _fixture.Customize(new AutoMoqCustomization { ConfigureMembers = configureMembers });
        }

        public void Inject<TObject>(TObject item)
        {
            _fixture.Inject(item);
        }

        public Mock<TObject> Get<TObject>() where TObject : class
        {
            var type = typeof(TObject);

            if (_injectedMocks.TryGetValue(type, out Mock mock))
            {
                return (Mock<TObject>)mock;
            }

            var newMock = new Mock<TObject>();

            _fixture.Inject(newMock.Object);
            _injectedMocks[type] = newMock;

            return newMock;
        }

        public T Build()
        {
            return _fixture.Create<T>();
        }
    }
}
