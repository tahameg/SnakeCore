


![Version](https://img.shields.io/badge/version-1.3.3-blue.svg)
![Unity](https://img.shields.io/badge/Unity-2022.3.4f1-black.svg)
![License](https://img.shields.io/badge/license-MIT-green.svg)

![Banner](SnakeCoreBanner.png)
# SnakeCore

**SnakeCore** is a Unity codebase designed to enhance software development practices by offering utilities that 
facilitate dependency injection, serialization, ini-based configurations, and event bus communications.
Aimed at Unity developers, SnakeCore simplifies common tasks, enabling cleaner, more maintainable, 
and scalable project structures.

## Features

- **Parallel Task Execution**: Includes `UniTask` for efficient parallel task execution, improving performance and responsiveness. [See UniTask](https://github.com/Cysharp/UniTask)
- **Dependency Injection**: Promotes loose coupling and modularity, making your Unity applications easier to test and maintain.
- **Serialization**: Supports efficient data serialization and deserialization, customized for Unity's environment.
- **Ini-based Configurations**: Provides a straightforward approach to manage application settings, leveraging ini files for easy configuration.
- **Ini-based Injection**: Allows you to inject types based on ini file configurations, providing a flexible and dynamic approach to dependency injection.
- **Event Bus**: Enables components to communicate effectively without direct dependencies, which reinforces Inversion of Control (IoC) principles.

## Installation

1. Download SnakeCore.
2. Copy the following scoped registries to your project's `Packages/manifest.json` file:
    ```json
    {
      "scopedRegistries": [
        {
          "name": "package.openupm.com",
          "url": "https://package.openupm.com",
          "scopes": [
            "jp.hadashikick.vcontainer",
            "com.cysharp.unitask",
            "com.snakelikecoding.snakecore"
          ]
        }
      ]
    }
    ```
3. Go to the package manager in Unity. Go to "My Registries". Find the "SnakeCore" package and click "Install".

## Usage

### Dependency Injection

Dependency injection is a software design pattern that promotes loose coupling and modularity.
SnakeCore is built on top a flexible and efficient dependency injection mechanism that provides:

- **Attribute-based Injection**: 
  - Use attributes to inject dependencies into your classes which makes
    easier to configure IoC container.
- **Registering with concrete types**: 
  - Registering concrete types is not suggested for most cases since it 
  harms the abstraction. On the other hand, this approach is better alternative to the singletons since 
  this approach doesn't hide the class depencencies.
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
- **Application Runtime Registry and Scene Runtime Registry**: 
  - Application Runtime provides a global scope for the
    registered types. Types that are registered to the Application Runtime are available throughout the application.
    Scene Runtime provides a scene-based scope for the registered types. Types that are registered to the Scene Runtime
    are created when the scene is loaded and are destroyed when the scene is unloaded.
    - **Application Runtime Registry**: 
      - Register types to the IoC container with the specified lifetime using the 
      `ApplicationRuntimeRegistry`. Also add `SnakeCoreApplicationRuntime` to the first scene of the application. 
      Application Runtime is preserved between the scenes. So, don't need to add `SnakeCoreApplicationRuntime` to the 
      other scenes.
      - Lifetime of the registration can be either `Singleton` or `Instanced`. A type that is registered as `Singleton`
      will be created once and will be available throughout the application.
      - A new object for every type that is registered as `Instanced` will be created each time it is requested.
          ```csharp
          //Registers the type to the IoC container with the specified lifetime.
          [ApplicationRuntimeRegistry(LifetimeType.Singleton)]
          public class SingletonType
          {
            
          }
          ```
    - **Scene Runtime Registry**:
      - Register types to the IoC container using the`SceneRuntimeRegistry` with the specified scene name. The scene 
      with the specified name must have a `SnakeCoreSceneRuntime` component.
      - Lifetime of the registration is always `Scoped`. This means, if multiple additive scenes are loaded, every 
      scene will have its own instance if the type is registered to both scenes.
      - Types that are registered to the Scene Runtime can access the types that are registered to the Application Runtime.
      ```csharp
       //Registers the type to the IoC container with the specified lifetime.
       [SceneRuntimeRegistry(sceneName: "SceneName")]
       public class SingletonType
       {
        
       }
       ```
    - **Registering Plain C# Entry Points**: 
      - Plain C# entry points are life-cycle methods that can be implemented in the classes that are registered t
    
- **Ini-based Injection**: 
  - Inject types based on ini file configurations,
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
   public class Factory<Enemy> : Factory
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
DI is built on top of [VContainer](https://vcontainer.hadashikick.jp/). Check out for detailed documentation.

### Ini-based Configuration

In addition to ini-based injection, SnakeCore provides utilities for accessing
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
- SnakeCore provides a simple logging interface that can be used to log messages.
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

- Logging can be directly performed with `SnakeCoreApplicationRuntime.Log` api as well.
```csharp
    public class TestLogger : InjectableMonoBehaviour
    {
        private void Start()
        {
            SnakeCoreApplicationRuntime.LogInfo("This is a log message");
            SnakeCoreApplicationRuntime.LogWarning("This is a warning message");
            SnakeCoreApplicationRuntime.LogError("This is an error message");
        }
    }
```
### Serialization (In progress)
- SnakeCore provides a simple serialization and deserialization mechanism for deserializing unknown types. This
allows polymorphic serialization where target type which the data will be deserialized to is specified by the data itself.
(See `IJsonSerializer`)

```csharp
    public interface ICommand 
    {
        void Execute();
    }
    
    /*
    {
        "$type" : "SomeGameProject.CommandA",
        "Force" : 10.0,
        "Acceleration" : 5.0
    } 
    */
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
    
    /*
    {
        "$type" : "SomeGameProject.CommandB",
        "Duration" : 12.0,
        "Interval" : 0.5
    } 
    */
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
           // Web service provides a command data in json format.
           var data = await GetCommandDataFromWebService();
            
           // Since the deserialization happens based on the `$Type` parameter,
           // the behaviour is specified at run-time by the data.
           var command = (ICommand)_deserializer.Deserialize(data); 
           if(command == null)
           {
               SnakeCoreApplicationRuntime.LogError("Deserialization failed");
           }
           command.Execute();
        }
    }
```


## Running Tests

SnakeCore includes a suite of tests to ensure feature reliability. To run these tests:

1. Open the Unity Test Runner.
2. Select `PlayMode` tests for SnakeCore.
3. Execute the tests to verify functionality.

## Dependencies

- Unity 2021.3 LTS or newer.
- UniTask 2.0.0 or newer.
- VContainer 1.13.0 or newer.

## License

SnakeCore is available under the MIT License. See [License](Licenses/LICENSE) for more information.
