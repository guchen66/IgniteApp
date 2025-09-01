using IT.Tangdao.Framework;
using StyletIoC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IgniteAdmin.Interfaces
{
    /// <summary>
    /// 基于Stylet的ITangdaoProvider实现。
    /// 这是核心适配器，让您的框架认为在用TangdaoProvider，实际在用Stylet容器。
    /// </summary>
    public class StyletTangdaoProvider : ITangdaoProvider
    {
        private readonly IContainer _styletContainer;

        public StyletTangdaoProvider(IContainer styletContainer)
        {
            _styletContainer = styletContainer ?? throw new ArgumentNullException(nameof(styletContainer));
        }

        public object Resolve(Type type)
        {
            try
            {
                return _styletContainer.Get(type);
            }
            catch (Exception ex)
            {
                // 如果Stylet容器无法解析， fallback 到Activator
                // 这确保了最大兼容性
                return Activator.CreateInstance(type);
            }
        }

        public object Resolve(Type type, params object[] impleType)
        {
            // 注意：Stylet的容器可能不支持这种传参方式
            // 这里退回到使用Activator，但可能会失去IOC benefits
            // 更好的做法是确保所有依赖都在容器中注册，而不是手动传入
            if (impleType == null || impleType.Length == 0)
                return _styletContainer.Get(type);

            return Activator.CreateInstance(type, impleType);
        }

        // 实现ITangdaoProviderBuilder接口的方法（根据您的接口定义可能需要实现）
        public ITangdaoProviderBuilder AsSingleton()
        {
            // Stylet有自己的生命周期管理，这里可能不需要实现
            return this;
        }

        public ITangdaoProviderBuilder AsTransient()
        {
            // Stylet有自己的生命周期管理，这里可能不需要实现
            return this;
        }
    }

    /// <summary>
    /// 基于Stylet的ITangdaoContainer实现。
    /// 这是一个空壳，主要为了提供ITangdaoProvider。
    /// </summary>
    public class StyletTangdaoContainer : ITangdaoContainer
    {
        private readonly ITangdaoProvider _provider;

        public StyletTangdaoContainer(ITangdaoProvider provider)
        {
            _provider = provider;
        }

        public ITangdaoProvider Builder()
        {
            // 直接返回基于Stylet的Provider
            return _provider;
        }

        // 以下注册方法在Stylet为主容器的情况下都是空操作
        // 因为注册应该在Stylet的ConfigureIoC中完成
        public ITangdaoContainer Register(Type serviceType, Type implementationType) => this;

        public ITangdaoContainer Register(Type implementationType) => this;

        public ITangdaoContainer Register(Type serviceType, Func<object> creator) => this;

        public ITangdaoContainer Register(Type type, Func<ITangdaoProvider, object> factoryMethod) => this;

        public ITangdaoContainer Register(string name) => this;

        // 这些方法返回ITangdaoContainerBuilder，需要返回this
        public ITangdaoContainerBuilder Register<TService, TImplementation>() where TImplementation : TService => this;

        public ITangdaoContainerBuilder Register<TImplementation>() => this;

        public void Register<TService>(Func<ITangdaoContainer, TService> factory)
        {
            // 在Stylet为主容器的情况下，注册应该通过Stylet的builder完成
        }
    }
}