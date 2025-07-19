# SignalBus

A lightweight and type-safe signal (pub-sub) framework for Unity, inspired by the simplicity of event-driven design.

## ✨ Features

- 🔧 **Type-safe**: Publish and subscribe to signals by type.
- 🧼 **Minimal**: No reflection, no attributes, no codegen.
- 🔄 **Scoped Subscriptions**: Automatically unsubscribe listeners using `ISignalScope`.
- ⚡ **Fast & GC-friendly**: Uses pooled lists and delegates under the hood.
- 📦 **Unity-friendly**: Designed to play nicely with MonoBehaviours and DI frameworks.

---

## 📦 Installation

Add the source files to your Unity project, or embed them in your package:

```
Assets/
└── Scripts/
    └── Signals/
        ├── ISignalBus.cs
        ├── ISignalScope.cs
        ├── SignalBus.cs
        └── SignalScope.cs
```

---

## 🚀 Usage

### Basic Usage

```csharp
var bus = new SignalBus();

bus.Subscribe<MySignal>(OnMySignal);
bus.Publish(new MySignal { Message = "Hello!" });
bus.Unsubscribe<MySignal>(OnMySignal);

void OnMySignal(MySignal signal)
{
    Debug.Log(signal.Message);
}
```

```csharp
public class MySignal
{
    public string Message;
}
```

---

### Scoped Subscriptions

```csharp
using var scope = bus.CreateScope();

scope.Subscribe<MySignal>(signal =>
{
    Debug.Log($"Received: {signal.Message}");
});

// On dispose, all scoped subscriptions are removed
scope.Dispose();
```

This is perfect for managing temporary listeners (e.g., in a MonoBehaviour’s `OnEnable`/`OnDisable` pair).

---

## 🧪 Testing

A test suite using NUnit is included and covers:

- Subscribing and receiving signals
- Unsubscribing and ensuring no delivery
- Scoped subscription disposal
- Scoped delivery before disposal

---

## 📘 Interfaces

### `ISignalBus`

```csharp
void Subscribe<T>(Action<T> callback);
void Unsubscribe<T>(Action<T> callback);
void Publish<T>(T signal);
ISignalScope CreateScope();
```

### `ISignalScope : IDisposable`

```csharp
void Subscribe<T>(Action<T> callback);
void Unsubscribe<T>(Action<T> callback);
```

---

## 📄 License

MIT License © Travis R. French  
Free to use in commercial or personal Unity projects.
