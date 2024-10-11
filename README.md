# Don't Touch the Spikes - C#

Developed as a university course project for C# Programming, using Godot 4.x. The goal was to incorporate as many C# features and syntactic sugar as possible, even if it didn’t always make sense in some places. The project was completed over the course of a few days.

## Installation

1. Clone the repository:
   ```bash
   git clone https://github.com/m1zukash1/dont-touch-the-spikes-cs.git
   ```
2. Open the project in the Godot Engine. It's that simple 🙃

## Contributing & Future Plans
This was developed as a university project with no future plans for further development. The project is purely here to showcase my work.

## Lecturer Provided Tasks (Translated into English)

- [x] **Created and applied your interface** (0.5 pt.)  
  _The Bird class uses `IFormattable`, which automatically turns the Bird class into an interface._

- [ ] **Correctly implemented IComparable<T>** (0.5 pt.)

- [ ] **Correctly implemented IEquatable<T>** (0.5 pt.)

- [x] **Correctly implemented IFormattable** (1 pt.)  
  _File: `Bird.cs` | Method: `ToString()`_

- [x] **Used 'switch' with the 'when' keyword** (0.5 pt.)  
  _File: `SpikeManager.cs` | Method: `GetSpikeCountBasedOnScore()`_

- [x] **Used 'Range' type** (0.5 pt.)  
  _File: `SpikeSpawner.cs` | Method: `GenerateSpikePositions()`_

- [ ] **Project consists of more than one assembly** (1 pt.)

- [x] **Used 'sealed' or 'partial' class** (0.5 pt.)  
  _All classes inherit from Godot-provided classes as `partial`._

- [x] **Used abstract class** (0.5 pt.)  
  _File: `ListExtensions.cs`_

- [x] **Used static constructor** (1 pt.)  
  _File: `CandyObject.cs`_

- [ ] **Used deconstructor** (0.5 pt.)

- [x] **Used operator overloading** (0.5 pt.)
  _File: `WiggleComponent.cs`_

- [x] **Used data structures from System.Collections or System.Collections.Generic** (1 pt.)  
  _File: `SpikeManager.cs` | using `List<SpikeObject>`_

- [x] **Used 'is' operator** (0.5 pt.)  
  _File: `DeathZoneComponent.cs` | Method: `OnBodyEntered()`_

- [ ] **Used default and named arguments** (0.5 pt.)

- [ ] **Used 'params' keyword** (0.5 pt.)

- [ ] **Implemented initialization using 'out' arguments** (1 pt.)

- [x] **Used delegates or lambda functions** (1.5 pt.)  
  _All Godot signals/events are defined as delegates._  
  _Lambda usage: File: `MenuController.cs` | Method: `StartIdleAnimation()`_

- [x] **Used bitwise operations** (1 pt.)  
  _File: `CandyManager.cs` | Method: `OnCandyCollected()`_

- [x] **Used operators `?.`, `?[]`, `??`, or `??=`** (0.5 pt.)  
  _File: `WiggleComponent.cs` | Method: `_Ready()`_

- [ ] **Used pattern matching** (1 pt.)

      
