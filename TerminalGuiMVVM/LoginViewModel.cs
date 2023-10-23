using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace TerminalGuiMVVM;

public partial class LoginViewModel : ObservableRecipient
{
	[ObservableProperty]
	[NotifyPropertyChangedFor(nameof(IsValid), nameof(UserNameLength))]
	[NotifyCanExecuteChangedFor(nameof(TryLoginCommand))]
	private string _UserName = string.Empty;

	public int UserNameLength => UserName.Length;

	[ObservableProperty]
	[NotifyPropertyChangedFor(nameof(IsValid), nameof(PasswordLength))]
	[NotifyCanExecuteChangedFor(nameof(TryLoginCommand))]
	private string _Password = string.Empty;
	
	public int PasswordLength => Password.Length;

	public bool IsValid => !string.IsNullOrEmpty(UserName) && !string.IsNullOrEmpty(Password);

	[RelayCommand(CanExecute = nameof(IsValid))]
	private void TryLogin()
	{
		Debug.WriteLine("Executing TryLogin command");
	}

	[RelayCommand]
	private void ClearInput()
	{
		UserName = string.Empty;
		Password = string.Empty;
	}
}