# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]
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