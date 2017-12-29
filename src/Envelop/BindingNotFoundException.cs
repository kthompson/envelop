using System;
using System.Collections.Generic;
using System.Text;

namespace Envelop
{
    /// <summary>
    /// This exception can occur when a binding is not found.
    /// </summary>
    public class BindingNotFoundException : InvalidOperationException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BindingNotFoundException" /> class.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="targetType">Type of the target.</param>
        /// <param name="message">The message.</param>
        internal BindingNotFoundException(IRequest request, Type targetType = null, string message = null)
            : base(GetMessageForRequest(request, targetType, message))
        {
        }

        private static string GetMessageForRequest(IRequest request, Type targetType, string message)
        {
            var sb = new StringBuilder();
            var fullfillments = new List<Tuple<Type, Type>>();
            var failedTarget = targetType;

            while (request != null)
            {
                fullfillments.Add(Tuple.Create(request.ServiceType, targetType));
                targetType = request.Target;
                request = request.ParentRequest;
            }

            fullfillments.Reverse();

            foreach (var r in fullfillments)
            {
                if (r.Item2 == null)
                {
                    sb.AppendFormat("Attempting to resolve '{0}' failed without finding a valid implementation", r.Item1.Name);
                    sb.AppendLine();

                    if (!string.IsNullOrEmpty(message))
                    {
                        sb.AppendLine(message);
                    }

                    return sb.ToString();
                }

                sb.AppendFormat("Attempting to resolve '{0}' with '{1}'", r.Item1.Name, r.Item2.Name);
                sb.AppendLine();
            }

            sb.AppendFormat("Failed to find a usable constructor for '{0}'", failedTarget.Name);


            if (!string.IsNullOrEmpty(message))
            {
                sb.AppendLine(); 
                sb.AppendLine(message);
            }

            return sb.ToString();
        }
    }
}