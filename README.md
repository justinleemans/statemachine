# Statemachine - Easy statemachine package for Unity

A simple statemachine package for Unity

# Table of Contents

- [Installation](#installation)
- [Quick Start](#quick-start)
- [Contributing](#contributing)

# Installation

Currently the best way to include this package in your project is through the unity package manager. Add the package using the git URL of this repo: https://github.com/justinleemans/statemachine.git

# Quick Start

To get started with the statemachine first make sure you have an enum setup with all the states you want to support. See below for an example.

```c#
public enum State
{
	Initialization,
	Login,
	Menu,
	Game
}
```

## Instance

After you have defined your states you can create an instance of `StateMachine<TState>` where `TState` should be your state. You can use the optional parameter `initialState` to set which state to start from. If not specified the state machine will start from the first state in the list of states.

```c#
var stateMachine = new StateMachine<State>(State.Initialization);
```

## Behaviours

For every state you should make a new class deriving from `StateBehaviour`. This class will function as the central point of logic for your state.

```c#
public class InitializationStateBehaviour : StateBehaviour<State>
{
	public void OnEnter()
	{
		...
	}
	
	public void OnExit()
	{
		...
	}
}
```

The state behaviour will have to required methods to implement. `OnEnter` and `OnExit`. These methods can be used to handle everything you want to happen once a state transition happens. For example you could use this to subscribe and unsubscribe from events.

## Component Behaviours

Alternativly you could derive from `MonoStateBehaviour` if you want your behaviour class to be a component. This can be especially helpful if you want to use unity component related methods. This can for example help when building a UI where you want to easily switch windows.

## Configuration

Once you have made all the necesairy behaviours you can start configuring your state machine. You can use the `ConfigureState()` method on the state machine to set a behaviour per state. This method takes two parameters. First the state you want to configure and second the behaviour instance you want to assign to that state.

```c#
stateMachine.ConfigureState(State.Initialization, new InitializationStateBehaviour());
```

After all these state behaviours have been assigned you need to define the permissions. You can do this using the `SetPermission()` method. Doing this tells the state machine which states can be accessed from a certain state and which transitions are allowed. In the example below we tell the state machine to allow transitions from `Initialization` to `Login`.

```c#
stateMachine.SetPermission(State.Initialization, State.Login);
```

Note that this is a one way permission, if you want to allow the state machine to transition the opposite way you need to define another permission.

## Transitioning

To make the state machine transition from one state to another you can use the `Fire()` method. This method take a state as the only argument which is the state you want the state machine to transition to. This method is available in the `StateBehaviour` class as well as on the state machine directly.

```c#
stateMachine.Fire(State.Login);
```

The state machine also has two method you can use to add/remove a callback which gets called whenever the state machine transitions from one state to another. These methods are called `AddListener()` and `RemoveListener` and both take a delegate with two arguments of the type of your state. These arguments represent the original state and the state it transitioned to.

```c#
stateMachine.AddListener(OnTransitioned);
stateMachine.RemoveListener(OnTransitioned);

private void OnTransitioned(State source, State target)
{
	Debug.Log($"Transitioned from {source} to {target}!");
}
```

# Contributing

Currently I have no set way for people to contribute to this project. If you have any suggestions regarding improving on this project you can make a ticket on the GitHub repository or contact me directly.
