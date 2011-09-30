using System;
using System.ServiceModel;
using System.Collections.Generic;
using System.Reflection.Emit;
using Freetime.Base.Business.Implementable;

namespace Freetime.Base.Business
{
    internal class DataSessionFactory : IDataSessionFactory
    {
        private bool UseDefaultDataSession { get; set; }

        internal DataSessionFactory()
        {
            UseDefaultDataSession = true;
        }

        #region Commented - do not remove
        //TContract IDataSessionFactory.GetDataSession<TContract>() 
        //{
        //    var httpFactory =
        //        new ChannelFactory<TContract>(
        //          new BasicHttpBinding(),
        //          new EndpointAddress(
        //            string.Format("http://192.168.175.123:8000/FreetimeDataServices/{0}", typeof(TContract).Name)));
        //    var contract = httpFactory.CreateChannel();
        //    return contract;
        //}
        
        //TContract IDataSessionFactory.GetDataSession<TContract>(TContract defaultContract) 
        //{
        //    if (UseDefaultDataSession) return defaultContract;

        //    var contract = (this as IDataSessionFactory).GetDataSession<TContract>();
        //    return Equals(contract, null) ? defaultContract : contract;
        //}
        #endregion

        TContract IDataSessionFactory.GetDataSession<TContract>(ILogic logic, TContract defaultContract)
        {
            var blogic = PluginManagement.PluginManager.Current.GetBusinessLogic(logic.GetType().FullName);

            if (blogic == null)
                return defaultContract;

            var sessionType = Type.GetType(string.Format("{0}, {1}", blogic.DataSessionType, blogic.DataSessionAssembly));

            if(sessionType == null)
                throw new Exception(string.Format("Unknown type: {0}, {1}", blogic.DataSessionType, blogic.DataSessionAssembly));

            var sessionInstance = CreateContractInstance<TContract>(sessionType);

            return (Equals(sessionInstance, null)) 
                ? defaultContract 
                : sessionInstance;
        }

        #region Dynamic Type Creation - Modify at your Own Risk    
        private static TContract CreateContractInstance<TContract>(Type contractType)
        {
            if(contractType == null)
                throw new ArgumentNullException("contractType");

            if(!ContractDelegateCache.ContainsKey(contractType))
            {
                
                var dm = new DynamicMethod("CreateInstance", contractType, Type.EmptyTypes);//, contractType);

                var il = dm.GetILGenerator();
                
                il.DeclareLocal(contractType);
                
                var constructor = contractType.GetConstructor(Type.EmptyTypes);
                if(Equals(constructor, null))
                    throw new Exception(string.Format("Unable to create instance of type {0}", contractType.FullName));

                il.Emit(OpCodes.Newobj, constructor);
                il.Emit(OpCodes.Stloc_0);
                il.Emit(OpCodes.Ldloc_0);
                il.Emit(OpCodes.Ret);
                ContractDelegateCache.Add(contractType, dm.CreateDelegate(typeof(CreateContractInstanceDelegate)));
            }
            
            var method = ContractDelegateCache[contractType] as CreateContractInstanceDelegate;
            if(method == null)
                throw new Exception(string.Format("Unable to create instance of type {0}", contractType.FullName));

            var contract = (TContract) method();
            if(Equals(contract, null))
                throw  new Exception(string.Format("Unable to create instance of type {0}", contractType.FullName));
            return contract;
        }

        private delegate object CreateContractInstanceDelegate();

        private static Dictionary<Type, Delegate> s_cachedContractDelegates;

        private static Dictionary<Type, Delegate> ContractDelegateCache
        {
            get { return s_cachedContractDelegates = s_cachedContractDelegates ?? new Dictionary<Type, Delegate>(); }

        }

        #endregion
    }
}
