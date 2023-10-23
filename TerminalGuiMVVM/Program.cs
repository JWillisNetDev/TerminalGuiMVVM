using Terminal.Gui;
using TerminalGuiMVVM;

Application.Init();

Application.Run(new LoginView(new LoginViewModel()));

Application.Shutdown();