﻿using Castle.DynamicProxy;
using Core.CrossCuttingConcerns.Validation;
using Core.Utilities.Interceptors;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Aspects.Autofac.Validation
{
    public class ValidationAspect : MethodInterception //Aspect--methodun başında, sonunda, ortasında hata verecek olan yapı
    {
        private Type _validatorType;
        public ValidationAspect(Type validatorType)
        {
            //defensive coding
            if (!typeof(IValidator).IsAssignableFrom(validatorType))
            {
                throw new System.Exception("Bu bir doğrulama sınıfı değil");
            }

            _validatorType = validatorType;
        }
        protected override void OnBefore(IInvocation invocation) //öncesinde çalış
        {
            var validator = (IValidator)Activator.CreateInstance(_validatorType); //ProductValidatorı newledi
            var entityType = _validatorType.BaseType.GetGenericArguments()[0];//<Product> al
            var entities = invocation.Arguments.Where(t => t.GetType() == entityType); //methodun(add) argumanlarını gez
            foreach (var entity in entities)
            {
                ValidationTool.Validate(validator, entity);
            }
        }


    }
}
