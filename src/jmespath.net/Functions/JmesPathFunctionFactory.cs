﻿using System;
using System.Collections.Generic;
using DevLab.JmesPath.Interop;

namespace DevLab.JmesPath.Functions
{
    public  class JmesPathFunctionFactory : IRegisterFunctions, IFunctionRepository
    {
        private readonly Dictionary<string, JFunction> functions_ = new Dictionary<string, JFunction>();
        private static IFunctionRepository repository_;

        public static IFunctionRepository Default {
            get
            {
                if (repository_ != null)
                    return repository_;
                var repo = new JmesPathFunctionFactory();
                repo
                    .Register<AbsFunction>()
                    .Register<ToNumberFunction>()
                    ;

                repository_ = repo;
                return repo;
            }
        } 

        public IRegisterFunctions Register(string name, JFunction function)
        {
            if (!functions_.ContainsKey(name))
                functions_.Add(name, function);
            else
                functions_[name] = function;

            return this;
        }

        public IRegisterFunctions Register<T>() where T : JFunction
        {
            var instance = Activator.CreateInstance<T>();
            Register(instance.Name,instance);

            return this;
        }

        public IEnumerable<string> Names => functions_.Keys;
        
        public JFunction this[string name] => functions_[name];

        public  bool Contains(string name)
        {
            return functions_.ContainsKey(name);
        }
    }
}