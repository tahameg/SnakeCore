# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [1.2.1] - 2024-08-01
### Added
- Dependency injection bug is fixed on InjectableMonoBehaviour.
- GameObjectPool now has a method overload that allows retrieving a 
GameObject on a specific position and rotation.

## [1.2.0] - 2024-07-30
### Added
- Scene Scope has been implemented. Scene Scope is a scope that is 
created for each scene and is destroyed when the scene is destroyed.
- EntryPoints are now supported. EntryPoints allow access to Unity's lifecycle events.

## [1.1.0] - 2024-07-24
### Added
- GameObjectPool has been implemented. It is a simple object pool that can be used to pool GameObjects.

## [1.0.0] - 2024-07-09
### Added
- IJSonSerializer has been implemented and is able to serialize and deserialize to unknown types based 
on the type information provided in the json.
- ConfigSection now falls back to the json deserializer if the value cannot be parsed to the primitive types. At this
mode, arrays and json objects are supported.
- TypeParser-based mechanism for customizing the way primitives are parsed has been abandoned. All primitive type parsing 
operations are now handled by `PrimitiveSerialization` static class.
- Name of the project has been changed to `SnakeCore`.

## [0.2.2] - 2024-01-25
### Added
- IJSonSerializer is declared and UnityJsonSerializer is implemented as default implementation.

## [0.2.1] - 2024-01-25

### Added
- SceneEvent mechanism is implemented.

## [0.1.1] - 2023-12-30

### Added
- Namespaces are rearranged.
- Config system has been reimplemented. 
    - `ITypeParser` interface has been introduced.
    - TypeParserRegistryAttribute has been introduced.
    - `ConfigSectionAttribute` has been introduced.
    - `ConfigPropertyAttribute` has been introduced.
    - Any class that inherits `ConfigSection` class and has `ConfigSectionAttribute`
    defined is automatically populated with config values.
    - String parameter of `ConfigPropertyAttribute` in each property of that class corresponds to a config value.
    - New TypeParsers can be introduced by implementing the `ITypeParser` interface. When these classes are 
    defined `TypeParserRegistryAttribute`, they can automatically be used with the ITypingProvider. 
    - See Readme.md for details on how the Config system is being used.

## [0.1.0] - 2023-11-19

### Added
- Package is initialized.
- Readme and Changelog are added.
- Dependency Injection mechanism is created. This has been implemented through the `TahaCoreApplicationRuntime`.
- `ApplicationRuntimeRegistry` attribute has been created to enable flexible registration.
- Ini file management system has been created. The system provide utilities for retrieving, caching the ini file content
and managing the ini file lifecycle.
- `ConfigConditionAttribute` has been created to allow config-based injection of types.
- `InjectableMonoBehaviour` has been created to allow injection to MonoBehaviours.
- Logging interface has been created and it is granted axcess via the TahaCoreApplicationRuntime. 