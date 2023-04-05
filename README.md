# MerpBot

Hey! This is a bot repository that I use to test different things, as well as some commands that I really like to use on a daily basis.

You can take it and host it yourself if you'd like. I've written it so that you should be able to do that!

Licensed under [CC BY-NC-SA 4.0](https://creativecommons.org/licenses/by-nc-sa/4.0/)

## Building and running

Requirements:
	- The DotNet 7 Runtime and SDK, which you can find [here](https://dotnet.microsoft.com/en-us/download/dotnet/7.0)

It's pretty simple. Just run `dotnet build` inside the `./MerpBot` folder and it should build. The output is in `./Merpbot/bin/Debug/net7.0/`

You'll need to create a file in there named `Globals.yml` or else it **will not** start. Follow the template in `Globals_tmp.yml` to write it. Or read the next bit.

```yml
LogLevel: "Verbose" # Valid types in ascending order of verbosity: "Critical", "Error", "Warning", "Info", "Verbose", "Debug". I'd suggest leaving it to Verbose but not everyone likes that amount of verbosity, or maybe wants more. 

DebugGuild: 1 # This guild will have commands pushed to on startup.

Channels:
  ErrorChannel: 1 # The channel where errors will be sent.


StartupStatus:
  StatusType: "Playing" # Valid types: "Playing", "Watching", "Streaming", "Listening", "Competing"
  StatusMessage: "" # Anything you want. Has a character limit of 200.
```

You will also need an .env file with `TOKEN` as the bot's Discord token.

When done, just run the file named MerpBot.exe (or just MerpBot, depending on OS)

## Todo

	- Docker
	- Clean up code probably
	- More stuff I can't think of right now
	- Rebrand because the name is pretty dumb tbh