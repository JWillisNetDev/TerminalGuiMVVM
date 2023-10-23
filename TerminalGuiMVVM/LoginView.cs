using NStack;
using Terminal.Gui;

namespace TerminalGuiMVVM;

public class LoginView : Window
{
	public LoginViewModel ViewModel { get; }

	public LoginView(LoginViewModel viewModel)
		: base("Login Example using Community Toolkit MVVM")
	{
		ViewModel = viewModel;

		var title = TitleLabel();
		var usernameLengthLabel = UsernameLengthLabel(title);
		var usernameInput = UsernameInput(usernameLengthLabel);
		var passwordLengthLabel = PasswordLengthLabel(usernameInput);
		var passwordInput = PasswordInput(passwordLengthLabel);
		var validationLabel = ValidationLabel(passwordInput);
		var loginButton = LoginButton(validationLabel);
		var clearButton = ClearButton(loginButton);
		var loginProgressLabel = LoginProgressLabel(clearButton);

		// Register binding for View -> ViewModel
		usernameInput.BindText2(ViewModel, vm => vm.UserName);
		passwordInput.BindText2(ViewModel, vm => vm.Password);

		// Register binding for ViewModel -> View
		ViewModel.PropertyChanged += (_, e) =>
		{
			switch (e.PropertyName)
			{
				case nameof(LoginViewModel.UserName):
					usernameInput.Text = ViewModel.UserName;
					break;

				case nameof(LoginViewModel.Password):
					passwordInput.Text = ViewModel.Password;
					break;

				case nameof(ViewModel.UserNameLength):
					usernameLengthLabel.Text = UsernameLengthText();
					break;

				case nameof(ViewModel.PasswordLength):
					passwordLengthLabel.Text = PasswordLengthText();
					break;
			}
		};

		loginButton.Enabled = false;
		ViewModel.TryLoginCommand.CanExecuteChanged += (_, _) =>
		{
			loginButton.Enabled = ViewModel.TryLoginCommand.CanExecute(null);
		};
		loginButton.Clicked += () => ViewModel.TryLoginCommand.Execute(null);

		clearButton.Clicked += () => ViewModel.ClearInputCommand.Execute(null);
	}

	private Label TitleLabel()
	{
		Label label = new("Login Form");
		Add(label);
		return label;
	}

	private TextField UsernameInput(View previous)
	{
		TextField usernameInput = new(ViewModel.UserName)
		{
			X = Pos.Left(previous),
			Y = Pos.Top(previous) + 1,
			Width = 40,
		};
		Add(usernameInput);
		return usernameInput;
	}

	private Label UsernameLengthLabel(View previous)
	{
		Label usernameLength = new()
		{
			X = Pos.Left(previous),
			Y = Pos.Top(previous) + 1,
			Width = 40,
			Text = UsernameLengthText(),
		};
		Add(usernameLength);
		return usernameLength;
	}

	private ustring UsernameLengthText() => ustring.Make($"UserName ({ViewModel.UserNameLength} characters)");

	private TextField PasswordInput(View previous)
	{
		TextField passwordInput = new(ViewModel.Password)
		{
			X = Pos.Left(previous),
			Y = Pos.Top(previous) + 1,
			Width = 40,
		};
		Add(passwordInput);
		return passwordInput;
	}

	private Label PasswordLengthLabel(View previous)
	{
		Label passwordLength = new()
		{
			X = Pos.Left(previous),
			Y = Pos.Top(previous) + 1,
			Width = 40,
			Text = PasswordLengthText()
		};
		Add(passwordLength);
		return passwordLength;
	}

	private ustring PasswordLengthText() => ustring.Make($"Password ({ViewModel.PasswordLength} characters)");

	private Label ValidationLabel(View previous)
	{
		ustring error = ustring.Make("Please enter your username and password.");
		ustring success = ustring.Make("Input is valid!");

		Label validationLabel = new(error)
		{
			X = Pos.Left(previous),
			Y = Pos.Top(previous) + 1,
			Width = 40,
		};
		Add(validationLabel);
		return validationLabel;
	}

	private Label LoginProgressLabel(View previous)
	{
		ustring progress = ustring.Make("Logging in...");
		ustring idle = ustring.Make("Press 'Login' to log in.");
		Label loginProgressLabel = new(idle)
		{
			X = Pos.Left(previous),
			Y = Pos.Top(previous) + 1,
			Width = 40,
		};
		Add(loginProgressLabel);
		return loginProgressLabel;
	}

	private Button LoginButton(View previous)
	{
		Button loginButton = new("Login")
		{
			X = Pos.Left(previous),
			Y = Pos.Top(previous) + 1,
			Width = 40,
		};
		Add(loginButton);
		return loginButton;
	}

	private Button ClearButton(View previous)
	{
		Button clearButton = new("Clear")
		{
			X = Pos.Left(previous),
			Y = Pos.Top(previous) + 1,
			Width = 40,
		};
		Add(clearButton);
		return clearButton;
	}
}