# UWPMessenger

A simple **UWP** application that demonstrates how to send and store messages using **Twilio** and **SQLite** via **Entity Framework Core**. It provides services for messaging logic and Twilio integration.

---

## Overview

- **Send SMS Messages**: The app lets you enter a recipient phone number and message content, then sends the SMS using Twilio.
- **Store and Retrieve Messages**: All sent messages are stored in a local SQLite database. You can review them in the UI after sending.
- **Services**: Separation of concerns through distinct services (`MessageService` for data logic, `TwilioService` for Twilio integration).

---

## Main Features

1. **Message Sending**  
   - Enter the recipient number and message text.
   - Press “Send” to dispatch the SMS via **Twilio**.

2. **Local Storage**  
   - Uses **SQLite** with **EF Core** to store messages and Twilio confirmations.
   - Displays a list of previously sent messages (including confirmation codes).

3. **Services**  
   - **MessageService**: Handles the creation of database records and interaction with Twilio.
   - **TwilioService**: Encapsulates Twilio API calls and credentials.

---

## Technologies Used

- **UWP (Universal Windows Platform)** for the UI.
- **C#** and **.NET Standard** for core logic.
- **SQLite** + **EF Core** for local data persistence.
- **Twilio** API for sending messages and receiving confirmation codes.
- **NuGet Packages**:
  - `Microsoft.EntityFrameworkCore.Sqlite`
  - `Microsoft.EntityFrameworkCore.Tools`
  - `Twilio`
- **Visual Studio** for development and debugging.

---

## How It Works

1. **User Input**: You provide the phone number and message text on the main page.
2. **MessageService**: Receives the data, stores it in the `Message` table, and calls `TwilioService`.
3. **TwilioService**: Sends the SMS via Twilio and returns a confirmation code (`Sid`).
4. **MessageService (cont.)**: Stores the Twilio confirmation in the `SentMessage` table, linked to the original message.
5. **UI Update**: The app updates the message list, displaying both the message details and confirmation code.

---

## Getting Started

1. **Clone the Repository**  
   - Open the solution in Visual Studio.

2. **Set Up Database**  
   - EF Core with SQLite will create the `.db` file automatically in UWP’s local folder upon migration or first run.

3. **Configure Twilio**  
   - In the setting page, ensure your Twilio credentials (`Account SID`, `Auth Token`) and the `From` number are set. This uses Windows Credential Manager service.

4. **Run the App**  
   - Press F5 to build and run.  
   - Enter a valid phone number and a message, then click “Send”.

---

## License

This project is provided for demonstration purposes. You are free to adapt and expand it according to your needs.

