﻿


![Version](https://img.shields.io/badge/version-0.2.2-blue.svg)
![Unity](https://img.shields.io/badge/Unity-2022.3.4f1-black.svg)
![License](https://img.shields.io/badge/license-MIT-green.svg)

# TahaCore

**TahaCore** is a Unity codebase designed to enhance software development practices by offering utilities that 
facilitate dependency injection, serialization, ini-based configurations, and event bus communications.
Aimed at Unity developers, TahaCore simplifies common tasks, enabling cleaner, more maintainable, 
and scalable project structures.

## Features

- **Parallel Task Execution**: Includes `UniTask` for efficient parallel task execution, improving performance and responsiveness. [See UniTask](https://github.com/Cysharp/UniTask)
- **Dependency Injection**: Promotes loose coupling and modularity, making your Unity applications easier to test and maintain.
- **Serialization**: Supports efficient data serialization and deserialization, customized for Unity's environment.
- **Ini-based Configurations**: Provides a straightforward approach to manage application settings, leveraging ini files for easy configuration.
- **Ini-based Injection**: Allows you to inject types based on ini file configurations, providing a flexible and dynamic approach to dependency injection.
- **Event Bus**: Enables components to communicate effectively without direct dependencies, which reinforces Inversion of Control (IoC) principles.

## Installation

1. Download TahaCore.
2. Copy the following scoped registries to your project's `Packages/manifest.json` file:
    ```json
    {
      "scopedRegistries": [
        {
          "name": "package.openupm.com",
          "url": "https://package.openupm.com",
          "scopes": [
            "jp.hadashikick.vcontainer",
            "com.cysharp.unitask"
          ]
        }
      ]
    }
    ```
3. Go to the package manager in Unity. Click on the `+` button and select `Add package from disk...`.
4. Select the `package.json` file in the Assets/Package folder of TahaCore.
5. Verify that all project dependencies are met.

## Usage

### Dependency Injection

Dependency injection is a software design pattern that promotes loose coupling and modularity.
TahaCore is built on top a flexible and efficient dependency injection mechanism that provides:

- **Attribute-based Injection**: Use attributes to inject dependencies into your classes which makes
  easier to configure IoC container.
- **Registering with concrete types**: Registering concrete types to the IoC container directly will ensure implementation will
  ensure that dependencies are resolved correctly. This approach also simplifies creation of singletons without hiding the dependencies.
   ```csharp
      public interface ISerializer
      {
          string Serialize<T>(T obj);
          T Deserialize<T>(string data);
      }
      
      //Registering the UnityJsonSerializer as default implementation of ISerializer
      [ApplicationRuntimeRegistry(LifetimeType.Singleton, typeof(ISerializer))]
      public class UnityJsonSerializer : ISerializer
      {
          public string Serialize<T>(T obj)
          {
              return JsonUtility.ToJson(obj);
          }
      
          public T Deserialize<T>(string data)
          {
              return JsonUtility.FromJson<T>(data);
          }
      }
      
      //Dependencies can be injected to the InjectableMonoBehaviours by using the [Inject] attribute.
      public class PlayerDataManager : InjectableMonoBehaviour
      {
          //The registered ISerializer will be injected to the _serializer field.
          [Inject] private ISerializer _serializer;
          private PlayerData _playerData;
      
          protected override void Awake()
          {
              //Don't forget to call base.Awake() to ensure that the dependencies are injected.
              base.Awake();
              // Use the injected dependency safely
              string serializedData = _serializer.Serialize(_playerData);
          }
      } 
      
      //Dependencies of the other classes that are also registered to the IoC container 
      //can be injected by with the constructor injection.
      //Concrete types can be registered to the IoC container directly 
      //to ensure that dependencies are resolved correctly and,
      //to simplify creation of singletons without hiding the dependencies.
      [ApplicationRuntimeRegistry(LifetimeType.Singleton)]
      public class PlayerDataEvaluater
      {
          private ISerializer _serializer;
          public void PlayerDataEvaluater(ISerializer serializer)
          {
              _serializer = serializer;
          }
          
          // some more implementation
          // ...
          private void EvaluteAndSavePlayerData()
          {
              // Do some operations ..
              var playerData = _serializer.Serialize(serializedData);
              saveManager.Save(playerData);
          }
      }
   
   ```
- **Ini-based Injection**: Inject types based on ini file configurations,
  providing a flexible and dynamic approach to dependency injection. This feature unlocks
  configuring which types to inject based on ini file configurations at runtime.
   ```csharp
    //Registers the type to the IoC container with the specified lifetime and the type 
    //only if ConfigConditionAttribute is satisfied: 
    // Register if there is a config value with the key "BoolCondition" 
    // in the section "CONFIG_CONDITIONS" and the value is true.
    [ApplicationRuntimeRegistry(LifetimeType.Singleton)]
    [BoolConfigCondition("CONFIG_CONDITIONS", "BoolCondition", true)]
    public class BoolConditionalInjectionPositiveTestType
    {
        
    }
   ```
- **Manual Injection**: Manually inject dependencies into your objects.
  `IInjector` interface provides a method to manually inject dependencies
  into your object. This is particularly useful when you need to inject dependencies to
  objects that are created at runtime.
   ```csharp
   public class PolledFactory<Enemy> : Factory
   {
       [Inject] private readonly IInjector _injector;
            
       public Enemy Generate()
       {
           var enemy = new Enemy();
           _injector.Inject(enemy);
           return enemy;
       }
   }
   ```

### Ini-based Configuration

In addition to ini-based injection, TahaCore provides utilities for accessing
ini properties and sections. This feature allows user to store and retrieve
data efficiently at runtime.

There are currently two ways to access ini properties and sections:
1. **IConfigValueProvider:** This interface provides a method to access ini properties and sections
   with a simple api.
   ```csharp
   public class ConfigValueProviderExample : InjectableMonoBehaviour
   {
       [Inject] private IConfigValueProvider _configValueProvider;
       private void Start()
       {
           //Get the value of the "SomeKey" in the "SomeSection" section.
           string value = _configValueProvider.GetParam("SomeSection", "SomeKey");
           //Get the value of the "SomeKey" in the "SomeSection" section as an integer.
           int intValue = _configValueProvider.GetParamValue<int>("SomeSection", "SomeKey");
       }
   }
   ```
2. **ConfigSection Class (Suggested)**: This class provides a way to access ini sections via classes
   that inherit `ConfigSection`. This approach allows creating classes that are automatically
   populated with config values. Since this approach provides a type-safe way to access config values,
   it is strongly suggested.
   ```csharp
    // Injecting this config section will return a valid instance of TestConfig with
    // values populated from the ini file. 
    [ApplicationRuntimeRegistry(LifetimeType.Singleton)]
    [ConfigSection("TestConfig")]
    public class TestConfig : ConfigSection
    {
        [ConfigProperty("IntValue")] public int SomeInteger { get; set; }
        [ConfigProperty("BoolValue")] public bool SomeBoolean { get; set; }
        [ConfigProperty("FloatValue")] public float SomeFloat { get; set; }
        [ConfigProperty("StringValue")] public string SomeString { get; set; }
        
        [ParseWith(typeof(LongParser))]
        [ConfigProperty("LongValue")] public long SomeLong { get; set; }
        [ConfigProperty("IntArray")] public int[] SomeIntArray { get; set; }
    }
   ```
### Scene Event
- Scene Event mechanism is an event bus system that is meant to be used for communication
  between entities that are in the scene. A scene event implies an event that happens in the
  scene. Communicating using this mechanism encourages Inversion of Control (IoC) instead
  of the forward control mechanisms that are commonly advertised by Unity. It is impossible
  to write maintainable and flexible code when entities in the scene are directly referencing
  each other. So, this simple mechanism, when utilized correctly, can help to write
  decoupled code.
```csharp
    //Receives a scene event and logs a message when the event is received.
    public class TestSceneEventReceiver : SceneBehaviour
    {
      private void OnEnable()
      {
            SceneEventProvider.Subscribe<TestSceneEvent>(OnTestSceneEvent);
      }
      
      private void OnDisable()
      {
            SceneEventProvider.Unsubscribe<TestSceneEvent>(OnTestSceneEvent);
      }
      
      private void OnTestSceneEvent(TestSceneEvent testSceneEvent)
      {
            Debug.Log("TestSceneEvent received");
      }
    }
    
    //Sends a scene event when a button is clicked.
    public class SceneEventSender : SceneBehaviour
    {
        [SerializeField] private Button _button;
        private void Start()
        {
            _button.onClick.AddListener(
                () => SceneEventHistory.AddSceneEvent(new TestSceneEvent()));
        }
    }
```
### Logging
- TahaCore provides a simple logging interface that can be used to log messages.
- The purpose of this interface is replacing Debug.Log, Debug.LogWarning and Debug.LogError
  and providing a customizable logging mechanism.
```csharp
    public class TestLogger : InjectableMonoBehaviour
    {
        [Inject] private ILogger _logger;
        private void Start()
        {
            _logger.Log("This is a log message");
            _logger.LogWarning("This is a warning message");
            _logger.LogError("This is an error message");
        }
    }
```
- Logging can be directly performed with `TahaCoreApplicationRuntime.Log` api as well.
```csharp
    public class TestLogger : InjectableMonoBehaviour
    {
        private void Start()
        {
            TahaCoreApplicationRuntime.LogInfo("This is a log message");
            TahaCoreApplicationRuntime.LogWarning("This is a warning message");
            TahaCoreApplicationRuntime.LogError("This is an error message");
        }
    }
```
### Serialization (In progress)
- TahaCore provides a simple serialization and deserialization mechanism for deserializing unknown types.
#### Why is this needed?
In order to deserialize a type, to what type the data will be deserialized
must be known at compile time. This prevents implementing polymorphic mechanisms that utilize polymorphism.

Lets consider this example:
You want to develop a command mechanism where commands are provided from a web service.
The received json data meant to be deserialized to any type that implements `ICommand` interface.
Parameters of the command are the payload of the json data. In this case it is impossible
to know what `ICommand` implementation will be created with the received data
at runtime unless there is a field in the received json data that specifies the type of
the command. The mechanism that is provided by TahaCore (will) allow deserializing
the received data with an api like `_deserializer.Deserialize<ICommand>(receivedData)`.
```csharp
    public interface ICommand 
    {
        void Execute();
    }
    
    //Serializable type
    public class CommandA : ICommand
    {   
        //Serializable property
        public float Force { get; set; }
        
        //Serializable property
        public float Acceleration { get; set; }
      
        public void Execute()
        {
            //Do something
        }
    }
    
    //Serializable type
    public class CommandB : ICommand
    {   
        //Serializable property
        public float Duration { get; set; }
        
        //Serializable property
        public float Interval { get; set; }
      
        public void Execute()
        {
            //Do something
        }
    }
    
    public class CommandReceiver : InjectableMonoBehaviour
    {
        [Inject] private IDeserializer _deserializer;
        
        private UniTask ReceiveCommand()
        {
           var data = await GetCommandDataFromWebService();
           var command = (ICommand)_deserializer.Deserialize(data); 
           if(command == null)
           {
               TahaCoreApplicationRuntime.LogError("Deserialization failed");
           }
           command.Execute();
        }
    }
```


## Running Tests

TahaCore includes a suite of tests to ensure feature reliability. To run these tests:

1. Open the Unity Test Runner.
2. Select `PlayMode` tests for TahaCore.
3. Execute the tests to verify functionality.

## Dependencies

- Unity 2021.3 LTS or newer.
- UniTask 2.0.0 or newer.
- VContainer 1.13.0 or newer.

## License

TahaCore is available under the MIT License. See [License](Licenses/LICENSE) for more information.
