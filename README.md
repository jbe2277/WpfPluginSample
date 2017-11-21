# WPF Plugin Sample
This reference sample application shows how to create a Plugin Framework for a WPF application. The design goal is to enable dynamic composition of isolatable Plugins.

## Use Case
Enable customers and partners to extend the functionality of a software product. Example: Browser Extensions.

## Features
- Stability: Plugins are created in an own process. This way instabilities of the Plugins do not affect the Host application.
- Side-by-side execution: Plugins are deployed in a separate directory so that they can come with their own version of dependent libraries.
- Localization: The Plugins can be localized via satellite assemblies.
- Configuration: The `App.config` file is supported for Plugins.

## Known Issues
- Stability: The UI thread of the Host application and the Plugins are synchronized. Plugins can block the Host application.
- Performance: The communication between the Host and the Plugins is done via .NET Remoting. This has some negative impact on the performance.
- Limitation: A Plugin cannot show a modal Dialog. It can show Dialogs but they cannot be Modal because the Dialog cannot connect with the Host Window.
