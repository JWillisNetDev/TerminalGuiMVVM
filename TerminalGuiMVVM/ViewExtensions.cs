using System.Linq.Expressions;
using System.Reflection;
using NStack;
using Terminal.Gui;

namespace TerminalGuiMVVM;

public static class ViewExtensions
{
	public static void BindText<T>(this TextField textField, T source, Expression<Func<T, string>> selector)
	{
		// This is actually much slower than source-gen and should be optimized

		if (selector.Body is not MemberExpression { Member: PropertyInfo { CanWrite: true } property })
		{
			throw new InvalidOperationException();
		}

		Func<T, string> compiled = selector.Compile();
		textField.TextChanging += args =>
		{
			if (compiled(source) != args.NewText)
			{
				property.SetValue(source, (string)args.NewText);
			}
		};
	}

	public static void BindText2<T>(this TextField textField, T source, Expression<Func<T, string>> selector)
	{
		if (selector.Body is not MemberExpression { Member: PropertyInfo { CanWrite: true, DeclaringType: not null } property })
		{
			throw new InvalidOperationException();
		}
		
		Func<T, string> get = selector.Compile();

		// VM.Value = args.NewText
		var instance = Expression.Parameter(property.DeclaringType);
		var argument = Expression.Parameter(typeof(ustring));

		Action<T, ustring> set = Expression.Lambda<Action<T, ustring>>(
			Expression.Call(instance, property.GetSetMethod()!,
				Expression.Convert(argument, typeof(string))), false, instance, argument).Compile();

		textField.TextChanging += args =>
		{
			if (get(source) != (string)args.NewText)
			{
				set(source, args.NewText);
			}
		};
	}
}