﻿using System;
using System.Diagnostics.Contracts;
using Whathecode.System.Reflection.Emit;
using Whathecode.System.Reflection.Extensions;


namespace Whathecode.System.Windows.DependencyPropertyFactory.Attributes.Coercion
{
	/// <summary>
	///   When applied to a property in combination with <see cref = "DependencyPropertyAttribute" />
	///   specifies how the property should be coerced.
	/// </summary>
	/// <author>Steven Jeuris</author>
	[AttributeUsage( AttributeTargets.Property )]
	public class CoercionHandlerAttribute : AbstractBehaviorAttribute
	{
		public readonly IControlCoercion<object, object> GenericCoercion;


		/// <summary>
		///   Create a new attribute which hooks a coercion handler to a dependency property.
		/// </summary>
		/// <param name = "dynamicType">
		///   The type of the coercion handler.
		///   Should extend from <see cref = "AbstractControlCoercion{T, U}" />.</param>
		/// <param name = "constructorArguments">The arguments to pass to the constructor of the validation handler.</param>
		public CoercionHandlerAttribute( Type dynamicType, params object[] constructorArguments )
			: base( dynamicType, constructorArguments )
		{
			Contract.Requires( dynamicType.IsOfGenericType( typeof( IControlCoercion<,> ) ) );

			GenericCoercion = Proxy.CreateGenericInterfaceWrapper<IControlCoercion<object, object>>( DynamicInstance );
		}
	}
}