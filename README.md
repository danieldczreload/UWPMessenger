# UWPMessenger

UWPMessenger is a Universal Windows Platform (UWP) application designed to send and manage messages using the Twilio API. The application follows the principles of **Clean Architecture**, ensuring a clear separation of concerns, maintainability, and scalability. It allows users to send messages to specified phone numbers and view a history of sent messages along with their confirmation codes.

## 📦 Features

- **Send Messages:** Compose and send messages to any phone number via Twilio.
- **Message History:** View a list of all sent messages with details such as recipient, content, status, and confirmation codes.
- **Clean Architecture:** Organized into distinct layers (Domain, Application, Infrastructure, Presentation) for better maintainability and scalability.
- **Dependency Injection:** Utilizes dependency injection for managing dependencies across different layers.
- **Unit Testing:** Includes unit tests to ensure the reliability of business logic and data access layers.
- **Logging:** Implements logging to track application behavior and diagnose issues.

## 🏛 Architecture

UWPMessenger is structured following the **Clean Architecture** principles, dividing the application into distinct layers:

1. **Domain (`UWPMessenger.Domain`):**
   - **Purpose:** Contains the core business entities and rules.
   - **Components:** Entities like `Message` and `SentMessage`.

2. **Application (`UWPMessenger.Application`):**
   - **Purpose:** Defines the application's use cases and orchestrates interactions between the Domain and Infrastructure layers.
   - **Components:** Services such as `IMessageService` and `MessageService`.

3. **Infrastructure (`UWPMessenger.Infrastructure`):**
   - **Purpose:** Handles data persistence and integration with external services.
   - **Components:** Repositories (`IMessageRepository`, `MessageRepository`), `DbContext` (`MessageAppContext`), and external services (`ITwilioService`, `TwilioService`).

4. **Presentation (`UWPMessenger`):**
   - **Purpose:** Manages the user interface and user interactions.
   - **Components:** UWP pages (`MainPage`), ViewModels (`MainPageViewModel`), and UI controls.

5. **Tests (`UWPMessenger.Tests`):**
   - **Purpose:** Contains unit tests to verify the functionality of the Application and Infrastructure layers.
   - **Components:** Test classes for services and repositories.
