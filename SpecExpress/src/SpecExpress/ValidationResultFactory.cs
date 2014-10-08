﻿using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using SpecExpress.Enums;
using SpecExpress.MessageStore;
using SpecExpress.Rules;

namespace SpecExpress
{
    internal static class ValidationResultFactory
    {
        public static ValidationResult Create(RuleValidator validator, RuleValidatorContext context, IList<Object> parameterValues, object messageKey, IEnumerable<ValidationResult> nestedValidationResults = null)
        {
            string message = string.Empty;
            var messageService = new MessageService();

            if (String.IsNullOrEmpty(validator.Message))
            {
                var messageContext = new MessageContext(context, validator.GetType(), validator.Negate, validator.MessageStoreName, messageKey, validator.MessageFormatter);
                message = messageService.GetDefaultMessageAndFormat(messageContext, parameterValues);
            }
            else
            {
                //Since the message was supplied, don't get the default message from the store, just format it
                message = messageService.FormatMessage(validator.Message, context, parameterValues, validator.MessageFormatter);
            }

            //Override level if all the nested validation errors are Warnings

           
            if (nestedValidationResults != null && nestedValidationResults.All(vr => vr.Level == ValidationLevelType.Warn))
            {
                return new ValidationResult(context.PropertyInfo, message, ValidationLevelType.Warn, context.PropertyValue, nestedValidationResults);
            }
            else
            {
                return new ValidationResult(context.PropertyInfo, message, context.Level, context.PropertyValue, nestedValidationResults);
            }
           
            //return new ValidationResult(context.PropertyInfo, message, context.Level, context.PropertyValue, nestedValidationResults);
        }
    }
}
